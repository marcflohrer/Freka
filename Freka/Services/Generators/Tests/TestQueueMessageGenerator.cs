using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class TestQueueMessageGenerator : StaticSourceCodeGenBase, IStaticSourceCodeGen
{
    public TestQueueMessageGenerator(DirectoryInfo sourceDirectory)
        : base(sourceDirectory, "TestQueueMessage") { }

    public string GenerateSourceCode(IReadOnlyList<MessageQueueInfo> messageQueueInfo)
    {
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
namespace OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler
{{
    public class TestQueueMessage
    {{
        public string Message {{ get; }}

        public TestQueueMessage(string message)
        {{
            Message = message;
        }}
    }}
}}

";
    }
}

