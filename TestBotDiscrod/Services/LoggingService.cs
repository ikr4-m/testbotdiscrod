using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace TestBotDiscrod.Services
{
    public class LoggingService
    {
        public LoggingService(DiscordShardedClient client, InteractionService command)
        {
            client.Log += this.ClientLog;
            command.SlashCommandExecuted += this.SlashCommandExecuted;
        }

        private Task ClientLog(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo cmdInfo, IInteractionContext ctx, IResult result)
        {
            Console.WriteLine($"{ctx.User} executing {cmdInfo.Name}");

            return Task.CompletedTask;
        }
    }
}
