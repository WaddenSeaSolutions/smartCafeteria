using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class OrderFromCustomerHandler : IMessageHandler
{
    
    private readonly IOrderOptionService _orderOptionService;
    
    public OrderFromCustomerHandler(IOrderOptionService orderOptionService)
    {
        _orderOptionService = orderOptionService;
    }
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderDTO orderDto = JsonSerializer.Deserialize<OrderDTO>(message);
            //Set the order to not done and not paid
            orderDto.Done = false;
            orderDto.Payment = false;
            orderDto.UserId = WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].UserId;
            
            
            
        }



    throw new NotImplementedException();
    }
}