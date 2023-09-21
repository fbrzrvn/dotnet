using Amazon.SQS;
using Amazon.SQS.Model;

string queueName = args.Length == 1 ? args[0] : "customers";

CancellationTokenSource cts       = new();
AmazonSQSClient         sqsClient = new();

GetQueueUrlResponse queueUrlResponse = await sqsClient.GetQueueUrlAsync(queueName);

ReceiveMessageRequest receivedMessageRequest = new()
{
    QueueUrl              = queueUrlResponse.QueueUrl,
    AttributeNames        = new List<string> { "All" },
    MessageAttributeNames = new List<string> { "All" }
};

while (!cts.IsCancellationRequested)
{
    ReceiveMessageResponse? response = await sqsClient.ReceiveMessageAsync(receivedMessageRequest, cts.Token);

    foreach (Message message in response.Messages)
    {
        Console.WriteLine($"Message Id: {message.MessageId}");
        Console.WriteLine($"Message body: {message.Body}");

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(3000);
}