using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using backend.Service;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class RegisterCustomerHandler : IMessageHandler
{
    private readonly RegisterCustomerService _registerCustomerService;

    public RegisterCustomerHandler(RegisterCustomerService registerCustomerService)
    {
        _registerCustomerService = registerCustomerService;
    }

    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        Console.WriteLine("handler");
        RegisterCustomerData registerCustomerData =  JsonSerializer.Deserialize<RegisterCustomerData>(message);

        var result = _registerCustomerService.RegisterCustomer(registerCustomerData);
        if (result = true)
        {
            await socket.Send(registerCustomerData.Username);
        }
    }
    
    public class RegisterCustomerData
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