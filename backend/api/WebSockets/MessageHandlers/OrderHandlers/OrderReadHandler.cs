using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class OrderReadHandler : IMessageHandler
{
    private readonly IOrderService _orderService;

    public OrderReadHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {

            List<Order> orders = _orderService.GetOrders();

            var response = new
            {
                eventType = "ordersRead",
                orders = orders
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };

            string ordersJson = JsonSerializer.Serialize(response, options);
            socket.Send(ordersJson);
        }
        else
        {
            socket.Send("You do not have permission to read orders.");
        }
        return Task.CompletedTask;
    }
}