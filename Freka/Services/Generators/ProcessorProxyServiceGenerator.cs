using System.Text;
using Freka.Services.Base;

namespace Freka.Services.Generators;

public class ProcessorProxyServiceGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public ProcessorProxyServiceGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"{messageQueueInfo.Model}ProcessorProxy") { }

    public string GenerateSourceCode(MessageQueueInfo maqn)
    {
        var senderDeclaration = new StringBuilder();
        var senderAssignment = new StringBuilder();
        var senderSendMessageCommand = new StringBuilder();
        var inputModel = maqn.Model;
        var outputModelName = maqn.OutputModel;
        var constructorParameters = new StringBuilder($"I{inputModel}Processor {inputModel.ToLower()}Processor");
        if (!string.IsNullOrEmpty(outputModelName))
        {
            constructorParameters.Append($", I{outputModelName}Sender {outputModelName.ToLower()}Sender");
            senderDeclaration.Append($@"{Environment.NewLine}    public I{outputModelName}Sender {outputModelName}Sender {{ get; }}");
            senderAssignment.Append($@"{Environment.NewLine}        {outputModelName}Sender = {outputModelName.ToLower()}Sender;");
            senderSendMessageCommand.Append($@"
            // send another message
            await {outputModelName}Sender.SendMessageAsync(new {outputModelName}.Models.{outputModelName}
            {{
                IsSuccessful = true,
                ErrorMessage = string.Empty
            }});");
        }
        return $@"using Azure.Messaging.ServiceBus;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging.{maqn.Model}.Generated;

public class {maqn.Model}ProcessorProxy : I{maqn.Model}ProcessorProxy
{{
    public I{maqn.Model}Processor {maqn.Model}Processor {{ get; }}{senderDeclaration}

    private int _initialBackoffInMillis = 2000;

    public MessageProcessorProxy({constructorParameters})
    {{
        {maqn.Model}Processor = {maqn.Model.ToLower()}Processor;{senderAssignment}
    }}
    // handle received messages
    public async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {{
        try
        {{
            await MessageProcessor.ProcessMessageAsync(args);
            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);{senderSendMessageCommand}
        }}
        catch (PermanentException permanentException)
        {{
            Console.WriteLine(permanentException.Message);
            // complete the message. message is deleted from the queue. 
            await args.DeadLetterMessageAsync(args.Message, string.Empty, string.Empty, CancellationToken.None);
        }}
        catch (Exception exception)
        {{
            Console.WriteLine(exception.Message);

            // complete the message. message is deleted from the queue. 
            await args.AbandonMessageAsync(args.Message, null, CancellationToken.None);
        }}
    }}

    // handle any errors when receiving messages
    public Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {{
        MessageProcessor.ProcessErrorAsync(args);
        return Task.CompletedTask;
    }}
}}

";
    }
}
