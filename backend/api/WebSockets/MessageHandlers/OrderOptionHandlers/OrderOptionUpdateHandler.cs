using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionUpdateHandler : IMessageHandler
{
    private readonly IOrderService _orderService;
    
    public OrderOptionUpdateHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderOption orderOption = JsonSerializer.Deserialize<OrderOption>(message);
        
            OrderOption updatedOrderOption = _orderService.UpdateOrderOption(orderOption);
            var response = new
            {
                eventType = "orderOptionUpdated",
                orderOption = updatedOrderOption
            };
            string updatedOrderOptionJson = JsonSerializer.Serialize(response);
        
            foreach (var connection in WebSocketManager._connectionMetadata.Values)
            {
                if (connection.Role == "personnel" || connection.IsAdmin)
                {
                    connection.Socket.Send(updatedOrderOptionJson);
                }
            }
        
            return Task.CompletedTask;
        }
    
        return Task.CompletedTask;
    }
}