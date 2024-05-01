using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionDeleteHandler : IMessageHandler
{
    private readonly IOrderService _orderService;

    public OrderOptionDeleteHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderOption orderOption = JsonSerializer.Deserialize<OrderOption>(message);

            OrderOption deletedOrderOption = _orderService.DeleteOrderOption(orderOption);
            DeletedOrderOption deletedOrderOptionJson = new DeletedOrderOption
            {
                OrderOption = deletedOrderOption,
                Deleted = true
            };

            string deletedOrderOptionJsonString = JsonSerializer.Serialize(deletedOrderOptionJson);

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

    public class DeletedOrderOption
    {
        public OrderOption OrderOption { get; set; }
        public bool Deleted { get; set; }
    }
