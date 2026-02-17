using Moq;
using MM.Application.Services.Devices;
using MM.Domain.Entities;
using MM.Domain.Enums;
using MM.Domain.Exceptions;
using MM.Domain.Ports;

namespace MM.Tests.Application;

[TestClass]
public class DeviceServiceTests
{
    private Mock<IDeviceRepository> _repositoryMock = null!;
    private DeviceService _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _sut = new DeviceService(_repositoryMock.Object);
    }

    [TestMethod]
    public async Task GetDeviceByIdAsync_WhenCorrectId_ReturnsDevice()
    {
        // Arrange
        var id = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var device = new Device("Router", "Cisco", State.Available, now);
        _repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(device);

        // Act
        var result = await _sut.GetDeviceByIdAsync(id);

        // Assert
        Assert.AreSame(device, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [TestMethod]
    public async Task GetDeviceByIdAsync_WhenWrongId_ThrowsDeviceNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ThrowsAsync(new DeviceNotFoundException(id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<DeviceNotFoundException>(() => _sut.GetDeviceByIdAsync(id));
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [TestMethod]
    public async Task GetAllDevicesAsync_ReturnsDevicesList()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device("Router", "Cisco", State.Available, DateTime.UtcNow),
            new Device("Switch", "Juniper", State.InUse, DateTime.UtcNow)
        };
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(devices);

        // Act
        var result = await _sut.GetAllDevicesAsync();

        // Assert
        Assert.AreEqual(devices, result);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetDevicesByStateAsync_WhenStateIsAvailable_ReturnsDevicesList()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device("Router", "Cisco", State.Available, DateTime.UtcNow),
            new Device("Switch", "Juniper", State.InUse, DateTime.UtcNow)
        };
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(devices);

        // Act
        var result = await _sut.GetDevicesByStateAsync(State.Available);

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Cisco", result.First().Brand);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetDevicesByStateAsync_WhenStateIsInUse_ReturnsEmptyList()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device("Router", "Cisco", State.Available, DateTime.UtcNow),
            new Device("Switch", "Juniper", State.Available, DateTime.UtcNow)
        };
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(devices);

        // Act
        var result = await _sut.GetDevicesByStateAsync(State.InUse);

        // Assert
        Assert.AreEqual(0, result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetDevicesByBrandAsync_WhenBrandIsCisco_ReturnsDevicesList()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device("Router", "Cisco", State.Available, DateTime.UtcNow),
            new Device("Switch", "Juniper", State.Available, DateTime.UtcNow)
        };
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(devices);

        // Act
        var result = await _sut.GetDevicesByBrandAsync("Cisco");

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Cisco", result.First().Brand);
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetDevicesByBrandAsync_WhenBrandIsUnknown_ReturnsEmptyList()
    {
        // Arrange
        var devices = new List<Device>
        {
            new Device("Router", "Cisco", State.Available, DateTime.UtcNow),
            new Device("Switch", "Juniper", State.Available, DateTime.UtcNow)
        };
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(devices);

        // Act
        var result = await _sut.GetDevicesByBrandAsync("Unknown");

        // Assert
        Assert.AreEqual(0, result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [TestMethod]
    public async Task AddDeviceAsync_WhenDeviceIsValid_ReturnsAddedDevice()
    {
        // Arrange
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.AddAsync(device))
            .ReturnsAsync(device);

        // Act
        var result = await _sut.AddDeviceAsync(device);

        // Assert
        Assert.AreSame(device, result);
        _repositoryMock.Verify(r => r.AddAsync(device), Times.Once);
    }

    [TestMethod]
    public async Task AddDeviceAsync_WhenDeviceIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        Device? device = null;

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => _sut.AddDeviceAsync(device!));
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Device>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenDeviceIsValid_ReturnsUpdatedDevice()
    {
        // Arrange
        var id = Guid.NewGuid();
        var devicePatch = new DevicePatch { State = State.InUse };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Device("Router", "Cisco", State.Available, DateTime.UtcNow));
        _repositoryMock
            .Setup(r => r.UpdateAsync(id, devicePatch))
            .ReturnsAsync(updatedDevice);

        // Act
        var result = await _sut.UpdateDeviceAsync(id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(id, devicePatch), Times.Once);
    }
}
