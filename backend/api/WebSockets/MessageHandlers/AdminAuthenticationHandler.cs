using backend.Attributes;
using backend.Model;
using Fleck;

namespace backend.WebSockets.MessageHandlers
{
    [AuthorizeAdmin]
    public class AdminAuthenticationHandler : IMessageHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminAuthenticationHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
      
        public Task HandleMessage(string message, IWebSocketConnection socket)
        {
            Console.WriteLine("hej kenni");
            // Get the user from HttpContext
            var user = _httpContextAccessor.HttpContext.Items["User"] as User;
            // Update the ConnectionMetadata for this connection
            var connectionId = Guid.Parse(socket.ConnectionInfo.Id.ToString());
            Console.WriteLine("Admin authenticated");
            if (WebSocketManager._connectionMetadata.TryGetValue(connectionId, out var connectionMetadata))
            {
                connectionMetadata.Username = user.Username;
                connectionMetadata.Authenticated = true;
                connectionMetadata.IsAdmin = user.Role == "admin";
            }
            else
            {
                throw new Exception("Connection not found in _connectionMetadata dictionary");
            }

            socket.Send("Admin authenticated");
            return Task.CompletedTask;
        }
    }
}