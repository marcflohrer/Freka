using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.Message.Generated;

public interface IMessageProcessor
{
    public Task ProcessMessageAsync(ProcessMessageEventArgs args);
    public void ProcessErrorAsync(ProcessErrorEventArgs processErrorEventArgs);
}

