using System.Text.Json;
using backend.Interface;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers;

public class OrderOptionReadHandler : IMessageHandler
{
    private readonly IOrderService _orderService;
    
    public OrderOptionReadHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }


    public Task HandleMessage(string message, IWebSocketConnection socket)
    {
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "admin")
        {
            OrderOption orderOption = _orderService.GetOrderOptionById(int.Parse(message));
            string orderOptionJson = JsonSerializer.Serialize(orderOption);
            socket.Send(orderOptionJson);
            return Task.CompletedTask;
        }
        
        socket.Send("Unauthorized");
        return Task.CompletedTask;
    }
}