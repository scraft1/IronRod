using Microsoft.EntityFrameworkCore;
using IronRod.Models;

namespace IronRod.Data
{
    public class ScriptureMasteryDbContext : DbContext
    {
        public ScriptureMasteryDbContext(DbContextOptions<ScriptureMasteryDbContext> options)
            : base(options)
        {
        }
        public DbSet<SMVolume> Volumes { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<SMVerse> Verses { get; set; }
    }
}