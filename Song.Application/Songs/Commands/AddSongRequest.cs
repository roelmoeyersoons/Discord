using MediatR;
using Song.Application.Core;
using Song.Application.Repositories;
using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Song.Application.Songs.Commands
{
    public class AddSongRequest : IRequest<Result>
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string SpotifyId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
    }

    public class AddSongHandler : IRequestHandler<AddSongRequest, Result>
    {

        private readonly IBasicSongRepository _repo;
        public AddSongHandler(IBasicSongRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException($"constructor parameter {nameof(repo)} is empty in {nameof(AddSongHandler)}");
        }

        public async Task<Result> Handle(AddSongRequest request, CancellationToken cancellationToken)
        {
            var retrievedSong = await _repo.GetSpotifySong(request.SpotifyId);
            if(retrievedSong != null)
            {
                return new Result(false, $"The song already exists");
            }

            var owner = new Owner
            {
                OwnerId = request.UserId,
                Name = request.Name
            };

            var spotifySong = new SpotifySong
            {
                SpotifyId = request.SpotifyId,
                Name = request.Name,
                Artist = request.Artist,
                Owner = owner
            };

            await _repo.AddSpotifySong(spotifySong);

            return new Result(true, null);
        }
    }
}
