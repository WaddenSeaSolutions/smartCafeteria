using backend.Interface;
using NUnit.Framework;
using Moq;
using backend.Model;
using backend.WebSockets.MessageHandlers;
using FluentAssertions;
using Fleck;
using System.Threading.Tasks;

[TestFixture]
public class LoginMessageHandlerTest
{
    private Mock<IUserService> _mockUserService;
    private Mock<ITokenService> _mockTokenService;
    private LoginMessageHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockUserService = new Mock<IUserService>();
        _mockTokenService = new Mock<ITokenService>();
        _handler = new LoginMessageHandler(_mockUserService.Object, _mockTokenService.Object);
    }

    [Test]
    public async Task HandleMessage_ShouldSendToken_WhenUserIsValid()
    {
        // Arrange
        var validUser = new User { Username = "jones", Password = "password", Role = "admin" };
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxMCIsInVuaXF1ZV9uYW1lIjoiam9uZXMiLCJyb2xlIjoiY3VzdG9tZXIiLCJuYmYiOjE3MTM4NTgwMjAsImV4cCI6MTgwODQ2NjAyMCwiaWF0IjoxNzEzODU4MDIwfQ.mhLLco9BxFnqnwJwfhVea0YDrVUzHivyhd4VQUocwRU";
        var loginDataJson = "{\"Username\":\"jones\",\"Password\":\"password\"}";

        _mockUserService.Setup(s => s.loginUser("jones", "password")).Returns(validUser);
        _mockTokenService.Setup(s => s.createToken(validUser)).Returns(token);

        // Mock the token service to return the expected user when validating the token
        _mockTokenService.Setup(s => s.validateTokenAndReturnUser(token)).Returns(validUser);

        var mockSocket = new Mock<IWebSocketConnection>();
        string sentToken = null;
        mockSocket.Setup(s => s.Send(It.IsAny<string>())).Callback<string>(message => sentToken = message);

        // Act
        await _handler.HandleMessage(loginDataJson, mockSocket.Object);

        // Assert
        sentToken.Should().Be(token); // This assertion checks if the token sent to the user matches the token you've set up in your mock.
    }
}