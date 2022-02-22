using Microsoft.EntityFrameworkCore;
using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Persistence
{
    //ideally there should be a separate IRepository interface and then an implemenation -> ef core
    //however ef core in itself is an abstraction already, for now they are the same layer

    public class SongOwnerContext : DbContext
    {
        public DbSet<Owner> Users { get; set; }

        public SongOwnerContext(DbContextOptions options) : base(options) { }
    }
}
