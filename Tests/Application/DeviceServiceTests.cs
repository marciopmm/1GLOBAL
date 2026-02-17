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

    #region GetDevice tests
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
    #endregion GetDevice tests

    #region AddDeviceAsync tests
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
    #endregion AddDeviceAsync tests

    #region UpdateDeviceAsync tests
    [TestMethod]
    public async Task UpdateDeviceAsync_WhenDeviceIsValid_ReturnsUpdatedDevice()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", State = State.Available, Brand = "Cisco" };
        var updatedDevice = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ReturnsAsync(updatedDevice);

        // Act
        var result = await _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Once);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenDeviceNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "", State = State.Available, Brand = "Cisco" };
        var updatedDevice = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ThrowsAsync(new ArgumentException("\"Name\" must be provided."));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenDeviceBrandIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", State = State.Available, Brand = "" };
        var updatedDevice = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ThrowsAsync(new ArgumentException("\"Brand\" must be provided."));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenDeviceStateIsNull_ThrowsInvalidStateException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", State = null, Brand = "Cisco" };
        var updatedDevice = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ThrowsAsync(new InvalidStateException(updatedDevice.Id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidStateException>(() => _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenStateInUseAndNameChanged_ThrowsInvalidStateForUpdateException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", State = State.Available, Brand = "Cisco" };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ThrowsAsync(new InvalidStateForUpdateException(updatedDevice.Id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidStateForUpdateException>(() => _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDeviceAsync_WhenStateInUseAndBrandChanged_ThrowsInvalidStateForUpdateException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "Router", State = State.Available, Brand = "New Brand" };
        var updatedDevice = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        _repositoryMock
            .Setup(r => r.GetByIdAsync(updatedDevice.Id))
            .ReturnsAsync(updatedDevice);
        _repositoryMock
            .Setup(r => r.UpdateAsync(updatedDevice.Id, devicePatch))
            .ThrowsAsync(new InvalidStateForUpdateException(updatedDevice.Id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidStateForUpdateException>(() => _sut.UpdateDeviceAsync(updatedDevice.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(updatedDevice.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(updatedDevice.Id, devicePatch), Times.Never);
    }
    #endregion UpdateDeviceAsync tests

    #region UpdateDevicePartialAsync tests
    // UpdateParcial tests can be added similarly, following the same pattern as UpdateDeviceAsync tests.
    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenDeviceIsValid_ReturnsUpdatedDevice()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", State = State.Available, Brand = "Cisco" };
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        var updatedDevice = device with { Name = devicePatch.Name!, Brand = devicePatch.Brand!, State = devicePatch.State!.Value };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ReturnsAsync(updatedDevice);

        // Act
        var result = await _sut.UpdateDevicePartialAsync(device.Id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Once);
    }

    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenDeviceNameIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "" };
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        var updatedDevice = device with { Name = devicePatch.Name! };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ThrowsAsync(new ArgumentException("\"Name\" must be provided."));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.UpdateDevicePartialAsync(device.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenDeviceBrandIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Brand = "" };
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        var updatedDevice = device with { Brand = devicePatch.Brand! };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ThrowsAsync(new ArgumentException("\"Brand\" must be provided."));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _sut.UpdateDevicePartialAsync(device.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenDeviceStateIsNull_ReturnsUpdatedDevice()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router", Brand = "New Cisco" };
        var device = new Device("Router", "Cisco", State.Available, DateTime.UtcNow);
        var updatedDevice = device with { Name = devicePatch.Name!, Brand = devicePatch.Brand! };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ReturnsAsync(updatedDevice);

        // Act
        var result = await _sut.UpdateDevicePartialAsync(device.Id, devicePatch);

        // Assert
        Assert.AreSame(updatedDevice, result);
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Once);
    }

    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenStateInUseAndNameChanged_ThrowsInvalidStateForUpdateException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Name = "New Router" };
        var device = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        var updatedDevice = device with { Name = devicePatch.Name! };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ThrowsAsync(new InvalidStateForUpdateException(device.Id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidStateForUpdateException>(() => _sut.UpdateDevicePartialAsync(device.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Never);
    }

    [TestMethod]
    public async Task UpdateDevicePartialAsync_WhenStateInUseAndBrandChanged_ThrowsInvalidStateForUpdateException()
    {
        // Arrange
        var devicePatch = new DevicePatch { Brand = "New Brand" };
        var device = new Device("Router", "Cisco", State.InUse, DateTime.UtcNow);
        var updatedDevice = device with { Brand = devicePatch.Brand! };
        _repositoryMock
            .Setup(r => r.GetByIdAsync(device.Id))
            .ReturnsAsync(device);
        _repositoryMock
            .Setup(r => r.UpdatePartialAsync(device.Id, devicePatch))
            .ThrowsAsync(new InvalidStateForUpdateException(device.Id));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<InvalidStateForUpdateException>(() => _sut.UpdateDevicePartialAsync(device.Id, devicePatch));
        _repositoryMock.Verify(r => r.GetByIdAsync(device.Id), Times.Once);
        _repositoryMock.Verify(r => r.UpdatePartialAsync(device.Id, devicePatch), Times.Never);
    }
    #endregion UpdateDevicePartialAsync tests
}
