using Song.Application.Repositories;
using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Persistence.Blockchain
{
    public class BlockChainRepo : IBasicSongRepository
    {
        public void Dispose()
        { }

        public async Task<bool> AddGenesisRecord(Genesis record)
        {
            return true;
        }

        public Task AddSpotifySong(SpotifySong song)
        {
            return Task.CompletedTask;
        } 

        public IAsyncEnumerable<Genesis> GetGenesisRecords()
        {
            return AsyncEnumerable.Empty<Genesis>();
        }

        public IAsyncEnumerable<SpotifySong> GetSongsByOwner(string ownerId)
        {
            return AsyncEnumerable.Empty<SpotifySong>();
        }

        public async Task<SpotifySong> GetSpotifySong(string spotifyId)
        {
            return null;
        }

        public async Task<bool> ValidateGenesisRecord(Genesis record, string signature)
        {
            return true;
        }
    }
}
