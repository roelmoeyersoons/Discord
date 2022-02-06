using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SongBlockChain.Modules.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Core
{
    public class DiscordBot
    {
        private readonly IOptions<BotOptions> _options;
        private readonly ILogger _logger;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;

        public DiscordBot(IOptions<BotOptions> options, DiscordSocketClient client, CommandService commands) //, ILogger logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public async Task Run(IServiceProvider serviceProvider, string[] args)
        {
            Console.WriteLine("Registering commands...");
            await RegisterCommandsAsync(serviceProvider);

            Console.WriteLine("Logging in...");
            await _client.LoginAsync(Discord.TokenType.Bot, _options.Value.ClientSecret);

            Console.WriteLine("Starting Client...");
            await _client.StartAsync();

            Console.WriteLine("Client started.");
            await Task.Delay(-1);
        }


        public async Task RegisterCommandsAsync(IServiceProvider provider)
        {
            _client.MessageReceived += (message) => HandleCommandAsync(message, provider);
            _client.MessageReceived += CheckSpotifyIdAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);

        }

        private async Task CheckSpotifyIdAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message is null
                || message.Author.IsBot
                || message.Channel.Name != "general"
                || message.Content.StartsWith("|addsong")) //hack for now
                return;

            var context = new SocketCommandContext(_client, message);

            var detectedId = Authenticated.GrabSongId(context, message.Content);

            if (detectedId != null)
            {
                Console.WriteLine("song detected, posting songId");
                await message.Channel.SendMessageAsync(detectedId);
            }
        }

        private async Task HandleCommandAsync(SocketMessage arg, IServiceProvider provider)
        {
            var message = arg as SocketUserMessage;

            var argPos = 0;
            if (message is null || !message.HasStringPrefix("|", ref argPos) || message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, provider);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }


        }
    }
}
