using CombatManager.LocalService;
using CombatManager.Utilities;
using PolyhydraGames.Core.IOC;

#if MONO

#else
#endif

namespace CombatManager
{

    //these settings are set in a platform appropriate manner in the core
    //the settings are automatically stored when changed

    public class CoreSettings : SimpleNotifyClass
    {
        public IPreferences Preferences { get; }
        private static CoreSettings _instance;

        private CoreSettings(IPreferences preferences)
        {
            Preferences = preferences;
        }

        public static CoreSettings Instance => _instance ??= IOC.Get<CoreSettings>();

        public bool RunLocalService
        {
            get => Preferences.LoadBoolValue("RunLocalService", false);
            set
            {
                if (RunLocalService == value) return;
                Preferences.SaveBoolValue("RunLocalService", value);
                Notify();
            }
        }


        public int LocalServicePort
        {
            get => Preferences.LoadIntValue("LocalServicePort", LocalCombatManagerService.DefaultPort);
            set
            {
                if (value > 0 && value < 32778 && LocalServicePort != value)
                {

                    Preferences.SaveIntValue("LocalServicePort", value);
                    Notify();
                }
            }
        }

        public string LocalServicePasscode
        {

            get => Preferences.LoadStringValue("LocalServicePasscode", "");
            set
            {
                if (LocalServicePasscode == value) return;
                Preferences.SaveStringValue("LocalServicePasscode", value);
                Notify();
            }
        }

        public bool AutomaticStabilization
        {
            get => Preferences.LoadBoolValue("AutomaticStabilization", false);
            set
            {
                if (AutomaticStabilization == value) return;
                Preferences.SaveBoolValue("AutomaticStabilization", value);
                Notify();
            }
        }



    }
}
