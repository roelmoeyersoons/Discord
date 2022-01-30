using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SongBlockChain.Modules.Commands
{
    public class Authenticated : ModuleBase<SocketCommandContext>
    {
        //commands in case you have a pubpriv key, right now there are no commands

        [Command("addsong")]
        public Task AddSong(string spotifyUrl)
        {
            var id = GrabSongId(Context, spotifyUrl);
            if(id != null)
            {
                //do logic to add the song
                return ReplyAsync($"Song added to the blockchain: {id}");
            }
            return ReplyAsync($"Construct a valid spotify url");
        }

        public static string GrabSongId(SocketCommandContext ctx, string spotifyUrl)
        {
            var regex = @"https:\/\/open\.spotify\.com\/track\/(.*)\s?";
            var match = Regex.Match(spotifyUrl, regex);

            if (match.Success)
            {
                Console.WriteLine("song detected, posting songId");
                var song = match.Groups.Values.Last().Value;
                return song;
            }
            return null;
        }
    }
}
