using Discord.Commands;
using Song.Model.Data;
using SongBlockChain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Modules.Commands
{
    public class Anonymous : ModuleBase<SocketCommandContext>
    {
        private readonly SongOwnerContext _context;

        //TODO: SWITCH TO MEDIATR
        public Anonymous(SongOwnerContext context)
        {
            this._context = context;
        }

        [Command("register")]
        public async Task RegisterUser(string pubKey)
        {
            var discordId = Context.User.Id;
            var discordName = Context.User.Username;

            //should be a mediatr request



            var newOwner = new Owner
            {
                OwnerId = discordId.ToString(),
                OwnerKey = pubKey,
                OwnerName = discordName,
            };

            await _context.Users.AddAsync(newOwner);
            await _context.SaveChangesAsync();
            
            await ReplyAsync($"Public key registered.");
            //await ReplyAsync($"Your discord is already registered with public key {"oldkey"}");
        }

        [Command("getowner")]
        public Task GetOwner(string songId)
        {
            //do blockchain search
            return ReplyAsync("I own all the songs");
        }
    }
}
