using Freka.Services.Base;

namespace Freka.Services.Generators;

public class SenderProxyInterfaceGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public SenderProxyInterfaceGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"I{messageQueueInfo.OutputModel}SenderProxy") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.OutputModel;
        return $@"
using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.{model}.Generated;

public interface I{model}SenderProxy
{{
    Task SendMessageAsync(ServiceBusMessage serviceBusMessage);
}}
";
    }
}
