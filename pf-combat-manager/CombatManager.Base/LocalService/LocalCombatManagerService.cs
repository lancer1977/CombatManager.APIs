using EmbedIO;

namespace CombatManager.LocalService
{
    public class LocalCombatManagerService
    {
        public enum UiAction
        {
            BringToFront,
            Minimize,
            Goto,
            ShowCombatListWindow,
            HideCombatListWindow
        }

        public class UiActionEventArgs : EventArgs
        {
            public UiAction Action { get; set; }
            public object Data { get; set; }
        }

        public delegate void UiActionEvent(object sender, UiActionEventArgs args);


        public event UiActionEvent UiActionTaken;



        CombatState _state;
        int _port;
        WebServer _server;
        Character.HpMode _hpmode;
        string _passcode;

        public const ushort DefaultPort = 12457;


        public delegate void ActionCallback(Action action);


        public ActionCallback StateActionCallback { get; set; }
        public Action SaveCallback { get; set; }


        public LocalCombatManagerServiceController ServiceController;

        public Character.HpMode HpMode
        {
            get => _hpmode;
            set
            {
                if (_hpmode != value)
                {
                    _hpmode = value;
                    if (ServiceController != null)
                    {
                        ServiceController.HpMode = value;
                    }
                }
            }

        }

        public LocalCombatManagerService(CombatState state, ushort port = DefaultPort, string passcode = null)
        {
            this._state = state;
            this._port = port;
            this._passcode = passcode;

        }

        void RunActionCallback(Action action)
        {
            StateActionCallback?.Invoke(action);
        }

        void RunSaveCallback()
        {
            SaveCallback?.Invoke();
        }

        public void TakeUiAction(UiAction ui, object data = null)
        {
            UiActionTaken?.Invoke(this, new UiActionEventArgs() { Action = ui, Data = data });
        }

        public void Start(bool runWeb = true)
        {
            //EndPointManager.UseIpv6 = false; 
            var universalUrl = $"http://*:{_port}";
            _server = new WebServer(o =>
                    o
                        .WithUrlPrefix(universalUrl)
                        .WithMode(HttpListenerMode.EmbedIO)
                        )
            .WithLocalSessionManager();

            if (runWeb)
            {
                _server.WithModule(new CombatManagerHtmlServer("/www/", _state, RunActionCallback));
            }

            _server.WithModule(new ImageServer())
            .WithModule(new CombatManagerNotificationServer("/api/notification/", _state))
            .WithWebApi("/api", m => m.RegisterController(() => new LocalCombatManagerServiceController(null, _state, this, RunActionCallback, RunSaveCallback)));
            _server.RunAsync();

        }

        public string Passcode
        {
            get => _passcode;
            set
            {
                _passcode = value;
                if (ServiceController != null)
                {
                    ServiceController.Passcode = _passcode;
                }
            }
        }



        public void Close()
        {
            if (_server != null)
            {
                try
                {
                    _server.Dispose();
                }
                catch (Exception)
                {

                }
                _server = null;
            }

        }
    }
}
