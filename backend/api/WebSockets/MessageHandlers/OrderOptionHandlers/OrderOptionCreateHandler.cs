using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionCreateHandler : IMessageHandler
{
    private readonly IOrderService _orderService;
    
    public OrderOptionCreateHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderOptionDTO orderOptionDto = JsonSerializer.Deserialize<OrderOptionDTO>(message);
            
            OrderOption orderOptionToJson = _orderService.CreateOrderOption(orderOptionDto);
            orderOptionToJson.isNew = true; //Shows frontend that it needs to add this orderoption to the list.
            
            string orderOptionJson = JsonSerializer.Serialize(orderOptionToJson);
            foreach (var connection in WebSocketManager._connectionMetadata)
            {
                if (connection.Value.Role == "personnel" || connection.Value.IsAdmin)
                {
                    connection.Value.Socket.Send(orderOptionJson);
                }
            }

            return Task.CompletedTask;
        }
        
        socket.Send("Unauthorized");
        return Task.CompletedTask;
    }
}