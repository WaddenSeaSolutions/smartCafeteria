using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class LoginMessageHandler : IMessageHandler
{
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        Console.WriteLine("Ola mi amigo");
        return Task.CompletedTask;
    }
}