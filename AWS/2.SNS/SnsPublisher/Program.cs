using System.Text.Json;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using SnsPublisher;

AmazonSimpleNotificationServiceClient snsClient = new();

CustomerCreated customer = new()
{
    Id             = Guid.NewGuid(),
    FullName       = "Fabrizio Ervini",
    Email          = "fabri.es018@gmail.com",
    GitHubUsername = "fab-rvn",
    DateOfBirth    = new DateTime(1987, 2, 27)
};

Topic? topicArnResponse = await snsClient.FindTopicAsync("customers");

PublishRequest publishRequest = new()
{
    TopicArn = topicArnResponse.TopicArn,
    Message  = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue { DataType = "String", StringValue = nameof(CustomerCreated) }
        }
    }
};

await snsClient.PublishAsync(publishRequest);