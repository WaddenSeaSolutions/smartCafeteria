using System.Text.Json;
using backend.Model;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class LoginMessageHandler : IMessageHandler
{
    private readonly UserService _userService;
    
    public LoginMessageHandler(UserService userService)
    {
        _userService = userService;
    }
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        LoginData loginData = JsonSerializer.Deserialize<LoginData>(message);
        
        User userToBeAuthenticated = _userService.loginUser(loginData.Username, loginData.Password);
        
        return Task.CompletedTask;
    }
    
    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}