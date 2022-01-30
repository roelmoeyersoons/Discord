using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Model.Data
{
    public class SpotifySong
    {
        public string SpotifyId { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public Owner Owner { get; set; } 
    }
}
