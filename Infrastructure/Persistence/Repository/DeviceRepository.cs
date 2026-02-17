using MM.Domain.Entities;
using MM.Domain.Exceptions;
using MM.Domain.Ports;
using MM.Infrastructure.Persistence.Db;
using Microsoft.EntityFrameworkCore;
using MM.Infrastructure.Persistence.Abstractions;

namespace MM.Infrastructure.Persistence.Repository;

public class DeviceRepository : IDeviceRepository
{
    private readonly IMMDbContext _context;

    public DeviceRepository(IMMDbContext context)
    {
        _context = context;
    }

    public async Task<Device> GetByIdAsync(Guid id)
    {
        return await _context.DeviceDbSet.FindAsync(id) ?? throw new DeviceNotFoundException(id);
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _context.DeviceDbSet.ToListAsync();
    }

    public async Task<Device> AddAsync(Device device)
    {
        if (string.IsNullOrWhiteSpace(device.Name)) throw new ArgumentNullException("Name");
        if (string.IsNullOrWhiteSpace(device.Brand)) throw new ArgumentNullException("Brand");

        await _context.DeviceDbSet.AddAsync(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device> UpdateAsync(Guid id, DevicePatch devicePatch)
    {
        var device = await _context.DeviceDbSet.FindAsync(id) ?? throw new DeviceNotFoundException(id);
        device.Name = devicePatch.Name ?? throw new ArgumentNullException("Name");
        device.Brand = devicePatch.Brand ?? throw new ArgumentNullException("Brand");
        device.State = devicePatch.State ?? throw new ArgumentNullException("State");

        _context.DeviceDbSet.Update(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task<Device> UpdatePartialAsync(Guid id, DevicePatch devicePatch)
    {
        var device = await _context.DeviceDbSet.FindAsync(id) ?? throw new DeviceNotFoundException(id);
        if (devicePatch.Name != null)
            device.Name = devicePatch.Name;
        if (devicePatch.Brand != null)
            device.Brand = devicePatch.Brand;
        if (devicePatch.State.HasValue)
            device.State = devicePatch.State.Value;

        _context.DeviceDbSet.Update(device);
        await _context.SaveChangesAsync();
        return device;
    }

    public async Task DeleteAsync(Guid id)
    {
        var device = await GetByIdAsync(id);
        _context.DeviceDbSet.Remove(device);
        await _context.SaveChangesAsync();
    }
}