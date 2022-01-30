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
    public class CreateGenesisBlockRequest : IRequest<Result>
    {
        public string GenesisId { get; set; }
        public string GenesisName { get; set; }
        public string Signature { get; set; }

    }

    public class CreateGenesisBlockRequestHandler : IRequestHandler<CreateGenesisBlockRequest, Result>
    {
        private readonly IBasicSongRepository _repo;
        public CreateGenesisBlockRequestHandler(IBasicSongRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException($"constructor parameter {nameof(repo)} is empty in {nameof(CreateGenesisBlockRequestHandler)}");
        }

        public async Task<Result> Handle(CreateGenesisBlockRequest request, CancellationToken cancellationToken)
        {
            var genesisRecord = new Genesis
            {
                Name = request.GenesisName,
                OwnerId = request.GenesisId,
            };

            bool success = await _repo.ValidateGenesisRecord(genesisRecord, request.Signature);

            if (success)
            {
                await _repo.AddGenesisRecord(new Genesis
                {
                    Name = request.GenesisName,
                    OwnerId = request.GenesisId,
                });
                return new Result(true, null);
            }
            return new Result(false, "The new genesis record validation failed, could not add the record");
        }
    }
}
