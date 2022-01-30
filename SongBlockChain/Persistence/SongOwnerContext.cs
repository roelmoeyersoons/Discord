using Microsoft.EntityFrameworkCore;
using Song.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongBlockChain.Persistence
{
    public class SongOwnerContext : DbContext
    {
        public DbSet<Owner> Users { get; set; }

        public SongOwnerContext(DbContextOptions options) : base(options) { }
    }
}
