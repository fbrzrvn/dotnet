using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS                    _sqs;
    private readonly IOptions<QueueSettings>       _queueSettings;
    private readonly IMediator                     _mediator;
    private readonly ILogger<QueueConsumerService> _logger;


    public QueueConsumerService
    (
        IAmazonSQS                    sqs,
        IOptions<QueueSettings>       queueSettings,
        IMediator                     mediator,
        ILogger<QueueConsumerService> logger
    )
    {
        _sqs           = sqs;
        _queueSettings = queueSettings;
        _mediator      = mediator;
        _logger        = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        GetQueueUrlResponse? queueUrlResponse = await _sqs.GetQueueUrlAsync(_queueSettings.Value.Name, stoppingToken);

        ReceiveMessageRequest receivedMessageRequest = new()
        {
            QueueUrl              = queueUrlResponse.QueueUrl,
            AttributeNames        = new List<string> { "All" },
            MessageAttributeNames = new List<string> { "All" },
            MaxNumberOfMessages   = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            ReceiveMessageResponse? response = await _sqs.ReceiveMessageAsync(receivedMessageRequest, stoppingToken);

            foreach (Message message in response.Messages)
            {
                string? messageType = message.MessageAttributes["MessageType"].StringValue;

                Type? type = Type.GetType($"Customers.Consumer.Messages.{messageType}");

                if (type is null)
                {
                    _logger.LogWarning("Unknown message type: {messageType}", messageType);
                    continue;
                }

                ISqsMessage typedMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;

                try
                {
                    await _mediator.Send(typedMessage, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Message failed during processing.");
                    continue;
                }

                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}