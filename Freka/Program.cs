using Freka.Services;
using Freka.Services.Generators;
using Freka.Services.Generators.Tests;

public partial class Program
{
    public static void Main(string[] args)
    {
        GenerateMessagingSourceCodeFiles(args);
    }

    private static void GenerateMessagingSourceCodeFiles(string[] args)
    {
        // Parse and validate arguments.
        var commandLineInfo = CommandLineParser.Parse(args);

        // Build folder and file paths for queues defined in frekafile
        var modelAndQueueNamesList = commandLineInfo.frekaFile!.Messaging!.Input!.Select(ff => new MessageQueueInfo(
                    ff.MessageName!,
                    ff.QueueName!,
                    ff.Output?.MessageName,
                    ff.Output?.QueueName!)).ToList();

        // Generate source code for queues defined in frekafile
        modelAndQueueNamesList.GenerateMessagingSourceCode(commandLineInfo.ProjectDir);
        modelAndQueueNamesList.GenerateMessagingTestSourceCode(commandLineInfo.ProjectDir);
    }
}