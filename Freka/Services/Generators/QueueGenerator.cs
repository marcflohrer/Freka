using Freka.Services.Base;

namespace Freka.Services.Generators;

public class QueueGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public QueueGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"{messageQueueInfo.Model}Queue") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace OutputProject.Messaging.{model}.Generated;

public class {model}Queue
{{
    private readonly ServiceBusProcessor? serviceBusProcessor;

    public {model}Queue(I{model}ProcessorProxy {model.ToLower()}ProcessorProxy, IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
    {{
        var serviceBusClient = serviceBusClientFactory.CreateClient(""ServiceBusClient"");
        serviceBusProcessor = serviceBusClient.CreateProcessor(""{model}"", new ServiceBusProcessorOptions
        {{
            PrefetchCount = 1,
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1,
            ReceiveMode = ServiceBusReceiveMode.PeekLock
        }});

        serviceBusProcessor.ProcessMessageAsync += {model.ToLower()}ProcessorProxy!.ProcessMessageAsync;
        serviceBusProcessor.ProcessErrorAsync += {model.ToLower()}ProcessorProxy!.ProcessErrorAsync;
    }}

    public async Task StartProcessing(CancellationToken cancellationToken)
    {{
        await serviceBusProcessor!.StartProcessingAsync(cancellationToken);
    }}
}}

";
    }
}
