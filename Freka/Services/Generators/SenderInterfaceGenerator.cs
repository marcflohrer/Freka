using Freka.Services.Base;

namespace Freka.Services.Generators;

public class SenderInterfaceGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public SenderInterfaceGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"I{messageQueueInfo.OutputModel}Sender") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.OutputModel;
        return $@"
namespace OutputProject.Messaging.{model}.Generated;

public interface I{model}Sender
{{
    public Task SendMessageAsync(Models.{model} {model!.ToLower()});
}}
";
    }
}
