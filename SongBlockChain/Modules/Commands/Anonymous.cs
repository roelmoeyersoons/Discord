using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Modules.Commands
{
    public class Anonymous : ModuleBase<SocketCommandContext>
    {
        [Command("register")]
        public Task RegisterUser(string pubKey)
        {
            var discordId = Context.User.Id;
            var discordName = Context.User.Username;

            //handle registering of code here
            //database.AddUserIfNotExists()
            
            return ReplyAsync($"Public key registered.");
            return ReplyAsync($"Your discord is already registered with public key {"oldkey"}");
        }

        [Command("getowner")]
        public Task GetOwner(string songId)
        {
            //do blockchain search
            return ReplyAsync("I own all the songs");
        }
    }
}
