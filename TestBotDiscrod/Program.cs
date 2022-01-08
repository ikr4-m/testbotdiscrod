using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace TestBotDiscrod
{
    public partial class Program
    {
        private readonly DiscordSocketConfig _config;

        static void Main() => new Program().Run().GetAwaiter().GetResult();

        public Program()
        {
            this._config = new DiscordSocketConfig { TotalShards = 1 };
        }

        public async Task Run()
        {
            using var services = ConfigureServices();
            var client = services.GetRequiredService<DiscordShardedClient>();

            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DISCORD_TOKEN"));
            await client.StartAsync();


            await Task.Delay(-1);
        }
    }
}