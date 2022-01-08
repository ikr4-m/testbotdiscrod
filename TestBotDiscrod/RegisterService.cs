using Discord.WebSocket;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using TestBotDiscrod.Services;

namespace TestBotDiscrod
{
    public partial class Program
    {
        private IServiceCollection SetCollector()
        {
            // Add your service here
            return new ServiceCollection()
                .AddSingleton(new DiscordShardedClient(this._config))
                .AddSingleton<LoggingService>()
                .AddSingleton<InteractionService>()
                .AddSingleton<CommandHandlerService>();
        }

        private object[] SetExludeImport()
        {
            // If you want to exclude import provider, add to this object.
            return new object[]
            {
                typeof(DiscordShardedClient),
                typeof(CommandHandlerService)
            };
        }

        private async void SetExtraStep(ServiceProvider build)
        {
            // If you have some extra step to initialize your service.
            // First of all, add your service in exlude and set the
            // "extra step" in here.

            // Initialize Command Handler async
            await build.GetRequiredService<CommandHandlerService>().InitializeAsync();
        }

        private ServiceProvider ConfigureServices()
        {
            var services = this.SetCollector();
            var build = services.BuildServiceProvider();
            var exclude = this.SetExludeImport();
            this.SetExtraStep(build);

            foreach (var service in services.ToArray())
            {
                if (exclude.Contains(service.ServiceType)) continue;
                build.GetRequiredService(service.ServiceType);
            }

            return build;
        }
    }
}
