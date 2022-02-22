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
using Microsoft.Extensions.Hosting;
using System.Threading;
using Song.Application.Core;
using Song.Application.Songs.Commands;
using Song.Application.Repositories;
using Song.Persistence.Blockchain;

namespace SongBlockChain
{
    

    class Program
    {
        static async Task Main(string[] args) 
        {
            //createdefaulthostbuilder sets default and environment and DI container -> overridden with configureservices
            using (var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) => { 
                services.AddDiscordBot();
                var configurationRoot = context.Configuration;
                services.Configure<BotOptions>(configurationRoot.GetSection(BotOptions.Client));

            }).Build())
            {
                await host.RunAsync();
                //await host.WaitForShutdownAsync();
            }
        }
        
    }

    public static class Startup
    {
        public static IServiceCollection AddDiscordBot(this IServiceCollection services)
        {
            services
                .AddHttpClient()
                .AddTransient<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddHostedService<DiscordBot>()
                //.AddOptions<BotOptions>().Configure<IConfiguration>((settings, config) =>
                //{
                //    config.GetSection("Dataverse").Bind(settings);
                //})
                //.Configure<BotOptions>(Config.GetSection(BotOptions.Client))
                .AddMediatR(Assembly.GetAssembly(typeof(AddSongRequest)), Assembly.GetExecutingAssembly()) //todo: investigate alternatives for adding assembly
                .AddDbContext<SongOwnerContext>(opt => opt.UseInMemoryDatabase("myDataBase"))
                .AddTransient<IBasicSongRepository, BlockChainRepo>(); //todo: should this be registered here or on another class?

            return services;
        }
    }
}
