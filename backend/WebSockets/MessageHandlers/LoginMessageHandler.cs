using backend.Model;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class LoginMessageHandler : IMessageHandler
{
    private readonly UserService _userService;
    
    public LoginMessageHandler(UserService userService)
    {
        _userService = new UserService();
    }
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        User userToBeAuthenticated = _userService.loginUser();
        return Task.CompletedTask;
    }
}