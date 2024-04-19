using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using backend.Model;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class RegisterUserHandler : IMessageHandler
{
    private readonly RegisterUserService _registerUserService;

    public RegisterUserHandler(RegisterUserService registerUserService)
    {
        _registerUserService = registerUserService;
    }

    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        RegisterUserData registerUserData =  JsonSerializer.Deserialize<RegisterUserData>(message);

        var result = _registerUserService.RegisterUser(registerUserData);
        if (result = true)
        {
            await socket.Send(registerUserData.Username);
        }
    }
    
    public class RegisterUserData
    {
        [Required]
        [MinLength(5)]
        [StringLength(20)]
        public  string Username { get; set; }
    
        [Required]
        [MinLength(8)]
        [StringLength(50)]
        public string Password { get; set; }
    
    }
}