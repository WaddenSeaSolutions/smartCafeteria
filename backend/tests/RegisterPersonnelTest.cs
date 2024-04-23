using NUnit.Framework;
using Moq;
using FluentAssertions;
using backend.Interface;
using backend.Service;
using backend.WebSockets.MessageHandlers;

namespace backend.Tests
{
    public class RegisterPersonnelTests
    {
        private Mock<IUserDAL> _mockUserDal;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserDal = new Mock<IUserDAL>();
            _userService = new UserService(_mockUserDal.Object);
        }

        [Test]
        public void RegisterPersonnel_CreatesPersonnel_ReturnsTrue()
        {
            // Arrange
            var registerPersonnelData = new RegisterPersonnelHandler.RegisterPersonnelData
            {
                Username = "testUser",
                Password = "testPassword"
            };

            _mockUserDal.Setup(dal => dal.registerPersonnel(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Act
            _userService.registerPersonnel(registerPersonnelData.Username, registerPersonnelData.Password, "Personnel");

            // Assert
            _mockUserDal.Verify(dal => dal.registerPersonnel(registerPersonnelData.Username, It.IsAny<string>(), "Personnel"), Times.Once);
        }
    }
}   