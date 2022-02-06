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


        static async Task Main(string[] args) 
        {
            var serviceCollection = new ServiceCollection()
                .AddHttpClient()
                .AddTransient<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                //.AddOptions<BotOptions>().Configure<IConfiguration>((settings, config) =>
                //{
                //    config.GetSection("Dataverse").Bind(settings);
                //})
                //.Configure<BotOptions>(Config.GetSection(BotOptions.Client))
                //.AddMediatR(typeof(Program))
                .AddDbContext<SongOwnerContext>(opt => opt.UseInMemoryDatabase("myDataBase"))
                .AddTransient<DiscordBot>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                //.AddEnvironmentVariables()
                .Build();

            serviceCollection.Configure<BotOptions>(configuration.GetSection(BotOptions.Client));

            var provider = serviceCollection.BuildServiceProvider();
            await provider.GetRequiredService<DiscordBot>().Run(provider, args);
        }
    }
}
