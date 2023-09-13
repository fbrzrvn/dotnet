using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS              _sqs;
    private readonly IOptions<QueueSettings> _queueSettings;
    private          string?                 _queueUrl;

    public SqsMessenger(IOptions<QueueSettings> queueSettings, IAmazonSQS sqs)
    {
        _queueSettings = queueSettings;
        _sqs           = sqs;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        string queueUrl = await GetQueueUrlAsync();

        SendMessageRequest? sendMessageRequest = new()
        {
            QueueUrl    = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue { DataType = "String", StringValue = typeof(T).Name }
                }
            }
        };

        return await _sqs.SendMessageAsync(sendMessageRequest);
    }

    private async Task<string> GetQueueUrlAsync()
    {
        if (_queueUrl is not null)
        {
            return _queueUrl;
        }

        GetQueueUrlResponse? queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name);
        _queueUrl = queueUrlResponse.QueueUrl;

        return _queueUrl;
    }
}