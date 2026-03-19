using System;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CombatManager.Api.Core;
using CombatManager.Api.Core.Data;
using Newtonsoft.Json;

namespace CombatManager.Api
{



    public class NotificationApiChat
    {
        public event EventHandler<RemoteServiceMessage> StateChanged;
        private readonly string _address;
        public NotificationApiChat(string address)
        {
            _address = address;
        }

        public async Task StartConnection(CancellationToken cts)
        {
            while (true)
            {
                if (cts.IsCancellationRequested) return;
                try
                {

                    await Connect(cts);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        public async Task Connect(CancellationToken cts)
        {
            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(_address), CancellationToken.None);
            Console.WriteLine("Connected");
            var receiving = Receiving(ws);
            var sending = Send(ws);
            var keepAlive = KeepAlive(ws);
            await Task.WhenAll(sending, receiving, keepAlive);
        }

        private async Task KeepAlive(ClientWebSocket ws)
        {
            while (true)
            {
                await ws.PingAsync();
                Debug.WriteLine("Keeping Alive");
            }
        }

        private async Task Send(ClientWebSocket ws)
        {
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                await ws.SendAsync(MessageDictionary.Message, line);
            }

            await ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
        }

        private async Task Receiving(ClientWebSocket socket)
        {
            Debug.WriteLine("Receiving");
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[8192]);
            while (true)
            {
                using (var ms = new MemoryStream())
                {
                    WebSocketReceiveResult result = null;
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    }
                    while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            var text = await reader.ReadToEndAsync();
                            var obj = JsonConvert.DeserializeObject<RemoteServiceMessage>(text);
                            StateChanged?.Invoke(this, obj);
                        }
                    }

                }
            }

        }
    }
}
