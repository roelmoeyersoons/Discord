using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.Model.Data
{
    public class Owner
    {

        [Key]
        public string OwnerId { get; set; }

        public string OwnerName { get; set; }
        
        public string OwnerKey { get; set; }

    }
}
