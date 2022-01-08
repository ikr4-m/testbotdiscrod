using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Interactions;

namespace TestBotDiscrod.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordShardedClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;

        public CommandHandlerService(DiscordShardedClient client,
            InteractionService commands, IServiceProvider service)
        {
            this._client = client;
            this._commands = commands;
            this._services = service;
        }

        public async Task InitializeAsync()
        {
            // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await this._commands.AddModulesAsync(Assembly.GetEntryAssembly(), this._services);

            this._client.InteractionCreated += this.HandleInteraction;
            this._client.ShardReady += this.RegisterInteractionToGuild;
        }

        private async Task RegisterInteractionToGuild(DiscordSocketClient client)
        {
#if DEBUG
            await this._commands.RegisterCommandsToGuildAsync(Global.Constant.GUIDDevelopment);
#else
            await this._commands.RegisterCommandsGloballyAsync();
#endif
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new ShardedInteractionContext(this._client, arg);
                await this._commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                if (arg.Type == InteractionType.ApplicationCommand)
                    await arg.GetOriginalResponseAsync()
                        .ContinueWith(async msg => await msg.Result.DeleteAsync());
            }
        }
    }
}
