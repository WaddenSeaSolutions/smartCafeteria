using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionReadHandler : IMessageHandler
{
    private readonly IOrderOptionService _orderOptionService;
    
    public OrderOptionReadHandler(IOrderOptionService orderOptionService)
    {
        _orderOptionService = orderOptionService;
    }
    
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin
            || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer");
        {
            List<OrderOption> orderOptions = _orderOptionService.GetOrderOptions();
            var response = new
            {
                eventType = "orderOptions",
                orderOptions = orderOptions
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };
            string orderOptionsJson = JsonSerializer.Serialize(response, options);
            
            Console.WriteLine(orderOptionsJson);
            socket.Send(orderOptionsJson);
            return Task.CompletedTask;
            
        }

        socket.Send("You do not have permission to read order options.");
        return Task.CompletedTask;
    }
}