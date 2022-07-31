using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace OutputProject.Messaging.Message.Generated;

public class MessageQueue
{
    private readonly ServiceBusProcessor? serviceBusProcessor;

    public MessageQueue(IMessageProcessorProxy messageProcessorProxy, IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
    {
        var serviceBusClient = serviceBusClientFactory.CreateClient("ServiceBusClient");
        serviceBusProcessor = serviceBusClient.CreateProcessor("Message", new ServiceBusProcessorOptions
        {
            PrefetchCount = 1,
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1,
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        });

        serviceBusProcessor.ProcessMessageAsync += messageProcessorProxy!.ProcessMessageAsync;
        serviceBusProcessor.ProcessErrorAsync += messageProcessorProxy!.ProcessErrorAsync;
    }

    public async Task StartProcessing(CancellationToken cancellationToken)
    {
        await serviceBusProcessor!.StartProcessingAsync(cancellationToken);
    }
}

