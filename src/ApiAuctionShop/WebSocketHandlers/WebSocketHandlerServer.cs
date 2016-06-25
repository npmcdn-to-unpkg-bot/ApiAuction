using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projekt.Controllers
{

    /// <summary>
    /// Mozna uzyc tego do chatu potem
    /// </summary>
    public class WebSocketHandlerServer
    { 
        public static async Task ChatHandler(HttpContext http, Func<Task> next)
        {
            if (http.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                        var token = CancellationToken.None;
                        var buffer = new ArraySegment<Byte>(new Byte[4096]);

                        var received = await webSocket.ReceiveAsync(buffer, token);

                        switch (received.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                var request = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
                                var buffor2 = new ArraySegment<Byte>(Encoding.UTF8.GetBytes(request + " test"));
                                var type = WebSocketMessageType.Text;
                                await webSocket.SendAsync(buffor2, type, true, token);
           
                                break;
                        }
                }
            }
            else
            {
                await next();
            }
        }
    }
}
