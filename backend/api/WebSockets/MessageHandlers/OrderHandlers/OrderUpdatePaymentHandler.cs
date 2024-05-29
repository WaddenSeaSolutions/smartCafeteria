using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class OrderUpdatePaymentHandler : IMessageHandler
{
    private readonly IOrderService _orderService;
    
    public OrderUpdatePaymentHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        UpdatePaymentOnOrderDTO updatePaymentOnOrderDto = JsonSerializer.Deserialize<UpdatePaymentOnOrderDTO>(message);

        
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            Order updatedOrder = _orderService.UpdatePaymentOnOrder(updatePaymentOnOrderDto);
            
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