using Fleck;

namespace backend.WebSockets.MessageHandlers;

public interface IMessageHandler
{
    Task HandleMessage(string message, IWebSocketConnection socket);
}