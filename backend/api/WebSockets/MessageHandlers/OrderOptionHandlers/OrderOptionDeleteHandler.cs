using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionDeleteHandler : IMessageHandler
{
    private readonly IOrderOptionService _orderOptionService;

    public OrderOptionDeleteHandler(IOrderOptionService orderOptionService)
    {
        _orderOptionService = orderOptionService;
    }

    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderOption orderOption = JsonSerializer.Deserialize<OrderOption>(message);
            
            orderOption = _orderOptionService.DeleteOrderOption(orderOption);
            var response = new
            {
                eventType = "orderOptionDeleted",
                orderOption = orderOption
            };
            
            string deletedOrderOptionJsonString = JsonSerializer.Serialize(response);

            foreach (var connection in WebSocketManager._connectionMetadata)
            {
                if (connection.Value.Role == "personnel" || connection.Value.IsAdmin)
                {
                    connection.Value.Socket.Send(deletedOrderOptionJsonString);
                }
            }

            return Task.CompletedTask;
        }

        socket.Send("Unauthorized");
        return Task.CompletedTask;
    }
}
