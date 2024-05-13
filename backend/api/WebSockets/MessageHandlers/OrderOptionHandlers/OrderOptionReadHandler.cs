using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionReadHandler : IMessageHandler
{
    private readonly IOrderService _orderService;
    
    public OrderOptionReadHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin
            || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer");
        {
            List<OrderOption> orderOptions = _orderService.GetOrderOptions();
            var response = new
            {
                eventType = "orderOptions",
                orderOptions = orderOptions
            };
            string orderOptionsJson = JsonSerializer.Serialize(response);
            
            Console.WriteLine(orderOptionsJson);
            socket.Send(orderOptionsJson);
            return Task.CompletedTask;
            
        }

        socket.Send("You do not have permission to read order options.");
        return Task.CompletedTask;
    }
}