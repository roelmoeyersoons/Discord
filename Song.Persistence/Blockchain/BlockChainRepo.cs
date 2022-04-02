using Song.Application.Repositories;
using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /*
        - wat is Block -> stelt een block in de blockchain voor
       - blockchain is een inmemory object, mag voorgesteld worden door dezelfde class
        maar waarop vloek je nu: je moet de hash berekenen van zo'n object
        dit kan je doen door tostrings te overschrijven en dat om te zetten naar bytes -> sha -> dan heb je

        hoe werk je met strings en bytes, waarom heb je zowel een string als byterepresentatie en welke moet je gebruiken voor hashing

        wat zijn de taken van de blockchain:
    - het bijhouden van data en structuur brengen, valideren chain
    - persistentie is belangrijkste taak
    - je moet het serialiseren, een init en finalise methode hebben ofzo, opslagmethodes: sql ook, een tekstbestand waarin je laatste geserialiseerde versie hebt staan -> je moet volledig kunnen parsen
    - bekijk mss northwindtrader project
    - jsonserializer gebruiken voor op bestand te zetten, later kan er entityframework ofzo van gemaakt worden, anders herladen 
    - <-> echte blockchaintechnologie moet enkel afweten van hashes voor aan elkaar te linken maar manier waarop je die berekent moet niet generatief zijn
     
     */

    public class Block {


        private byte[] _previousBlockHash;
        public byte[] PreviousBlockHash { 
            get => _previousBlockHash; 
            set {
                _previousBlockHash = value;
                PreviousBlockHashBase64 = System.Convert.ToBase64String(value);
            } 
        }
        public string PreviousBlockHashBase64 { get; private set;  }

        private byte[] _blockHash;
        public byte[] BlockHash
        {
            get => _blockHash;
            set
            {
                _blockHash = value;
                BlockHashBase64 = System.Convert.ToBase64String(value);
            }
        }
        public string BlockHashBase64 { get; private set; }




        public uint ProofOfWorkNonce { get; private set; }

        public IEnumerable<BlockChainItem> Items { get; set; }

        public void ValidateBlock()
        {
            var random = new Random();
            var veryHardToCalculateNonce = ((uint)random.Next());
            ProofOfWorkNonce = veryHardToCalculateNonce;

            using(var sha = SHA256.Create())
            {
                var sb = new StringBuilder();


                //var prevBytes = Encoding.UTF8.GetBytes(PreviousBlockHash);
                //var nonceBytes = Encoding.UTF8.GetBytes(ProofOfWorkNonce);

                var 

                sha.ComputeHash() //with inputstream
                //sha.ComputeHash()
            }
        }

    }

    public class BlockChainItem
    {
        public SpotifySong Song { get; set; }

        private byte[] _signature;
        public byte[] Signature 
        { 
            get => _signature; 
            set 
            { 
                _signature = value;
                SignatureBase64 = System.Convert.ToBase64String(value);
            } 
        }
        public string SignatureBase64 { get; private set; }

        //public byte[] CalculateHash(SHA256 crypto)
        //{
        //    return crypto.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ToString()));
        //}

        public override string ToString()
        {
            return String.Format("{0}, Signature: {1}", Song, SignatureBase64);
        }

    }

    public class BlockChain
    {
        public List<BlockChainItem> Actual { get; set; }
    }
}
