using Moq;
using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Exceptions;
using MM.Infrastructure.Persistence.Abstractions;
using MM.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MM.Tests.Infrastructure;

[TestClass]
public class DeviceRepositoryTests
{
    private Mock<IMMDbContext> _dbContextMock = null!;
    private DeviceRepository _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _dbContextMock = new Mock<IMMDbContext>();
        _sut = new DeviceRepository(_dbContextMock.Object);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenCorrectId_ReturnsDevice()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var device = new Device("Router", "Cisco", State.Available, now);
        var id = device.Id;
        _dbContextMock.Setup(r => r.DeviceDbSet.FindAsync(id)).ReturnsAsync(device);

        // Act
        var result = await _sut.GetByIdAsync(id);

        // Assert
        Assert.AreSame(device, result);
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenWrongId_ThrowsDeviceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(id))
            .ThrowsAsync(new DeviceNotFoundException(id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<DeviceNotFoundException>(() => _sut.GetByIdAsync(id));
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
    }

    [TestMethod]
    public async Task AddAsync_WhenDeviceIsValid_ReturnsAddedDevice()
    {
        // Arrange
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.AddAsync(device, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EntityEntry<Device>(null!));

        // Act
        var result = await _sut.AddAsync(device);

        // Assert
        Assert.AreSame(device, result);
        _dbContextMock.Verify(r => r.DeviceDbSet.AddAsync(device, It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task AddAsync_WhenDeviceHasInvalidName_ThrowsArgumentNullException()
    {
        // Arrange
        var device = new Device(null!, "Cisco", State.Available, DateTime.UtcNow);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.AddAsync(device));
        _dbContextMock.Verify(r => r.DeviceDbSet.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task AddAsync_WhenDeviceHasInvalidBrand_ThrowsArgumentNullException()
    {
        // Arrange
        var device = new Device("Router", null!, State.Available, DateTime.UtcNow);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.AddAsync(device));
        _dbContextMock.Verify(r => r.DeviceDbSet.AddAsync(It.IsAny<Device>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenDeviceIsValid_ReturnsUpdatedDevice()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "Router", Brand = "Cisco", State = State.InUse };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.Update(updatedDevice))
            .Verifiable();
        _dbContextMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        var result = await _sut.UpdateAsync(updatedDevice.Id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(updatedDevice.Id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(It.IsAny<Device>()), Times.Once);
        _dbContextMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenDeviceIsNotFound_ThrowsDeviceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var devicePatch = new DevicePatch { Name = "Switch", Brand = "Cisco", State = State.InUse };
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(id))
            .ReturnsAsync((Device)null!);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<DeviceNotFoundException>(() => _sut.UpdateAsync(id, devicePatch));

        // Assert
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenDeviceHasInvalidName_ThrowsArgumentNullException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = null!, Brand = "Cisco", State = State.Available };
        var device = new Device(null!, "Cisco", State.InUse, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(device.Id))
            .ReturnsAsync(new Device("Router", "Cisco", State.Available, DateTime.UtcNow));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.UpdateAsync(device.Id, devicePatch));
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_WhenDeviceHasInvalidBrand_ThrowsArgumentNullException()
    {
        // Arrange
        var device = new Device("Router", null!, State.InUse, DateTime.UtcNow);
        var devicePatch = new DevicePatch { Name = "Router", Brand = null!, State = State.Available };
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(device.Id))
            .ReturnsAsync(device);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.UpdateAsync(device.Id, devicePatch));
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdatePartialAsync_WhenDeviceIsValid_ReturnsUpdatedDevice()
    {
        // Arrange
        var devicePatch = new DevicePatch { State = State.InUse };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.Update(updatedDevice))
            .Verifiable();
        _dbContextMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Verifiable();

        // Act
        var result = await _sut.UpdatePartialAsync(updatedDevice.Id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(updatedDevice.Id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(updatedDevice), Times.Once);
        _dbContextMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdatePartialAsync_WhenDeviceIsNotFound_ThrowsDeviceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var devicePatch = new DevicePatch { State = State.InUse };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(id))
            .ReturnsAsync((Device)null!);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<DeviceNotFoundException>(() => _sut.UpdatePartialAsync(id, devicePatch));

        // Assert
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Update(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenIdExists_ReturnsOK()
    {
        // Arrange
        var id = Guid.NewGuid();
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(id))
            .ReturnsAsync(device);
        _dbContextMock
            .Setup(r => r.DeviceDbSet.Remove(device))
            .Verifiable();

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Remove(device), Times.Once);
    }

    [TestMethod]
    public async Task DeleteAsync_WhenIdDoesNotExist_ThrowsDeviceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _dbContextMock
            .Setup(r => r.DeviceDbSet.FindAsync(id))
            .ReturnsAsync((Device)null!);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<DeviceNotFoundException>(() => _sut.DeleteAsync(id));

        // Assert
        _dbContextMock.Verify(r => r.DeviceDbSet.FindAsync(id), Times.Once);
        _dbContextMock.Verify(r => r.DeviceDbSet.Remove(It.IsAny<Device>()), Times.Never);
    }
}