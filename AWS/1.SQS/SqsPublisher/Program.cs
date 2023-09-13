using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

AmazonSQSClient sqsClient = new();

CustomerCreated customer = new()
{
    Id             = Guid.NewGuid(),
    FullName       = "Fabrizio Ervini",
    Email          = "fabri.es018@gmail.com",
    GitHubUsername = "fab-rvn",
    DateOfBirth    = new DateTime(1987, 2, 27)
};

GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

SendMessageRequest sendMessageRequest = new()
{
    QueueUrl    = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue { DataType = "String", StringValue = nameof(CustomerCreated) }
        }
    }
};

await sqsClient.SendMessageAsync(sendMessageRequest);