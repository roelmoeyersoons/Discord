using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SongBlockChain.Modules.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SongBlockChain.Core
{
    public class DiscordBot : IHostedService
    {
        private readonly IOptions<BotOptions> _options;
        private readonly ILogger<DiscordBot> _logger;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;

        public DiscordBot(IOptions<BotOptions> options, DiscordSocketClient client, CommandService commands, IServiceProvider provider, ILogger<DiscordBot> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _provider = provider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {

            _logger.LogInformation("Registering commands...");

            _client.MessageReceived += HandleCommandAsync;
            _client.MessageReceived += CheckSpotifyIdAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);

            _logger.LogInformation("Logging in...");
            await _client.LoginAsync(Discord.TokenType.Bot, _options.Value.ClientSecret);

            _logger.LogInformation("Starting Client...");
            await _client.StartAsync();

            _logger.LogInformation("Client started.");
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

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            var argPos = 0;
            if (message is null || !message.HasStringPrefix("|", ref argPos) || message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _provider);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }


        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.DisposeAsync();
        }
    }
}
