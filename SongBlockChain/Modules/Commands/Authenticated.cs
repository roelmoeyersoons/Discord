using Discord.Commands;
using MediatR;
using Song.Application.Core;
using SongBlockChain.Application.Songs.Commands;
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
        private readonly IMediator _mediator;

        //commands in case you have a pubpriv key, right now there are no commands

        public Authenticated(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Command("addsong")]
        public async Task AddSong(string spotifyUrl)
        {
            var id = GrabSongId(Context, spotifyUrl);
            if(id != null)
            {
                var result = await _mediator.Send(new AddNewSongRequest
                {
                    DiscordId = Context.User.Id.ToString(),
                    SongId = id,
                });

                if (result.Success)
                {
                    await ReplyAsync($"Song added to the blockchain: {id}");
                }
                else
                {
                    await ReplyAsync(result.Error);
                }
            }
            else
            {
                ReplyAsync($"Construct a valid spotify url");
            }
            
        }

        public static string GrabSongId(SocketCommandContext ctx, string spotifyUrl)
        {
            var regex = @"https:\/\/open\.spotify\.com\/track\/(.*)\s?";
            var match = Regex.Match(spotifyUrl, regex);

            if (match.Success)
            {
                var song = match.Groups.Values.Last().Value;
                return song;
            }
            return null;
        }
    }
}
