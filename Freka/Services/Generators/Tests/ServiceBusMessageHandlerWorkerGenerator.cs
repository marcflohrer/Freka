using Freka.Services.Base;

namespace Freka.Services.Generators.Tests;

public class ServiceBusMessageHandlerWorkerGenerator : StaticSourceCodeGenBase, IStaticSourceCodeGen
{
    public ServiceBusMessageHandlerWorkerGenerator(DirectoryInfo sourceDirectory)
        : base(sourceDirectory, "ServiceBusMessageHandlerWorker") { }

    public string GenerateSourceCode(IReadOnlyList<MessageQueueInfo> messageQueueInfo)
    {
        return $@"// Inspired by: https://github.com/andrewjpoole/azure-messaging-servicebus-handler-tests
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Hosting;
using OutputProject.Messaging;

namespace OutputProject.Tests.ServiceBusTests.TestFramework.ServiceBusMessageHandler
{{
    public abstract class ServiceBusMessageHandlerWorker : IHostedService
    {{
        private readonly ServiceBusClient _serviceBusClient;
        private ServiceBusProcessor? _serviceBusProcessor;
        private readonly string _queueName;
        private readonly int _initialBackoffInMs;

        public abstract Task OnReceiveMessage(ProcessMessageEventArgs args);

        public ServiceBusMessageHandlerWorker(
            ServiceBusClient serviceBusClient,
            string queueName,
            int intInitialBackoffInMs = 2000
        )
        {{
            _serviceBusClient = serviceBusClient;
            _queueName = queueName;
            _initialBackoffInMs = intInitialBackoffInMs;

            Console.WriteLine($""Constructor called"");
        }}

    public async Task StartAsync(CancellationToken cancellationToken)
    {{
        Console.WriteLine($""Starting service"");

        _serviceBusProcessor = _serviceBusClient.CreateProcessor(_queueName);

        try
        {{
            _serviceBusProcessor.ProcessMessageAsync += MessageHandler;
            _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

            Console.WriteLine($""Starting to consume from {{_queueName}} "");

            await _serviceBusProcessor.StartProcessingAsync(cancellationToken);

        }}
        catch (Exception e)
        {{
            Console.WriteLine($""Exception thrown during startup"", e);
            throw;
        }}

        Console.WriteLine($""Entering keep alive"");
    }}

    public async Task StopAsync(CancellationToken cancellationToken)
    {{
        if (_serviceBusProcessor is not null)
            await _serviceBusProcessor.DisposeAsync().ConfigureAwait(false);

        await _serviceBusClient.DisposeAsync().ConfigureAwait(false);
    }}

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {{
        if (args == null) throw new ArgumentNullException(nameof(args));

        try
        {{
            await OnReceiveMessage(args);
            await args.CompleteMessageAsync(args.Message);
        }}
        catch (PermanentException e)
        {{
            Console.WriteLine($""PermanentError thrown while processing message, message will be dead lettered: {{e}}"");
            await args.DeadLetterMessageAsync(args.Message, e.Message);
        }}
        catch (Exception e)
        {{
            Console.WriteLine($""Error thrown while processing message: {{e}}"");
            await args.AbandonMessageAsync(args.Message);
        }}
    }}

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {{
        var errorDetails = new Dictionary<string, string>
            {{
                {{nameof(args.EntityPath), args.EntityPath}},
                {{nameof(args.FullyQualifiedNamespace), args.FullyQualifiedNamespace}},
                {{nameof(args.ErrorSource), args.ErrorSource.ToString()}},
            }};

        Console.WriteLine($""Error handling message. {{ string.Join(Environment.NewLine, errorDetails)}}{{ Environment.NewLine }}{{ args.Exception }}"");

        return Task.CompletedTask;
    }}
}}
}}

";
    }
}

