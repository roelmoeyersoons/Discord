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

namespace Song.Application.Songs.Queries
{
    public class CheckOwnerRequest : IRequest<DataResult<Owner>>
    {
        public string SpotifyId { get; set; }
    }

    public class CheckOwnerHandler : IRequestHandler<CheckOwnerRequest, DataResult<Owner>>
    {
        private readonly IBasicSongRepository _repo;

        public CheckOwnerHandler(IBasicSongRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException($"constructor parameter {nameof(repo)} is empty in {nameof(CheckOwnerHandler)}");
        }

        public async Task<DataResult<Owner>> Handle(CheckOwnerRequest request, CancellationToken cancellationToken)
        {
            var requestedSong = await _repo.GetSpotifySong(request.SpotifyId);

            return requestedSong switch
            {
                null => new DataResult<Owner>(false, null, "Song not found"),
                _ => new DataResult<Owner>(true, requestedSong.Owner, null),
            };
        }
    }
}
