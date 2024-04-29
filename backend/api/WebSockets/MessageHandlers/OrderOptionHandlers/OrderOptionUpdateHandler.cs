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
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "admin")
        {
            OrderOption orderOption = JsonSerializer.Deserialize<OrderOption>(message);
            
            OrderOption updatedOrderOption = _orderService.UpdateOrderOption(orderOption);
            string updatedOrderOptionJson = JsonSerializer.Serialize(updatedOrderOption);
            
            socket.Send(updatedOrderOptionJson);
            return Task.CompletedTask;
        }

        throw new NotImplementedException();
    }
}