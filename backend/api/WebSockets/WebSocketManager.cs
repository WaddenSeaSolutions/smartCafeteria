using System.Text.Json;
using backend.Model;
using Fleck;
using backend.WebSockets.MessageHandlers;

public class WebSocketManager
{
    private readonly Dictionary<string, IMessageHandler> _messageHandlers;
    private readonly WebSocketServer _server;

    public static Dictionary<Guid, ConnectionMetadata> _connectionMetadata;
    
    public WebSocketManager(Dictionary<string, IMessageHandler> messageHandlers)
    {
        _messageHandlers = messageHandlers;
        _connectionMetadata = new Dictionary<Guid, ConnectionMetadata>();

        var port = Environment.GetEnvironmentVariable("PORT") ?? "8181";
        _server = new WebSocketServer("ws://0.0.0.0:"+port);
        
        _server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                var connectionMetadata = new ConnectionMetadata
                {
                    ConnectionId = socket.ConnectionInfo.Id.ToString(),
                    Socket = socket
                };
                _connectionMetadata[socket.ConnectionInfo.Id] = connectionMetadata;
            };
            
            socket.OnMessage = message =>
            {
                var jsonDocument = JsonDocument.Parse(message);
                var messageType = jsonDocument.RootElement.GetProperty("action").GetString();
                HandleMessage(messageType, message, socket);
            };
            socket.OnClose = () =>
            {
                _connectionMetadata.Remove(socket.ConnectionInfo.Id);
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