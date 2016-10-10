using Microsoft.EntityFrameworkCore;
using IronRod.Models;

namespace IronRod.Data
{
    public class ScripturesDbContext : DbContext
    {
        public ScripturesDbContext(DbContextOptions<ScripturesDbContext> options)
            : base(options)
        {
        }
        public DbSet<Volume> Volumes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Verse> Verses { get; set; }
    }
}