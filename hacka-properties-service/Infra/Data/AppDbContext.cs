using hacka_properties_service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace hacka_properties_service.Infra.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties => Set<Property>();
        public DbSet<Field> Fields => Set<Field>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasMany(p => p.Fields)
                .WithOne()
                .HasForeignKey(f => f.PropertyId);

            base.OnModelCreating(modelBuilder);
        }
    }
}