using Freka.Services.Base;

namespace Freka.Services.Generators;

public class ProcessorInterfaceGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public ProcessorInterfaceGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"I{messageQueueInfo.Model}Processor") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"using Azure.Messaging.ServiceBus;

namespace OutputProject.Messaging.{model}.Generated;

public interface I{model}Processor
{{
    public Task ProcessMessageAsync(ProcessMessageEventArgs args);
    public void ProcessErrorAsync(ProcessErrorEventArgs processErrorEventArgs);
}}

";
    }
}
