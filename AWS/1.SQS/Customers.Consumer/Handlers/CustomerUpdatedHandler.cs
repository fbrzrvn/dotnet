using Customers.Consumer.Messages;
using MediatR;

namespace Customers.Consumer.Handlers;

public class CustomerUpdatedHandler : IRequestHandler<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedHandler> _logger;

    public CustomerUpdatedHandler(ILogger<CustomerUpdatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerUpdated request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(request.GitHubUsername);

        return Unit.Task;
    }
}