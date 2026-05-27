using AdminService.Models;
using AdminService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminService.Tests.Controllers;

[TestClass]
public class AdminControllerTests
{
    private Mock<IAdminService> _serviceMock = null!;
    private Mock<ILogger<AdminController>> _loggerMock = null!;
    private AdminController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _serviceMock = new Mock<IAdminService>();
        _loggerMock = new Mock<ILogger<AdminController>>();

        _controller = new AdminController(
            _serviceMock.Object,
            _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAdmin_ReturnsOk_WhenAdminExists()
    {
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex"
        };

        _serviceMock
            .Setup(x => x.GetByIdAsync("123"))
            .ReturnsAsync(admin);

        var result = await _controller.GetAdmin("123");

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task GetAdmin_ReturnsNotFound_WhenAdminDoesNotExist()
    {
        _serviceMock
            .Setup(x => x.GetByIdAsync("123"))
            .ReturnsAsync((Admin?)null);

        var result = await _controller.GetAdmin("123");

        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    [TestMethod]
    public async Task CreateAdmin_CallsServiceCreate()
    {
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex",
            LastName = "Skov",
            CenterId = "center1",
            Role = AdminRole.Centerleder
        };

        _serviceMock
            .Setup(x => x.CreateAsync(It.IsAny<Admin>()))
            .ReturnsAsync(admin);

        await _controller.CreateAdmin(admin);

        _serviceMock.Verify(
            x => x.CreateAsync(It.IsAny<Admin>()),
            Times.Once);
    }

    [TestMethod]
    public async Task CreateAdmin_ReturnsCreated()
    {
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex",
            LastName = "Skov",
            CenterId = "center1",
            Role = AdminRole.Centerleder
        };

        _serviceMock
            .Setup(x => x.CreateAsync(It.IsAny<Admin>()))
            .ReturnsAsync(admin);

        var result = await _controller.CreateAdmin(admin);

        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }
}