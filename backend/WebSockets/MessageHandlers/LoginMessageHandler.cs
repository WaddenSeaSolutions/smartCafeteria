using System.Text.Json;
using backend.Model;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class LoginMessageHandler : IMessageHandler
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;
    
    public LoginMessageHandler(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }
    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        LoginData loginData = JsonSerializer.Deserialize<LoginData>(message);
        
        User userToBeAuthenticated = _userService.loginUser(loginData.Username, loginData.Password);

        string tokenForUser = _tokenService.createToken(userToBeAuthenticated);

        Console.WriteLine(loginData.Username + loginData.Password + "great success");
        await socket.Send(tokenForUser);
    }
    
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}