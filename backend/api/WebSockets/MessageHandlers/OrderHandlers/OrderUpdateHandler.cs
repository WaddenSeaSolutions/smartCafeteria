using backend.Model;
using Fleck;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Interface;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class OrderUpdateHandler : IMessageHandler
{
    private readonly IOrderService _orderService;

    public OrderUpdateHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        
        Order order = JsonSerializer.Deserialize<Order>(message);
        Console.WriteLine(order.Done);
        Console.WriteLine(order.Payment);

        // Check if the user is authorized to update the order
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            Order updatedOrder = _orderService.UpdateOrder(order);
            // Create a response
            var response = new
            {
                eventType = "orderUpdated",
                order = updatedOrder
            };

            string orderJson = JsonSerializer.Serialize(response);

            foreach (var connection in WebSocketManager._connectionMetadata)
            {
                if (connection.Value.UserId == updatedOrder.UserId || connection.Value.IsAdmin || connection.Value.Role == "personnel")
                {
                    await connection.Value.Socket.Send(orderJson);
                }
            }
        }
        else
        {
            socket.Send("You are not authorized to update an order");
        }
    }
}