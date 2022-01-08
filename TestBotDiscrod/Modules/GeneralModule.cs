using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace TestBotDiscrod.Modules
{
    public class GeneralModule : InteractionModuleBase<ShardedInteractionContext>
    {
        [SlashCommand("ping", "Ping!")]
        public async Task Ping()
        {
            await this.RespondAsync("Pong!");
        }

        [SlashCommand("respond", "Talk as bot")]
        public async Task Respond(string message)
        {
            await this.RespondAsync(message);
        }

        [SlashCommand("summontest", "Summon test button")]
        public async Task SummonTestButton()
        {
            var builder = new ComponentBuilder()
                .WithButton("Click me!", "test-button", ButtonStyle.Danger);

            await this.RespondAsync(text: "Here it is!", components: builder.Build());
        }

        [SlashCommand("summonidtest", "Summon test (but only user with id can summon it)")]
        public async Task SummonIDTestButton()
        {
            var builder = new ComponentBuilder()
                .WithButton("Only my master can click this shit", $"test-button:{this.Context.User.Id}")
                .Build();

            await this.RespondAsync(text: "Here it is!", components: builder);
        }

        [ComponentInteraction("test-button")]
        public async Task TestButton()
        {
            await this.Context.Interaction.RespondAsync("Mantap bosku");
        }

        [ComponentInteraction("test-button:*")]
        public async Task TestButtonID(string id)
        {
            if (this.Context.User.Id == ulong.Parse(id))
            {
                await this.Context.Interaction.RespondAsync("This is my master");
            }
            else
            {
                await this.Context.Interaction.RespondAsync("Who is you?");
            }
        }
    }
}
