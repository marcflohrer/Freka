using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.Message.Generated;

public interface IMessageProcessorProxy
{
    Task ProcessMessageAsync(ProcessMessageEventArgs args);
    Task ProcessErrorAsync(ProcessErrorEventArgs args);
}

