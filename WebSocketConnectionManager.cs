using System.Net.WebSockets;
using System.Collections.Concurrent;

public class WebSocketConnectionManager
{
    private readonly ConcurrentDictionary<int, WebSocket> _connections = new();

    public void AddConnection(int playerId, WebSocket socket)
    {
        _connections[playerId] = socket;
    }

    public WebSocket? GetConnection(int playerId)
    {
        _connections.TryGetValue(playerId, out var socket);
        return socket;
    }

    public async Task RemoveConnection(int playerId)
    {
        if (_connections.TryRemove(playerId, out var socket))
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
        }
    }
}
