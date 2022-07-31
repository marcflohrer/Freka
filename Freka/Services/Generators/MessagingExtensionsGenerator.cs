using System.Text;
using Freka.Services.Base;

namespace Freka.Services.Generators;

public class MessagingExtensionsGenerator : StaticSourceCodeGenBase, IStaticSourceCodeGen
{
    public MessagingExtensionsGenerator(DirectoryInfo sourceDirectory)
        : base(sourceDirectory, "MessagingExtensions") { }

    public string GenerateSourceCode(IReadOnlyList<MessageQueueInfo> messageQueueInfo)
    {
        var queueServiceRegistration = new StringBuilder();
        foreach (var iq in messageQueueInfo)
        {
            queueServiceRegistration = new StringBuilder($@"services.AddSingleton<I{iq.Model}ProcessorProxy, {iq.Model}ProcessorProxy>();
        services.AddSingleton<I{iq.Model}Processor, {iq.Model}Processor>();
        services.AddSingleton<{iq.Model}Queue>();");
            var returnStatement = $"return services.AddHostedService<{iq.Model}Worker>();";
            if (!string.IsNullOrEmpty(iq.OutputModel))
            {
                returnStatement = $@"
        services.AddHostedService<{iq.Model}Worker>();
        services.AddSingleton<I{iq.OutputModel}Sender, {iq.OutputModel}Sender>();
        return services.AddSingleton<I{iq.OutputModel}SenderProxy, {iq.OutputModel}SenderProxy>();";
            }
            queueServiceRegistration.Append(returnStatement);
        }
        return $@"using Microsoft.Extensions.Azure;
using OutputProject.Messaging.Message;
using OutputProject.Messaging.Message.Generated;
using OutputProject.Messaging.Response.Generated;

namespace OutputProject.Messaging;

public static class ServiceBusExtensions
{{
    private static IServiceCollection AddServiceBus(this IServiceCollection services, IConfiguration configuration)
    {{
        services.AddAzureClients(clientsBuilder =>
        {{
            clientsBuilder.AddServiceBusClient(configuration.GetSection(""ServiceBus"").GetValue<string>(""ConnectionString""))
              // (Optional) Provide name for instance to retrieve by with DI
              .WithName(""ServiceBusClient"")
              // (Optional) Override ServiceBusClientOptions (e.g. change retry settings)
              .ConfigureOptions(options =>
              {{
                  options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
                  options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(30);
                  options.RetryOptions.MaxRetries = 30;
              }});
        }});
        return services;
    }}

    private static IServiceCollection AddMessageProcessor(this IServiceCollection services)
    {{
        {queueServiceRegistration}
    }}

    public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
    {{
        services.AddServiceBus(configuration);
        services.AddMessageProcessor();
        return services;
    }}
}}

";
    }
}

