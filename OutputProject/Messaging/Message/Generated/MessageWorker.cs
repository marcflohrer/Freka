namespace OutputProject.Messaging.Message.Generated;

public class MessageWorker : BackgroundService
{
    public MessageWorker(MessageQueue messageServiceBusQueue)
    {
        MessageQueue = messageServiceBusQueue;
    }

    public MessageQueue MessageQueue { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await MessageQueue.StartProcessing(stoppingToken);
    }
}

