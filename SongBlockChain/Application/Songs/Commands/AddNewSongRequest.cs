using MediatR;
using Song.Application.Core;
using Song.Application.Songs.Commands;
using SongBlockChain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SongBlockChain.Application.Songs.Commands
{
    public class AddNewSongRequest : IRequest<Result>
    {
        public string DiscordId;
        public string SongId;
    }

    public class AddNewSongRequestHandler : IRequestHandler<AddNewSongRequest, Result>
    {
        private readonly SongOwnerContext _context;
        private readonly IMediator _mediator;

        public AddNewSongRequestHandler(SongOwnerContext context, IMediator mediator)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<Result> Handle(AddNewSongRequest request, CancellationToken cancellationToken)
        {
            //do validation

            var foundUser = await _context.Users.FindAsync(request.DiscordId);

            if (foundUser == null)
                return new Result(false, "The user was not registered!");

            var fullSong = ""; //retrieve full song stuff from spotify

            var req = new AddSongRequest
            {
                UserId = foundUser.OwnerKey,
                UserName = foundUser.OwnerName,
                SpotifyId = request.SongId,
                Artist = "",
                Name = ""
            };

            var result = await _mediator.Send(req);
            return result;
        }
    }
}
