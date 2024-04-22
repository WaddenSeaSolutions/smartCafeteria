using System.Text.Json;
using backend.Model;
using Fleck;
using backend.WebSockets.MessageHandlers;

public class WebSocketManager
{
    private readonly Dictionary<string, IMessageHandler> _messageHandlers;
    private readonly WebSocketServer _server;

    private readonly Dictionary<Guid, ConnectionMetadata> _connectionMetadata;
    
    public WebSocketManager(Dictionary<string, IMessageHandler> messageHandlers)
    {
        _messageHandlers = messageHandlers;
        _connectionMetadata = new Dictionary<Guid, ConnectionMetadata>();
        
        _server = new WebSocketServer("ws://0.0.0.0:8181");
        
        _server.Start(socket =>
        {
            socket.OnMessage = message =>
            {
                var jsonDocument = JsonDocument.Parse(message);
                var messageType = jsonDocument.RootElement.GetProperty("Type").GetString();
                HandleMessage(messageType, message, socket);
            };
        });
    }

    public async Task HandleMessage(string messageType, string message, IWebSocketConnection socket)
    {
        if (_messageHandlers.TryGetValue(messageType, out var handler))
        {
            await handler.HandleMessage(message, socket);
        }
        else
        {
            throw new Exception($"No handler registered for message type {messageType}");
        }
    }
}