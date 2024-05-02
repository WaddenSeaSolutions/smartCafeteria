using System.Text.Json;
using backend.Interface;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class RegisterPersonnelHandler : IMessageHandler
{
    private readonly IUserService _userService;

    
    public RegisterPersonnelHandler(IUserService userService)
    {
        _userService = userService;
    }
    
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        Console.WriteLine("Hello");
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            RegisterPersonnelData registerPersonnelData = JsonSerializer.Deserialize<RegisterPersonnelData>(message);

            _userService.registerPersonnel(registerPersonnelData.Username, registerPersonnelData.Password, "Personnel");

            socket.Send("Personnel registered");
        }
        else
        {
            socket.Send("Unauthorized");
        }

        return Task.CompletedTask;
    }
    
    public class RegisterPersonnelData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}