using System.Text.Json;
using Azure.Messaging.ServiceBus;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging.Message;

public partial class ResponseSender : IResponseSender
{
    public ResponseSender(IResponseSenderProxy responseSenderProxy)
    {
        ResponseSenderProxy = responseSenderProxy;
    }

    public IResponseSenderProxy ResponseSenderProxy { get; }

    public async Task SendMessageAsync(Response.Models.Response response)
    {
        var serializedResponse = JsonSerializer.Serialize(response);
        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(serializedResponse);
        await ResponseSenderProxy.SendMessageAsync(serviceBusMessage);
    }
}
