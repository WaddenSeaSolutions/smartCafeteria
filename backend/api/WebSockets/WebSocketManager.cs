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
        
        _server = new WebSocketServer("ws://0.0.0.0:8181");
        
        _server.Start(socket =>
        {
            socket.OnOpen = () =>
            {
                var connectionMetadata = new ConnectionMetadata
                {
                    ConnectionId = socket.ConnectionInfo.Id.ToString(),
                };
               // _connectionMetadata[socket.ConnectionInfo.Id] = connectionMetadata;
                _connectionMetadata.Add(socket.ConnectionInfo.Id, connectionMetadata);
            };  
            
            socket.OnMessage = message =>
            {
                var jsonDocument = JsonDocument.Parse(message);
                var messageType = jsonDocument.RootElement.GetProperty("action").GetString();
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