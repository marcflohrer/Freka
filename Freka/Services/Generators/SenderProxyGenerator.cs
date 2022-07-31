using Freka.Services.Base;

namespace Freka.Services.Generators;

public class SenderProxyGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public SenderProxyGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"{messageQueueInfo.OutputModel}SenderProxy") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.OutputModel;
        return $@"
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

namespace OutputProject.Messaging.{model}.Generated;

public class {model}SenderProxy : I{model}SenderProxy
{{
    private ServiceBusSender serviceBusSender;

    public {model}SenderProxy(IAzureClientFactory<ServiceBusClient> serviceBusClientFactory)
    {{
        var serviceBusClient = serviceBusClientFactory.CreateClient(""ServiceBusClient"");
        serviceBusSender = serviceBusClient.CreateSender(""{model}"");
    }}
    // handle received messages
    public async Task SendMessageAsync(ServiceBusMessage serviceBusMessage)
    {{
        await serviceBusSender.SendMessageAsync(serviceBusMessage);
    }}
}}
";
    }
}
