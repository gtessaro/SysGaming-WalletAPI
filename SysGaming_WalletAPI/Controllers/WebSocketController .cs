using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace SysGaming_WalletAPI.Controllers
{
    [ApiController]
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly WebSocketConnectionManager _webSocketManager;

        public WebSocketController(WebSocketConnectionManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }

        [HttpGet("{playerId}")]
        public async Task Get(int playerId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _webSocketManager.AddConnection(playerId, socket);

                await ReceiveMessages(socket); 
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task ReceiveMessages(WebSocket socket)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);
                }
            }
        }
    }
}