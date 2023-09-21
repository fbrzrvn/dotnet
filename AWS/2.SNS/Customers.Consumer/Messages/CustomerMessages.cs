using MediatR;

namespace Customers.Consumer.Messages;

public class CustomerCreated : ISqsMessage
{
    public required Guid     Id             { get; init; }
    public required string   FullName       { get; init; }
    public required string   Email          { get; init; }
    public required string   GitHubUsername { get; init; }
    public required DateTime DateOfBirth    { get; init; }
}

public class CustomerUpdated : ISqsMessage
{
    public required Guid     Id             { get; init; }
    public required string   FullName       { get; init; }
    public required string   Email          { get; init; }
    public required string   GitHubUsername { get; init; }
    public required DateTime DateOfBirth    { get; init; }
}

public class CustomerDeleted : ISqsMessage
{
    public required Guid Id { get; init; }
}