using Microsoft.AspNetCore;

namespace OutputProject;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
        .UseKestrel(options =>
        {
            options.ListenAnyIP(5000);
        })
        .ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("secrets/appsettings.json");
            config.AddEnvironmentVariables();
        })
        .UseStartup<Startup>()
        .Build();
}

