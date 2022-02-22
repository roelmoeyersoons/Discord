using Discord;
using Discord.Commands;
using SongBlockChain.Persistence;
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
        private readonly SongOwnerContext _context;

        public Admin(SongOwnerContext context)
        {
            _context = context;
        }

        [RequireOwner]
        [Command("rebuild")]
        public async Task RebuildBlockChain() //int number ~ argpos 
        {
            
            await ReplyAsync($"Rebuilding... {Context.User?.Id}");
        }


        [RequireOwner]
        [Command("getallusers")]
        public async Task GetAllUsers() //int number ~ argpos 
        {
            var sb = new StringBuilder();

            sb.AppendLine("Users:");
            foreach(var user in _context.Users)
            {
                sb.AppendLine(user.OwnerKey);
            }

            await ReplyAsync($"{sb}");
        }
    }
}
