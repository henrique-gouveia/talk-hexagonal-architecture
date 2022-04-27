using Microsoft.EntityFrameworkCore;
using Store4Dev.Domain.Entities;
using Store4Dev.Domain.Support;

namespace Store4Dev.Data
{
    public class StoreContext : DbContext, IUnitOfWork
    {
        public StoreContext(DbContextOptions options)
            : base(options) { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=store4dev.db;Cache=Shared");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var properties = modelBuilder.Model.GetEntityTypes()
                    .SelectMany(e => e
                        .GetProperties()
                        .Where(p => new[] { typeof(string), typeof(decimal) }.Contains(p.ClrType))
                    );

            foreach (var property in properties)
            {
                if (property.ClrType == typeof(string))
                    property.SetColumnType("varchar(100)");

                if (property.ClrType == typeof(decimal))
                    property.SetColumnType("numeric(19,2)");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> CompleteAsync()
        {
            var entries = ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTimeOffset.UtcNow;
                    entry.Property("UpdatedAt").IsModified = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("CreatedAt").IsModified = false;
                    entry.Property("UpdatedAt").CurrentValue = DateTimeOffset.UtcNow;
                }
            }

            return await SaveChangesAsync() > 0;
        }

        public DbSet<Brand> Brands { get; private set; }
        public DbSet<Product> Products { get; private set; }
    }
}
