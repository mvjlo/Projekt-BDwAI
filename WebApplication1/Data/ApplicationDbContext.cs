using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<License> Licenses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Plugin>()
                .HasOne(p => p.Manufacturer)
                .WithMany(m => m.Plugins)
                .HasForeignKey(p => p.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Plugin>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Plugins)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<License>()
                .HasOne(l => l.Plugin)
                .WithMany(p => p.Licenses)
                .HasForeignKey(l => l.PluginId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Manufacturer>().HasData(
                new Manufacturer { Id = 1, Name = "FabFilter", Website = "https://www.fabfilter.com", SupportEmail = "support@fabfilter.com" },
                new Manufacturer { Id = 2, Name = "Xfer Records", Website = "https://xferrecords.com", SupportEmail = "support@xferrecords.com" },
                new Manufacturer { Id = 3, Name = "Native Instruments", Website = "https://www.native-instruments.com", SupportEmail = "support@native-instruments.com" }
            );

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Syntezator", Description = "Wtyczki syntetyzujące dźwięk" },
                new Category { Id = 2, Name = "Kompresor", Description = "Wtyczki do kompresji dynamiki" },
                new Category { Id = 3, Name = "Reverb", Description = "Wtyczki pogłosowe" },
                new Category { Id = 4, Name = "EQ", Description = "Korektory graficzne i parametryczne" }
            );
        }
    }
}
