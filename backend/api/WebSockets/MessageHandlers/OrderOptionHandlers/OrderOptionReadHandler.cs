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
        if (WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].Role == "personnel" || WebSocketManager._connectionMetadata[socket.ConnectionInfo.Id].IsAdmin)
        {
            List<OrderOption> orderOptions = _orderService.GetOrderOptions();
            
            string orderOptionsJson = JsonSerializer.Serialize(orderOptions);
            
            socket.Send(orderOptionsJson);
            
        }
        return Task.CompletedTask;
    }
}