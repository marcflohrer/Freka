using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.Response.Generated;

public interface IResponseSenderProxy
{
    Task SendMessageAsync(ServiceBusMessage serviceBusMessage);
}
