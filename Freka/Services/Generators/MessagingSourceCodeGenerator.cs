using Freka.Services.Base;

namespace Freka.Services.Generators;

public static class MessagingSourceCodeGenerator
{
    public static void GenerateMessagingSourceCode(this List<MessageQueueInfo> messageQueueInfoList, DirectoryInfo projectDirectory)
    {
        List<IStaticSourceCodeGen> sourceCodeFiles = new();
        var messagingDir = new DirectoryInfo(Path.Join(projectDirectory.FullName, "Messaging"));

        // Generate code for source files
        sourceCodeFiles.Add(new MessagingExtensionsGenerator(messagingDir));
        sourceCodeFiles.Add(new PermanentExceptionGenerator(messagingDir));
        foreach (var file in sourceCodeFiles)
        {
            file.WriteToFile(messageQueueInfoList);
        }

        const string generatedDirName = "Generated";
        List<InputQueueSourceGenBase> srcCodeList = new();
        foreach (MessageQueueInfo msgQueue in messageQueueInfoList)
        {
            var inputDir = new DirectoryInfo(Path.Join(messagingDir.FullName, msgQueue.Model, generatedDirName));
            srcCodeList.Add(new ProcessorInterfaceGenerator(inputDir, msgQueue));
            srcCodeList.Add(new ProcessorProxyInterfaceGenerator(inputDir, msgQueue));
            srcCodeList.Add(new ProcessorProxyServiceGenerator(inputDir, msgQueue));
            srcCodeList.Add(new QueueGenerator(inputDir, msgQueue));
            srcCodeList.Add(new WorkerGenerator(inputDir, msgQueue));

            if (string.IsNullOrEmpty(msgQueue.OutputModel))
            {
                var outputQueueDirectory = new DirectoryInfo(Path.Join(messagingDir.FullName, msgQueue.OutputModel!, generatedDirName));
                srcCodeList.Add(new SenderInterfaceGenerator(outputQueueDirectory, msgQueue));
                srcCodeList.Add(new SenderProxyInterfaceGenerator(outputQueueDirectory, msgQueue));
                srcCodeList.Add(new SenderProxyGenerator(outputQueueDirectory, msgQueue));
            }
        }
        foreach (var src in srcCodeList)
        {
            src.WriteToFile();
        }
    }
}

