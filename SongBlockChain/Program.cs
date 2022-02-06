using Discord.Commands;
using Discord.WebSocket;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SongBlockChain.Core;
using SongBlockChain.Modules.Commands;
using SongBlockChain.Persistence;
using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Options;

namespace SongBlockChain
{
    

    class Program
    {
        public IConfiguration Config { get; }


    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            var serviceCollection = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddHttpClient()
                //.AddOptions<BotOptions>().Configure<IConfiguration>((settings, config) =>
                //{
                //    config.GetSection("Dataverse").Bind(settings);
                //})
                //.Configure<BotOptions>(Config.GetSection(BotOptions.Client))
                //.AddMediatR(typeof(Program))
                .AddDbContext<SongOwnerContext>(opt => opt.UseInMemoryDatabase("myDataBase"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                //.AddEnvironmentVariables()
                .Build();

            serviceCollection.Configure<BotOptions>(configuration.GetSection(BotOptions.Client));

            _services = serviceCollection.BuildServiceProvider();

            var opts = _services.GetService<IOptions<BotOptions>>();



            Console.WriteLine("Registering commands...");
            await RegisterCommandsAsync();

            Console.WriteLine("Logging in...");
            await _client.LoginAsync(Discord.TokenType.Bot, opts.Value.ClientSecret);

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
