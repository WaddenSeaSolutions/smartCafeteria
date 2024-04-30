using Fleck;

public class ConnectionMetadata
{
    public string ConnectionId { get; set; }
    public string Role { get; set; }
    public bool IsAdmin { get; set; }
    public IWebSocketConnection Socket { get; set; }
}