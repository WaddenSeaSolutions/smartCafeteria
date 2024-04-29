using NUnit.Framework;
using Moq;
using backend.DAL;
using backend.Interface;
using backend.Service;
using backend.Model;
using FluentAssertions;

[TestFixture]
public class LoginTest
{
    [Test]
    public void LoginUser_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var mockUserDAL = new Mock<IUserDAL>();
        var testUser = new User { Username = "testUser", Password = BCrypt.Net.BCrypt.HashPassword("testPassword", 12) };
        mockUserDAL.Setup(dal => dal.userFromUsername("testUser")).Returns(testUser);
        var userService = new UserService(mockUserDAL.Object);

        // Act
        var result = userService.loginUser("testUser", "testPassword");

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("testUser");
    }
}