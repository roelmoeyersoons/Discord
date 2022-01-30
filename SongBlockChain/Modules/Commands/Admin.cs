using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Modules.Commands
{
    //[RequireOwner]
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [RequireOwner]
        [Command("rebuild")]
        public async Task RebuildBlockChain() //int number ~ argpos 
        {
            
            await ReplyAsync($"Rebuilding... {Context.User?.Id}");
        }
    }
}
