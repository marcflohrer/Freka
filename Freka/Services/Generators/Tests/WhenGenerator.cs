using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class WhenGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public WhenGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"When") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler;

namespace OutputProject.Tests.ServiceBusTests.{model}ProcessorTests.TestElements;

public class When
{{
    private readonly TestHost _host;

    public When(TestHost host)
    {{
        _host = host;
    }}

    public static When OnThe(TestHost host) => new(host);
    public When And() => this;

    public When A{model}IsSentToInputQueue(string testPayload, bool simulateRetries = false)
    {{
        var payload = new TestQueueMessage(testPayload);

        if (simulateRetries)
        {{
            _host.TestableInputQueueMessageProcessor.SendMessageWithRetries(payload, _host.NumberOfSimulatedServiceBusMessageRetries).GetAwaiter().GetResult();
        }}
        else
        {{
            var task = _host.TestableInputQueueMessageProcessor.SendMessage(payload);
            task.GetAwaiter().GetResult();
        }}

        return this;
    }}
}}

";
    }
}

