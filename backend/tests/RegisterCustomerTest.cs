using backend.Interface;
using backend.Service;
using backend.WebSockets.MessageHandlers;
using FluentAssertions;
using NUnit.Framework;
using Moq;


[TestFixture]
public class RegisterCustomerTests
{
    private Mock<IRegisterCustomerDAL> _mockRegisterCustomerDal;
    private RegisterCustomerService _registerCustomerService;

    [SetUp]
    public void Setup()
    {
        _mockRegisterCustomerDal = new Mock<IRegisterCustomerDAL>();
        _registerCustomerService = new RegisterCustomerService(_mockRegisterCustomerDal.Object);
    }

    [Test]
    public void RegisterCustomer_CreatesCustomer_ReturnsTrue()
    {
        // Arrange
        var registerCustomerData = new RegisterCustomerHandler.RegisterCustomerData
        {
            Username = "testUser",
            Password = "testPassword"
        };

        _mockRegisterCustomerDal
            .Setup(dal => dal.RegisterCustomer(It.IsAny<RegisterCustomerHandler.RegisterCustomerData>())).Returns(true);

        // Act
        var result = _registerCustomerService.RegisterCustomer(registerCustomerData);

        // Assert
        result.Should().BeTrue();
    }
}