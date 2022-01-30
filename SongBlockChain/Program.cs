using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SongBlockChain.Modules.Commands;
using SongBlockChain.Persistence;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SongBlockChain
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddHttpClient()
                //.AddMediatR(typeof(Program))
                .AddDbContext<SongOwnerContext>(opt => opt.UseInMemoryDatabase("myDataBase"))
                .BuildServiceProvider();

            string token = "OTM0NTA5Nzk5MDkyMTUwMzAy.YexIEg.8LNg8W1mXyn1CLX0r85Ki3MSnJU";


            Console.WriteLine("Registering commands...");
            await RegisterCommandsAsync();

            Console.WriteLine("Logging in...");
            await _client.LoginAsync(Discord.TokenType.Bot, token);

            Console.WriteLine("Starting Client...");
            await _client.StartAsync();

            Console.WriteLine("Client started.");
            await Task.Delay(-1);
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            _client.MessageReceived += CheckSpotifyIdAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            
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
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess)
            {
                Console.WriteLine(result.ErrorReason);
            }
            

        }
    }
}
