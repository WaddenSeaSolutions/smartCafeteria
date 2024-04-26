using backend.Service;
using Fleck;
using System.Text.Json;
using backend.Interface;
using Microsoft.IdentityModel.Tokens;

namespace backend.WebSockets.MessageHandlers
{
    public class AuthenticationHandler : IMessageHandler
    {
        private readonly ITokenService _tokenService;

        public AuthenticationHandler(ITokenService tokenService)
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
                
                // Update the ConnectionMetadata for this connection
                var connectionId = Guid.Parse(socket.ConnectionInfo.Id.ToString());
                if (WebSocketManager._connectionMetadata.TryGetValue(connectionId, out var connectionMetadata))
                {
                    connectionMetadata.Username = user.Username;
                    connectionMetadata.UserId = user.Id;
                    connectionMetadata.Authenticated = true;
                    connectionMetadata.Role = user.Role;
                    connectionMetadata.IsAdmin = user.Role == "admin";
                }
                else
                {
                    Console.WriteLine("Exception thrown in AdminAuthenticationHandler");
                }

                socket.Send("User Authenticated");
            }
            catch (SecurityTokenException)
            {
                socket.Send("Invalid token");
            }
        }
    }
}