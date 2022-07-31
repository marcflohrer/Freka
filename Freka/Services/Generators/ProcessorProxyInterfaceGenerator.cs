using Freka.Services.Base;

namespace Freka.Services.Generators;

public class ProcessorProxyInterfaceGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public ProcessorProxyInterfaceGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo modelAndQueueNames)
        : base(sourceDirectory, modelAndQueueNames, $"I{modelAndQueueNames.Model}ProcessorProxy") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.{model}.Generated;

public interface I{model}ProcessorProxy
{{
    Task ProcessMessageAsync(ProcessMessageEventArgs args);
    Task ProcessErrorAsync(ProcessErrorEventArgs args);
}}

";
    }
}
