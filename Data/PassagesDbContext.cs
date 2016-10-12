using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IronRod.Models;

namespace IronRod.Data
{
    public class PassagesDbContext : IdentityDbContext<ApplicationUser>
    {
        public PassagesDbContext(DbContextOptions<PassagesDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Passage> Passages { get; set; }
        public DbSet<PassageVerse> PassageVerses { get; set; }
        public DbSet<PassageTopic> PassageTopics { get; set; }
        public DbSet<Topic> Topics { get; set; }
    }
}
