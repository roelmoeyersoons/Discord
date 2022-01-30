using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Application.Repositories
{
    public interface IBasicSongRepository : IDisposable
    {
        public Task<bool> ValidateGenesisRecord(Genesis record, string signature);
        public IAsyncEnumerable<Genesis> GetGenesisRecords();
        public Task<bool> AddGenesisRecord(Genesis record);


        public Task<SpotifySong> GetSpotifySong(string spotifyId);
        public IAsyncEnumerable<SpotifySong> GetSongsByOwner(string ownerId);
        public Task AddSpotifySong(SpotifySong song);

    }
}
