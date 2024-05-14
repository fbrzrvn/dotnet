namespace Identity.Application.Commands.Create;

using Common.DTOs;
using Domain.Enum;
using Domain.Events;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Infrastructure.Broker;

public class CreateIdentityUserCommandHandler : IRequestHandler<CreateIdentityUserCommand, ErrorOr<IdentityUserDto>>
{
    // private readonly IPublisher                _eventBus;
    private readonly ILogger                   _logger;
    private readonly IMapper                   _mapper;
    private readonly IMessageBroker            _messageBroker;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateIdentityUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IMessageBroker            messageBroker,
        // IPublisher                eventBus,
        IMapper mapper,
        ILogger logger
    )
    {
        _userManager   = userManager;
        _messageBroker = messageBroker;
        // _eventBus      = eventBus;
        _mapper        = mapper;
        _logger        = logger;
        _messageBroker = messageBroker;
    }

    public async Task<ErrorOr<IdentityUserDto>> Handle(
        CreateIdentityUserCommand request,
        CancellationToken         cancellationToken
    )
    {
        request = request with { Role = request.Role ?? Role.RegularUser.Name };

        var result = await CreateUserAsync(request);
        var user   = result.Value;

        await AddRoleToUserAsync(user, request.Role);

        var userCreatedEventData = new UserCreatedEventData(user);
        var userCreatedEvent     = new UserCreatedEvent(userCreatedEventData);
        await _messageBroker.PublishAsync(userCreatedEvent);
        // await _eventBus.Publish(userCreatedEvent, cancellationToken);

        var userDto = _mapper.Map<IdentityUserDto>((user, request.Role));

        return ErrorOrFactory.From(userDto);
    }

    private async Task<ErrorOr<IdentityUser>> CreateUserAsync(CreateIdentityUserCommand request)
    {
        var user = _mapper.Map<IdentityUser>(request);

        var createUserResult = await _userManager.CreateAsync(user, request.Password);

        if (createUserResult.Succeeded == false)
        {
            var createUserErrors = string.Join(", ", createUserResult.Errors.Select(error => error.Description));
            _logger.Error("Failed to create user. Reasons: {@Errors}", createUserErrors);

            return Error.Validation(description: $"Failed to create user. Reasons: {createUserErrors}");
        }

        _logger.Information("Successfully created user: {@User}", user);

        return ErrorOrFactory.From(user);
    }

    private async Task<ErrorOr<Success>> AddRoleToUserAsync(IdentityUser user, string role)
    {
        var addRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (addRoleResult.Succeeded)
        {
            return ErrorOrFactory.From(Result.Success);
        }

        var addRoleErrors = string.Join(", ", addRoleResult.Errors.Select(error => error.Description));
        _logger.Error("Failed to assign role '{@Role}' to user. Reasons: {@Errors}", role, addRoleErrors);

        return Error.Unexpected(description: $"Failed to assign role '{role}' to user. Reasons: {addRoleErrors}");
    }
}
