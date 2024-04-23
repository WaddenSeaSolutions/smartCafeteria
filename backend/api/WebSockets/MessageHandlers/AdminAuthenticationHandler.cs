using backend.Service;
using Fleck;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace backend.WebSockets.MessageHandlers
{
    public class AdminAuthenticationHandler : IMessageHandler
    {
        private readonly TokenService _tokenService;

        public AdminAuthenticationHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }
      
        public async Task HandleMessage(string message, IWebSocketConnection socket)
        {
            // Extract the token from the message
            var jsonDocument = JsonDocument.Parse(message);
            var token = jsonDocument.RootElement.GetProperty("Token").GetString();
            
            try
            {
                // Validate the token and get the user
                var user = _tokenService.validateTokenAndReturnUser(token);

                // Check if the user is an admin
                if (user.Role != "admin")
                {
                    socket.Send("Unauthorized");
                    return;
                }
                // Update the ConnectionMetadata for this connection
                var connectionId = Guid.Parse(socket.ConnectionInfo.Id.ToString());
                if (WebSocketManager._connectionMetadata.TryGetValue(connectionId, out var connectionMetadata))
                {
                    connectionMetadata.Username = user.Username;
                    connectionMetadata.Authenticated = true;
                    connectionMetadata.IsAdmin = user.Role == "admin";
                }
                else
                {
                    Console.WriteLine("Exeption thrown in AdminAuthenticationHandler");
                    throw new Exception("Connection not found in _connectionMetadata dictionary");
                }

                socket.Send("Admin authenticated");
            }
            catch (SecurityTokenException)
            {
                socket.Send("Invalid token");
            }
        }
    }
}