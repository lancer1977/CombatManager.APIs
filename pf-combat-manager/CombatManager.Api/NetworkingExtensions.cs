using System;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CombatManager.Api.Core;
using CombatManager.Api.Core.Data;
using Newtonsoft.Json;

namespace CombatManager.Api
{
    public static class NetworkingExtensions
    {
        public static async Task SendAsync(this ClientWebSocket socket, string messageType, object data)
        {
            var serviceMessage = new RemoteServiceMessage()
            {
                Name = messageType,
                Data = data
            };
            var json = JsonConvert.SerializeObject(serviceMessage);
            var bytes = Encoding.UTF8.GetBytes(json);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);

        }

        /// <summary>
        /// Hits the server to tell it your alive, sends binary to encourage this call to be mostly ignored. Delay included at 1k default milliseconds
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public static async Task PingAsync(this ClientWebSocket socket, int milliseconds = 1000)
        {
            var serviceMessage = new RemoteServiceMessage()
            {
                Name = MessageDictionary.Ping,
                Data = null
            };
            var json = JsonConvert.SerializeObject(serviceMessage);
            var bytes = Encoding.UTF8.GetBytes(json);
            await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, endOfMessage: true, cancellationToken: CancellationToken.None);
            await Task.Delay(milliseconds);
        }

        public static async Task<T> PostAsync<T>(string baseaddress, string endpoint, object parameters)
        {
            var json = JsonConvert.SerializeObject(parameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseaddress);
                var response = await client.PostAsync(endpoint, data);

                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }

        public static async Task<T> GetAsync<T>(string baseaddress, string endpoint, string passcode)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("passcode", passcode);
                client.BaseAddress = new Uri(baseaddress);
                var result = await client.GetAsync(endpoint);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Error: " + result.StatusCode.ToString() + $" Address: {baseaddress}{endpoint}");
                var content = await result.Content.ReadAsStringAsync();
   
                Console.WriteLine(content);
                Console.WriteLine(result.StatusCode);
                return JsonConvert.DeserializeObject<T>(content);
            }

        }
        public static async Task<string> GetAsync(string baseaddress, string endpoint, string passcode)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("passcode", passcode);
                client.BaseAddress = new Uri(baseaddress);
                var result = await client.GetAsync(endpoint);
                if (result.StatusCode != HttpStatusCode.OK)
                    throw new Exception("Error: " + result.StatusCode.ToString() + $" Address: {baseaddress}{endpoint}");
                var content = await result.Content.ReadAsStringAsync();
                //var obj = JObject.FromObject( result.Content.ReadAsStreamAsync());
                Console.WriteLine(content);
                Console.WriteLine(result.StatusCode);
                return content;
            }

        } 
    }
}