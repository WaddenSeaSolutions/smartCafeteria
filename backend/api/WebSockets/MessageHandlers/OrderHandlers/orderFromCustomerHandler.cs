using System.Text.Json;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class orderFromCustomerHandler : IMessageHandler
{
    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderDTO orderDto = JsonSerializer.Deserialize<OrderDTO>(message);
            //Set the order to not done and not paid
            orderDto.Done = false;
            orderDto.Payment = false;
            
            
        }



    throw new NotImplementedException();
    }
}