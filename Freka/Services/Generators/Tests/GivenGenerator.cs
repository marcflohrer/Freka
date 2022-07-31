using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class GivenGenerator : InputQueueSourceGenBase, IInputQueueSourceCodeGen
{
    public GivenGenerator(DirectoryInfo sourceDirectory, MessageQueueInfo messageQueueInfo)
        : base(sourceDirectory, messageQueueInfo, $"Given") { }

    public string GenerateSourceCode(MessageQueueInfo messageQueueInfo)
    {
        var model = messageQueueInfo.Model;
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using Moq;
using OutputProject.Messaging;

namespace OutputProject.Tests.ServiceBusTests.{model}ProcessorTests.TestElements;

public class Given
{{
    private readonly TestHost _host;

    public Given(TestHost host)
    {{
        _host = host;
    }}

    public static Given OnThe(TestHost host) => new(host);
    public Given And() => this;

    public Given ATestPayloadIsGenerated(out string testPayload)
    {{
        testPayload = Guid.NewGuid().ToString();

        return this;
    }}

    public Given The{model}ProcessorWillThrowAPermanentException()
    {{
        _host.{model}Processor.Setup(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()))
            .ThrowsAsync(new PermanentException(""something permanent has gone wrong!""));

        return this;
    }}

    public Given The{model}ProcessorWillThrowANumberOfTransientExceptions(int times = 1)
    {{
        var sequence = _host.{model}Processor
                 .SetupSequence(x => x.ProcessMessageAsync(It.IsAny<ProcessMessageEventArgs>()));
        for (int i = 0; i < times; i++)
        {{
            sequence.ThrowsAsync(new Exception(""something transient has gone wrong!""));
        }}
        sequence.Returns(Task.CompletedTask);

        return this;
    }}
}}

";
    }
}

