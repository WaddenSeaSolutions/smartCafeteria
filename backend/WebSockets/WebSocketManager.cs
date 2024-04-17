using System.Net.WebSockets;
using System.Text.Json;
using Fleck;
using backend.WebSockets.MessageHandlers;

public class WebSocketManager
{
    private readonly Dictionary<string, IMessageHandler> _messageHandlers;
    private readonly WebSocketServer _server;

    public WebSocketManager(Dictionary<string, IMessageHandler> messageHandlers)
    {
        _messageHandlers = messageHandlers;

        _server = new WebSocketServer("ws://0.0.0.0:8181");
        _server.Start(socket =>
        {
            socket.OnOpen = () => Console.WriteLine("Open!");
            socket.OnClose = () => Console.WriteLine("Close!");
            socket.OnMessage = message =>
            {
                var jsonDocument = JsonDocument.Parse(message);
                var messageType = jsonDocument.RootElement.GetProperty("type").GetString();
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