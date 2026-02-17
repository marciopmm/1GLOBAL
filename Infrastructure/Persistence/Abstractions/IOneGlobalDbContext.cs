using MM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace MM.Infrastructure.Persistence.Abstractions
{
    public interface IMMDbContext
    {
        DbSet<Device> DeviceDbSet { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}