namespace Identity.Application.Commands.Register;

using Common.DTOs;
using Domain.Enum;
using Domain.Events;
using ErrorOr;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Infrastructure.Broker;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ErrorOr<IdentityUserDto>>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    // private readonly IPublisher                _eventBus;
    private readonly IMessageBroker            _messageBroker;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IMessageBroker            messageBroker,
        // IPublisher                eventBus,
        IMapper mapper,
        ILogger logger
    )
    {
        _userManager   = userManager;
        _messageBroker = messageBroker;
        // _eventBus    = eventBus;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ErrorOr<IdentityUserDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var result = await CreateUserAsync(request);
        var user   = result.Value;

        await AddRoleToUserAsync(user);

        var userCreatedEventData = new UserCreatedEventData(user);
        var userCreatedEvent     = new UserCreatedEvent(userCreatedEventData);
        await _messageBroker.PublishAsync(userCreatedEvent);
        // await _eventBus.Publish(userCreatedEvent, cancellationToken);

        var userResult = _mapper.Map<IdentityUserDto>((user, Role.RegularUser.Name));

        return ErrorOrFactory.From(userResult);
    }

    private async Task<ErrorOr<IdentityUser>> CreateUserAsync(RegisterUserCommand request)
    {
        var user = _mapper.Map<IdentityUser>(request);

        var createUserResult = await _userManager.CreateAsync(user, request.Password);

        if (createUserResult.Succeeded == false)
        {
            var createUserErrors = string.Join(", ", createUserResult.Errors.Select(error => error.Description));
            _logger.Error("Failed registering user. Reasons: {@Errors}", createUserErrors);

            return Error.Unexpected(description: "User registration failed. Errors: " + createUserErrors);
        }

        _logger.Information("Successfully registered user: {@User}", user);

        return ErrorOrFactory.From(user);
    }

    private async Task<ErrorOr<Success>> AddRoleToUserAsync(IdentityUser user)
    {
        var addRoleResult = await _userManager.AddToRoleAsync(user, Role.RegularUser.Name);

        if (addRoleResult.Succeeded)
        {
            return Result.Success;
        }

        var addRoleErrors = string.Join(", ", addRoleResult.Errors.Select(error => error.Description));
        _logger.Error("Failed to assign role {@Role}. Reasons: {@Errors}", Role.RegularUser.Name, addRoleErrors);

        return Error.Unexpected(
            description: $"Failed to assign role {Role.RegularUser.Name} to user. Errors: {addRoleErrors}"
        );
    }
}
