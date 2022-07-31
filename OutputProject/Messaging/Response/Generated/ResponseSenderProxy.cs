using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace OutputProject.Messaging.Response.Generated;

public class ResponseSenderProxy : IResponseSenderProxy
{
    private ServiceBusSender serviceBusSender;

    public ResponseSenderProxy(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
    {
        var serviceBusClient = serviceBusClientFactory.CreateClient("ServiceBusClient");
        serviceBusSender = serviceBusClient.CreateSender("Response");
    }
    // handle received messages
    public async Task SendMessageAsync(ServiceBusMessage serviceBusMessage)
    {
        await serviceBusSender.SendMessageAsync(serviceBusMessage);
    }
}
