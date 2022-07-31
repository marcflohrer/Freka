using Freka.Services.Base;

namespace Freka.Services.Generators;

public class WorkerGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{

    public WorkerGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"{messageQueueInfo.Model}Worker") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"namespace OutputProject.Messaging.{model}.Generated;

public class {model}Worker : BackgroundService
{{
    public {model}Worker({model}Queue {model.ToLower()}ServiceBusQueue)
    {{
        {model}Queue = {model.ToLower()}ServiceBusQueue;
    }}

    public {model}Queue {model}Queue {{ get; }}

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {{
        await {model}Queue.StartProcessing(stoppingToken);
    }}
}}

";
    }
}
