using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers.OrderHandlers;

public class OrderCreateHandler : IMessageHandler
{

    private readonly IOrderService _orderService;
    
    public OrderCreateHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    public async Task HandleMessage(string message, IWebSocketConnection socket)
    {
        Console.WriteLine("OrderCreateHandler called");
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "customer" ||
            WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            OrderDTO orderDto = JsonSerializer.Deserialize<OrderDTO>(message);
            //Set the order to not done and not paid
            orderDto.Done = false;
            orderDto.Payment = false;
            orderDto.UserId = WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].UserId;

            Console.WriteLine("before timezone");

            try
            {
                DateTime localDateTime = DateTime.Now;
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTimeOffset dateTimeOffset = TimeZoneInfo.ConvertTime(localDateTime, tzi);
                orderDto.Timestamp = dateTimeOffset.ToUniversalTime();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during timezone conversion: " + e);
                throw;
            }

            Order order = _orderService.CreateOrder(orderDto);
            
        }
        else
        {
            socket.Send("You are not authorized to create an order.");
        }
    }
}