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
            updatedOrderOption.IsUpdated = true; //Shows frontend that it needs to replace this order option instead of adding it to the list.
            
            string updatedOrderOptionJson = JsonSerializer.Serialize(updatedOrderOption);
        
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