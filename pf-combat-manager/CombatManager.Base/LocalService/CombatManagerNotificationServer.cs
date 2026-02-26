using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CombatManager.Api.Core;
using EmbedIO.WebSockets;

namespace CombatManager.LocalService
{
    public class CombatManagerNotificationServer : WebSocketModule
    {
        private readonly List<User> _users = new List<User>();
        private readonly CombatState _state;

        public CombatManagerNotificationServer(string path, CombatState state)
            : base(path, true)
        {
            _state = state;
            base.MaxMessageSize = 10000;
            state.CharacterAdded += State_CharacterAdded;
            state.CharacterRemoved += State_CharacterRemoved;
            state.CharacterPropertyChanged += State_CharacterPropertyChanged;
            state.CharacterSortCompleted += State_CharacterSortCompleted;
            state.PropertyChanged += State_PropertyChanged;
            state.TurnChanged += State_TurnChanged;
        }

        void SendState(string title)
        {
            var remoteState = _state.ToRemote();
            var message = LocalServiceMessage.Create(title, remoteState);
            var data = JsonSerializer.Serialize(message);
            new Thread(() => BroadcastAsync(data)).Start();
        }

        void SendState(string title, IWebSocketContext context)
        {
            var remoteState = _state.ToRemote();
            var message = LocalServiceMessage.Create(title, remoteState);
            var data = JsonSerializer.Serialize(message);
            new Thread(() => BroadcastAsync(data, x => x == context)).Start();
        }

        private void State_TurnChanged(object sender, CombatStateCharacterEventArgs e)
        {
            SendState(MessageDictionary.Turn);
        }


        private void State_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private void State_CharacterSortCompleted(object sender, EventArgs e)
        {

        }

        private void State_CharacterPropertyChanged(object sender, CombatStateCharacterEventArgs e)
        {
            SendState(e.Property);
    
        }

        private void State_CharacterRemoved(object sender, CombatStateCharacterEventArgs e)
        { 
            SendState(MessageDictionary.Removed);
        }

        private void State_CharacterAdded(object sender, CombatStateCharacterEventArgs e)
        {
            SendState(MessageDictionary.Added);
        }


        protected override async Task OnMessageReceivedAsync(IWebSocketContext context, byte[] buffer, IWebSocketReceiveResult result)
        {
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer);
            var sender = _users.FirstOrDefault(x => x.Id == context.Id);
            while (true)
            {
                using (var ms = new MemoryStream())
                {

                    //do
                    {
                        ms.Write(bufferSegment.Array, bufferSegment.Offset, result.Count);
                    }
                    //while (!result.EndOfMessage);

                    ms.Seek(0, SeekOrigin.Begin);
                    var socket = result  ;
                    if (socket.MessageType == (int)WebSocketMessageType.Text)
                    {
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            var text = await reader.ReadToEndAsync();
                            var obj = JsonSerializer.Deserialize<LocalServiceMessage>(text);
                            Console.WriteLine(sender.Name + " " + obj.Name + ":" + obj.Data);
                        }
                    }

                }
            }


        }


        protected override Task OnClientConnectedAsync(IWebSocketContext context)
        {
            _users.Add(new User(context));

            var message = LocalServiceMessage.Create("Users", _users);
            var data = JsonSerializer.Serialize(message);
            new Thread(() => BroadcastAsync(data)).Start();
            return Task.CompletedTask;
        }

        protected override Task OnFrameReceivedAsync(IWebSocketContext context, byte[] rxBuffer, IWebSocketReceiveResult rxResult)
        {

            return Task.CompletedTask;
        }

        protected override Task OnClientDisconnectedAsync(IWebSocketContext context)
        {
            var user = _users.FirstOrDefault(x => x.Id == context.Id);
            if (user != null)
                _users.Remove(user);
            return Task.CompletedTask;
        }


    }


}
