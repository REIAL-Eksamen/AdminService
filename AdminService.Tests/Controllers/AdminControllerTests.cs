using AdminService.Clients;
using AdminService.Models;
using AdminService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminService.Tests.Controllers;

/// <summary>
/// Unit tests til AdminController.
/// 
/// Testene verificerer:
/// - Korrekte HTTP responses
/// - Kald til repository
/// - Kald til eksterne services
/// </summary>
[TestClass]
public class AdminControllerTests
{
    // Mockede dependencies som controlleren bruger
    private Mock<IAdminRepository> _repoMock = null!;
    private Mock<IClassServiceClient> _clientMock = null!;
    private Mock<ILogger<AdminController>> _loggerMock = null!;

    // Selve controlleren vi tester
    private AdminController _controller = null!;

    /// <summary>
    /// Kører før hver test.
    /// Opretter nye mocks og en frisk controller.
    /// </summary>
    [TestInitialize]
    public void Setup()
    {
        _repoMock = new Mock<IAdminRepository>();
        _clientMock = new Mock<IClassServiceClient>();
        _loggerMock = new Mock<ILogger<AdminController>>();

        _controller = new AdminController(
            _repoMock.Object,
            _clientMock.Object,
            _loggerMock.Object);
    }

    /// <summary>
    /// Tester at GetAdmin returnerer HTTP 200 OK
    /// når admin findes i databasen.
    /// </summary>
    [TestMethod]
    public async Task GetAdmin_ReturnsOk_WhenAdminExists()
    {
        // Arrange
        // Opretter fake admin-data
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex"
        };

        // Konfigurerer repository mock
        // til at returnere adminen
        _repoMock
            .Setup(x => x.GetById("123"))
            .ReturnsAsync(admin);

        // Act
        // Kalder controller-metoden
        var result = await _controller.GetAdmin("123");

        // Assert
        // Verificerer at resultatet er HTTP 200 OK
        var okResult = result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    /// <summary>
    /// Tester at GetAdmin returnerer HTTP 404 NotFound
    /// når admin ikke findes.
    /// </summary>
    [TestMethod]
    public async Task GetAdmin_ReturnsNotFound_WhenAdminDoesNotExist()
    {
        // Arrange
        // Repository returnerer null
        _repoMock
            .Setup(x => x.GetById("123"))
            .ReturnsAsync((Admin?)null);

        // Act
        var result = await _controller.GetAdmin("123");

        // Assert
        // Verificerer at resultatet er NotFound
        Assert.IsInstanceOfType(result, typeof(NotFoundResult));
    }

    /// <summary>
    /// Tester at CreateAdmin kalder repository Create-metoden.
    /// </summary>
    [TestMethod]
    public async Task CreateAdmin_CallsRepositoryCreate()
    {
        // Arrange
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex",
            LastName = "Skov",
            CenterId = "center1",
            Role = AdminRole.Centerleder
        };

        // Act
        await _controller.CreateAdmin(admin);

        // Assert
        // Verificerer at Create bliver kaldt præcis én gang
        _repoMock.Verify(
            x => x.Create(It.IsAny<Admin>()),
            Times.Once);
    }

    /// <summary>
    /// Tester at CreateAdmin kalder ClassServiceClient
    /// for at tilføje admin til center-servicen.
    /// </summary>
    [TestMethod]
    public async Task CreateAdmin_CallsClassServiceClient()
    {
        // Arrange
        var admin = new Admin
        {
            Id = "123",
            FirstName = "Alex",
            LastName = "Skov",
            CenterId = "center1",
            Role = AdminRole.Instruktør
        };

        // Act
        await _controller.CreateAdmin(admin);

        // Assert
        // Verificerer at den eksterne service bliver kaldt
        // med de korrekte parametre
        _clientMock.Verify(x =>
            x.AddAdminToCenter(
                admin.CenterId,
                admin.FirstName,
                admin.LastName,
                admin.Id,
                admin.Role),
            Times.Once);
    }
}