using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MM.Domain.Entities;
using MM.Infrastructure.Persistence.Abstractions;

namespace MM.Infrastructure.Persistence.Db
{
    public class MMDbContext : DbContext, IMMDbContext
    {
        private readonly string _connectionString;

        public virtual DbSet<Device> DeviceDbSet { get; set; } = null!;

        public MMDbContext(IConfiguration configuration)
        {
            var rawConnection = configuration.GetConnectionString("Default") ?? "Data Source=/app/SQLite/MMDb.sqlite";
            var sqliteBuilder = new SqliteConnectionStringBuilder(rawConnection);

            var absolutePath = Path.GetFullPath(sqliteBuilder.DataSource);
            var directory = Path.GetDirectoryName(absolutePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            sqliteBuilder.DataSource = absolutePath;
            _connectionString = sqliteBuilder.ToString();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // You can add custom logic here before saving changes, such as auditing or validation
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Device entity
            modelBuilder.Entity<Device>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Brand).IsRequired();
                entity.Property(e => e.State).IsRequired();
                entity.Property(e => e.CreationTime).IsRequired();
            });
        }
    }
}