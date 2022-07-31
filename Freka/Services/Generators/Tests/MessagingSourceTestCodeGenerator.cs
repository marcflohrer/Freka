using Freka.Services.Base;

namespace Freka.Services.Generators.Tests
{
    public static class MessagingSourceTestCodeGenerator
    {
        public static void GenerateMessagingTestSourceCode(this List<MessageQueueInfo> messageQueueInfoList, DirectoryInfo projectDirectory)
        {
            List<IStaticSourceCodeGen> sourceCodeFiles = new();
            var messagingDir = new DirectoryInfo(Path.Join($"{projectDirectory.FullName}.Tests", "MessagingTests"));
            var testFrameworkDir = new DirectoryInfo(Path.Join(messagingDir.FullName, "TestFramework"));
            var serviceBusMessageHandlerDir = new DirectoryInfo(Path.Join(testFrameworkDir.FullName, "ServiceBusMessageHandler"));

            // Generate code for source files
            sourceCodeFiles.Add(new ServiceBusMessageHandlerWorkerGenerator(serviceBusMessageHandlerDir));
            sourceCodeFiles.Add(new TestQueueHandlerGenerator(serviceBusMessageHandlerDir));
            sourceCodeFiles.Add(new TestQueueMessageGenerator(serviceBusMessageHandlerDir));

            var testSupportDir = new DirectoryInfo(Path.Join(testFrameworkDir.FullName, "TestSupport"));
            sourceCodeFiles.Add(new TestableProcessMessageEventArgsGenerator(testSupportDir));
            sourceCodeFiles.Add(new TestableServiceBusProcessorGenerator(testSupportDir));

            foreach (var file in sourceCodeFiles)
            {
                file.WriteToFile(messageQueueInfoList);
            }

            List<InputQueueSourceGenBase> srcCodeList = new();
            foreach (MessageQueueInfo msgQueue in messageQueueInfoList)
            {
                var queueTestDir = new DirectoryInfo(Path.Join(messagingDir.FullName, $"{msgQueue.Model}ProcessorTests"));
                srcCodeList.Add(new ProcessorTestGenerator(queueTestDir, msgQueue));

                var testElementsDir = new DirectoryInfo(Path.Join(queueTestDir.FullName, "TestElements"));
                srcCodeList.Add(new GivenGenerator(testElementsDir, msgQueue));
                srcCodeList.Add(new TestHostGenerator(testElementsDir, msgQueue));
                srcCodeList.Add(new ThenGenerator(testElementsDir, msgQueue));
                srcCodeList.Add(new WhenGenerator(testElementsDir, msgQueue));
                if (string.IsNullOrEmpty(msgQueue.OutputModel))
                {
                }
            }
            foreach (var src in srcCodeList)
            {
                src.WriteToFile();
            }
        }
    }
}

