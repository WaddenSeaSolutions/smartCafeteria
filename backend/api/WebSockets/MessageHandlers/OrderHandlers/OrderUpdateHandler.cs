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
        // Deserialize the message into an order DTO
        OrderDTO orderDto = JsonSerializer.Deserialize<OrderDTO>(message);

        // Check if the user is authorized to update the order
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            // Update the order
            Order updatedOrder = _orderService.UpdateOrder(orderDto);

            // Create a response
            var response = new
            {
                eventType = "orderUpdated",
                order = updatedOrder
            };

            // Serialize the response into a JSON string
            string orderJson = JsonSerializer.Serialize(response);

            // Send the updated order to all relevant connections
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