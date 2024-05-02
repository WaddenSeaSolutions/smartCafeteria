using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderHandler : IMessageHandler
{
    private readonly IOrderService _orderService;

    public OrderHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    /// <summary>
    /// A method that takes a message and checks if the user has a valid role to create an order.
    /// If valid it deserializes it to an order object and creates it in DB, thereafter sends a order
    /// object to all open personnel connections.
    /// </summary>
    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        // Check if the connection is in the dictionary and if the associated role is valid
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role != "customer")
        {
            socket.Send("You do not have permission to create an order.");
            return;
        }
        
        //Deserialize the message to a order object
        Order order = JsonSerializer.Deserialize<Order>(message);

        
        
        throw new NotImplementedException();
    }
}

public class orderDTO
{
    
    //Todo ift. om den skal ændres til et objekt med 7 properties
}