/*
 *  UserSettings.cs
 *
 *  Copyright (C) 2010-2012 Kyle Olson, kyle@kyleolson.com
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU General Public License
 *  as published by the Free Software Foundation; either version 2
 *  of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 */

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Win32;
using CombatManager.LocalService;

namespace CombatManager
{


    public class UserSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static UserSettings _Settings;
        private IPreferences _preferences;

        public enum SettingsSaveSection
        {
            All,
            WindowState,
            Sources,
            Initiative,
            Filters,
            System,
            LocalService,
            Timer,
        }

        public enum MonsterSetFilter
        {
            Monsters = 0,
            NPCs = 1,
            Custom = 2,
            All = 3
        }


        private bool _RollHP;
        private bool _UseCore;
        private bool _UseAPG;
        private bool _UseChronicles;
        private bool _UseModules;
        private bool _UseUltimateMagic;
        private bool _UseUltimateCombat;
        private bool _UseOther;
        private bool _ConfirmInitiativeRoll;
        private bool _ConfirmCharacterDelete;
        private bool _ConfirmClose;
        private bool _ShowAllDamageDice;
        private bool _AlternateInit3d6;
        private string _AlternateInitRoll;
        private bool _ShowHiddenInitValue;
        private bool _AddMonstersHidden;
        private bool _StatsOpenByDefault;
        private bool _CheckForUpdates;
        private Character.HPMode _DefaultHPMode;

        private bool _PlayerMiniMode;
        private bool _MonsterMiniMode;
        private int _MainWindowWidth;
        private int _MainWindowHeight;
        private int _MainWindowLeft;
        private int _MainWindowTop;
        private int _SelectedTab;

        private MonsterSetFilter _MonsterDBFilter;
        private MonsterSetFilter _MonsterTabFilter;


        private bool _RunCombatViewService;




        private bool _InitiativeShowPlayers;
        private bool _InitiativeShowMonsters;
        private bool _InitiativeHideMonsterNames;
        private bool _InitiativeHidePlayerNames;
        private bool _InitiativeShowConditions;

        private int _InitiativeConditionsSize;


        private bool _InitiativeAlwaysOnTop;
        private double _InitiativeScale;
        private bool _InitiativeFlip;

        private int _ColorScheme;
        private bool _DarkScheme;

        private bool _RunLocalService;
        private bool _RunWebService;
        private ushort _LocalServicePort;
        private string _LocalServicePasscode;

        private bool _UseTurnClock;
        private bool _CountdownToNextTurn;
        private bool _MoveAutomaticallyOnTurnTimer;
        private int _TurnTimeSeconds;
        private int _WarningTimeSeconds;
        private bool _PlayWarningSound;
        private string _WarningSound;
        private bool _PlayTurnEndSound;
        private string _TurnEndSound;
        private bool _ShowClockForMonsters;

        private int _RulesSystem;

        private bool optionsLoaded;

        public UserSettings()
        {
            _preferences = CoreSettings.Instance.Preferences;
            _RollHP = false;
            _UseAPG = true;
            _UseCore = true;
            _UseChronicles = true;
            _UseModules = true;
            _UseUltimateMagic = true;
            _UseUltimateCombat = true;
            _UseOther = true;
            _AlternateInitRoll = "3d6";
            _PlayerMiniMode = false;
            _MonsterMiniMode = false;
            _RunCombatViewService = false;
            _CheckForUpdates = true;
            _DefaultHPMode = Character.HPMode.Default;
            _MainWindowWidth = -1;
            _MainWindowHeight = -1;
            _MainWindowLeft = int.MinValue;
            _MainWindowTop = int.MinValue;
            _MonsterDBFilter = MonsterSetFilter.Monsters;
            _MonsterTabFilter = MonsterSetFilter.Monsters;
            _SelectedTab = 0;
            _InitiativeConditionsSize = 2;
            _ColorScheme = 0;
            _DarkScheme = false;
            _RunLocalService = false;
            _RunWebService = true;
            _RulesSystem = 0;
            _LocalServicePort = LocalCombatManagerService.DefaultPort;
            _LocalServicePasscode = "";
            _UseTurnClock = false;
            _CountdownToNextTurn = true;
            _MoveAutomaticallyOnTurnTimer = false;
            _TurnTimeSeconds = 180;
            _WarningTimeSeconds = 10;
            _PlayWarningSound = true;
            _WarningSound = "";
            _PlayTurnEndSound = true;
            _TurnEndSound = "";
            _ShowClockForMonsters = true;
            LoadOptions();
        }

        public bool RollHP
        {
            get { return _RollHP; }
            set
            {
                if (_RollHP != value)
                {
                    _RollHP = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RollHP")); }
                }
            }
        }

        public bool UseCore
        {
            get { return _UseCore; }
            set
            {
                if (_UseCore != value)
                {
                    _UseCore = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseCore")); }
                }
            }
        }
        public bool UseAPG
        {
            get { return _UseAPG; }
            set
            {
                if (_UseAPG != value)
                {
                    _UseAPG = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseAPG")); }
                }
            }
        }
        public bool UseChronicles
        {
            get { return _UseChronicles; }
            set
            {
                if (_UseChronicles != value)
                {
                    _UseChronicles = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseChronicles")); }
                }
            }
        }
        public bool UseModules
        {
            get { return _UseModules; }
            set
            {
                if (_UseModules != value)
                {
                    _UseModules = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseModules")); }
                }
            }
        }
        public bool UseUltimateMagic
        {
            get { return _UseUltimateMagic; }
            set
            {
                if (_UseUltimateMagic != value)
                {
                    _UseUltimateMagic = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseUltimateMagic")); }
                }
            }
        }
        public bool UseUltimateCombat
        {
            get { return _UseUltimateCombat; }
            set
            {
                if (_UseUltimateCombat != value)
                {
                    _UseUltimateCombat = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseUltimateCombat")); }
                }
            }
        }
        public bool UseOther
        {
            get { return _UseOther; }
            set
            {
                if (_UseOther != value)
                {
                    _UseOther = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseOther")); }
                }
            }
        }
        public bool ConfirmInitiativeRoll
        {
            get { return _ConfirmInitiativeRoll; }
            set
            {
                if (_ConfirmInitiativeRoll != value)
                {
                    _ConfirmInitiativeRoll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ConfirmInitiativeRoll")); }
                }
            }
        }
        public bool ConfirmCharacterDelete
        {
            get { return _ConfirmCharacterDelete; }
            set
            {
                if (_ConfirmCharacterDelete != value)
                {
                    _ConfirmCharacterDelete = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ConfirmCharacterDelete")); }
                }
            }
        }
        public bool ConfirmClose
        {
            get { return _ConfirmClose; }
            set
            {
                if (_ConfirmClose != value)
                {
                    _ConfirmClose = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ConfirmClose")); }
                }
            }
        }
        public bool ShowAllDamageDice
        {
            get { return _ShowAllDamageDice; }
            set
            {
                if (_ShowAllDamageDice != value)
                {
                    _ShowAllDamageDice = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ShowAllDamageDice")); }
                }
            }
        }
        public bool AlternateInit3d6
        {
            get { return _AlternateInit3d6; }
            set
            {
                if (_AlternateInit3d6 != value)
                {
                    _AlternateInit3d6 = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AlternateInit3d6")); }
                }
            }
        }

        public DieRoll AlternateInitDieRoll
        {
            get
            {
                return DieRoll.FromString(AlternateInitRoll);
            }
        }

        public String AlternateInitRoll
        {
            get { return _AlternateInitRoll; }
            set
            {
                if (_AlternateInitRoll != value)
                {
                    _AlternateInitRoll = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AlternateInitRoll")); }
                }
            }
        }
        public bool PlayerMiniMode
        {
            get { return _PlayerMiniMode; }
            set
            {
                if (_PlayerMiniMode != value)
                {
                    _PlayerMiniMode = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PlayerMiniMode")); }
                }
            }
        }
        public bool MonsterMiniMode
        {
            get { return _MonsterMiniMode; }
            set
            {
                if (_MonsterMiniMode != value)
                {
                    _MonsterMiniMode = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MonsterMiniMode")); }
                }
            }
        }


        public int MainWindowWidth
        {
            get { return _MainWindowWidth; }
            set
            {
                if (_MainWindowWidth != value)
                {
                    _MainWindowWidth = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MainWindowWidth")); }
                }
            }
        }
        public int MainWindowHeight
        {
            get { return _MainWindowHeight; }
            set
            {
                if (_MainWindowHeight != value)
                {
                    _MainWindowHeight = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MainWindowHeight")); }
                }
            }
        }

        public int MainWindowLeft
        {
            get { return _MainWindowLeft; }
            set
            {
                if (_MainWindowLeft != value)
                {
                    _MainWindowLeft = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MainWindowLeft")); }
                }
            }
        }
        public int MainWindowTop
        {
            get { return _MainWindowTop; }
            set
            {
                if (_MainWindowTop != value)
                {
                    _MainWindowTop = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MainWindowTop")); }
                }
            }
        }
        public int SelectedTab
        {
            get { return _SelectedTab; }
            set
            {
                if (_SelectedTab != value)
                {
                    _SelectedTab = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SelectedTab")); }
                }
            }
        }


        public bool InitiativeShowPlayers
        {
            get { return _InitiativeShowPlayers; }
            set
            {
                if (_InitiativeShowPlayers != value)
                {
                    _InitiativeShowPlayers = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeShowPlayers")); }
                }
            }
        }
        public bool InitiativeShowMonsters
        {
            get { return _InitiativeShowMonsters; }
            set
            {
                if (_InitiativeShowMonsters != value)
                {
                    _InitiativeShowMonsters = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeShowMonsters")); }
                }
            }
        }
        public bool InitiativeHideMonsterNames
        {
            get { return _InitiativeHideMonsterNames; }
            set
            {
                if (_InitiativeHideMonsterNames != value)
                {
                    _InitiativeHideMonsterNames = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeHideMonsterNames")); }
                }
            }
        }
        public bool InitiativeHidePlayerNames
        {
            get { return _InitiativeHidePlayerNames; }
            set
            {
                if (_InitiativeHidePlayerNames != value)
                {
                    _InitiativeHidePlayerNames = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeHidePlayerNames")); }
                }
            }
        }
        public bool InitiativeShowConditions
        {
            get { return _InitiativeShowConditions; }
            set
            {
                if (_InitiativeShowConditions != value)
                {
                    _InitiativeShowConditions = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeShowConditions")); }
                }
            }
        }
        public bool CheckForUpdates
        {
            get { return _CheckForUpdates; }
            set
            {
                if (_CheckForUpdates != value)
                {
                    _CheckForUpdates = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CheckForUpdates")); }
                }
            }
        }

        public Character.HPMode DefaultHPMode
        {
            get { return _DefaultHPMode; }
            set
            {
                if (_DefaultHPMode != value)
                {
                    _DefaultHPMode = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DefaultHPMode")); }
                }
            }
        }

        public int InitiativeConditionsSize
        {
            get { return _InitiativeConditionsSize; }
            set
            {
                if (_InitiativeConditionsSize != value)
                {
                    _InitiativeConditionsSize = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeConditionsSize")); }
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeConditionsSizePercent")); }

                }
            }
        }
        public double InitiativeConditionsSizePercent
        {
            get { return .5 + .25 * (double)_InitiativeConditionsSize; }
        }

        public bool InitiativeAlwaysOnTop
        {
            get { return _InitiativeAlwaysOnTop; }
            set
            {
                if (_InitiativeAlwaysOnTop != value)
                {
                    _InitiativeAlwaysOnTop = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeAlwaysOnTop")); }
                }
            }
        }
        public double InitiativeScale
        {
            get { return _InitiativeScale; }
            set
            {
                if (_InitiativeScale != value)
                {
                    _InitiativeScale = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeScale")); }
                }
            }
        }
        public bool InitiativeFlip
        {
            get { return _InitiativeFlip; }
            set
            {
                if (_InitiativeFlip != value)
                {
                    _InitiativeFlip = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("InitiativeFlip")); }
                }
            }
        }

        public int ColorScheme
        {
            get { return _ColorScheme; }
            set
            {
                if (_ColorScheme != value)
                {
                    _ColorScheme = value;
                    if (optionsLoaded)
                    {
                        ColorManager.PrepareCurrentScheme();
                    }

                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ColorScheme")); }

                }
            }
        }

        public bool DarkScheme
        {
            get { return _DarkScheme; }
            set
            {
                if (_DarkScheme != value)
                {
                    _DarkScheme = value;
                    if (optionsLoaded)
                    {
                        ColorManager.PrepareCurrentScheme();
                    }

                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("DarkScheme")); }

                }
            }
        }


        public MonsterSetFilter MonsterDBFilter
        {
            get { return _MonsterDBFilter; }
            set
            {
                if (_MonsterDBFilter != value)
                {
                    _MonsterDBFilter = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MonsterDBFilter")); }
                }
            }
        }
        public MonsterSetFilter MonsterTabFilter
        {
            get { return _MonsterTabFilter; }
            set
            {
                if (_MonsterTabFilter != value)
                {
                    _MonsterTabFilter = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MonsterTabFilter")); }
                }
            }
        }


        public bool RunCombatViewService
        {
            get { return _RunCombatViewService; }
            set
            {
                if (_RunCombatViewService != value)
                {
                    _RunCombatViewService = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RunCombatViewService")); }
                }
            }
        }

        public bool ShowHiddenInitValue
        {
            get { return _ShowHiddenInitValue; }
            set
            {
                if (_ShowHiddenInitValue != value)
                {
                    _ShowHiddenInitValue = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ShowHiddenInitValue")); }
                }
            }
        }

        public bool AddMonstersHidden
        {
            get { return _AddMonstersHidden; }
            set
            {
                if (_AddMonstersHidden != value)
                {
                    _AddMonstersHidden = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("AddMonstersHidden")); }
                }
            }
        }

        public bool StatsOpenByDefault
        {
            get { return _StatsOpenByDefault; }
            set
            {
                if (_StatsOpenByDefault != value)
                {
                    _StatsOpenByDefault = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("StatsOpenByDefault")); }
                }
            }
        }

        public bool RunLocalService
        {
            get { return _RunLocalService; }
            set
            {
                if (_RunLocalService != value)
                {
                    _RunLocalService = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RunLocalService")); }
                }
            }
        }

        public bool RunWebService
        {
            get { return _RunWebService; }
            set
            {
                if (_RunWebService != value)
                {
                    _RunWebService = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RunWebService")); }
                }
            }
        }

        public ushort LocalServicePort
        {
            get { return _LocalServicePort; }
            set
            {
                if (_LocalServicePort != value)
                {
                    _LocalServicePort = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LocalServicePort")); }
                }
            }
        }

        public String LocalServicePasscode
        {
            get { return _LocalServicePasscode; }
            set
            {
                if (_LocalServicePasscode != value)
                {
                    _LocalServicePasscode = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("LocalServicePasscode")); }
                }
            }
        }

        public RulesSystem RulesSystem
        {
            get { return (RulesSystem)_RulesSystem; }
            set
            {
                if (_RulesSystem != (int)value)
                {
                    _RulesSystem = (int)value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RulesSystem"));
                }
            }
        }

        public bool UseTurnClock
        {
            get { return _UseTurnClock; }
            set
            {
                if (_UseTurnClock != value)
                {
                    _UseTurnClock = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("UseTurnClock")); }
                }
            }
        }
        public bool CountdownToNextTurn
        {
            get { return _CountdownToNextTurn; }
            set
            {
                if (_CountdownToNextTurn != value)
                {
                    _CountdownToNextTurn = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CountdownToNextTurn")); }
                }
            }
        }
        public bool MoveAutomaticallyOnTurnTimer
        {
            get { return _MoveAutomaticallyOnTurnTimer; }
            set
            {
                if (_MoveAutomaticallyOnTurnTimer != value)
                {
                    _MoveAutomaticallyOnTurnTimer = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MoveAutomaticallyOnTurnTimer")); }
                }
            }
        }
        public int TurnTimeSeconds
        {
            get { return _TurnTimeSeconds; }
            set
            {
                if (_TurnTimeSeconds != value)
                {
                    _TurnTimeSeconds = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("TurnTimeSeconds")); }
                }
            }
        }
        public int WarningTimeSeconds
        {
            get { return _WarningTimeSeconds; }
            set
            {
                if (_WarningTimeSeconds != value)
                {
                    _WarningTimeSeconds = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("WarningTimeSeconds")); }
                }
            }
        }
        public bool PlayWarningSound
        {
            get { return _PlayWarningSound; }
            set
            {
                if (_PlayWarningSound != value)
                {
                    _PlayWarningSound = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PlayWarningSound")); }
                }
            }
        }
        public string WarningSound
        {
            get { return _WarningSound; }
            set
            {
                if (_WarningSound != value)
                {
                    _WarningSound = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("WarningSound")); }
                }
            }
        }
        public bool PlayTurnEndSound
        {
            get { return _PlayTurnEndSound; }
            set
            {
                if (_PlayTurnEndSound != value)
                {
                    _PlayTurnEndSound = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("PlayTurnEndSound")); }
                }
            }
        }
        public string TurnEndSound
        {
            get { return _TurnEndSound; }
            set
            {
                if (_TurnEndSound != value)
                {
                    _TurnEndSound = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("TurnEndSound")); }
                }
            }
        }

        public bool ShowClockForMonsters
        {
            get { return _ShowClockForMonsters; }
            set
            {
                if (_ShowClockForMonsters != value)
                {
                    _ShowClockForMonsters = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("ShowClockForMonsters")); }
                }
            }
        }





        void LoadOptions()
        {
            try
            {
                RollHP = _preferences.LoadBoolValue("RollHP", false);
                UseCore = _preferences.LoadBoolValue("UseCore", true);
                UseAPG = _preferences.LoadBoolValue("UseAPG", true);
                UseChronicles = _preferences.LoadBoolValue("UseChronicles", true);
                UseModules = _preferences.LoadBoolValue("UseModules", true);
                UseUltimateMagic = _preferences.LoadBoolValue("UseUltimateMagic", true);
                UseUltimateCombat = _preferences.LoadBoolValue("UseUltimateCombat", true);
                UseOther = _preferences.LoadBoolValue("UseOther", true);
                ConfirmCharacterDelete = _preferences.LoadBoolValue("ConfirmCharacterDelete", false);
                ConfirmInitiativeRoll = _preferences.LoadBoolValue("ConfirmInitiativeRoll", false);
                ConfirmClose = _preferences.LoadBoolValue("ConfirmClose", false);
                ShowAllDamageDice = _preferences.LoadBoolValue("ShowAllDamageDice", false);
                PlayerMiniMode = _preferences.LoadBoolValue("PlayerMiniMode", false);
                MonsterMiniMode = _preferences.LoadBoolValue("MonsterMiniMode", false);
                MainWindowWidth = _preferences.LoadIntValue("MainWindowWidth", -1);
                MainWindowHeight = _preferences.LoadIntValue("MainWindowHeight", -1);
                MainWindowLeft = _preferences.LoadIntValue("MainWindowLeft", int.MinValue);
                MainWindowTop = _preferences.LoadIntValue("MainWindowTop", int.MinValue);
                SelectedTab = _preferences.LoadIntValue("SelectedTab", 0);
                AlternateInitRoll = _preferences.LoadStringValue("AlternateInitRoll", "3d6");
                AlternateInit3d6 = _preferences.LoadBoolValue("AlternateInit3d6", false);
                InitiativeShowPlayers = _preferences.LoadBoolValue("InitiativeShowPlayers", true);
                InitiativeShowMonsters = _preferences.LoadBoolValue("InitiativeShowMonsters", true);
                InitiativeHideMonsterNames = _preferences.LoadBoolValue("InitiativeHideMonsterNames", false);
                InitiativeHidePlayerNames = _preferences.LoadBoolValue("InitiativeHidePlayerNames", false);
                InitiativeShowConditions = _preferences.LoadBoolValue("InitiativeShowConditions", false);
                InitiativeConditionsSize = _preferences.LoadIntValue("InitiativeConditionsSize", 2);
                InitiativeAlwaysOnTop = _preferences.LoadBoolValue("InitiativeAlwaysOnTop", false);
                InitiativeScale = _preferences.LoadDoubleValue("InitiativeScale", 1.0);
                InitiativeFlip = _preferences.LoadBoolValue("InitiativeFlip", false);
                RunCombatViewService = _preferences.LoadBoolValue("RunCombatViewService", false);
                ShowHiddenInitValue = _preferences.LoadBoolValue("ShowHiddenInitValue", false);
                AddMonstersHidden = _preferences.LoadBoolValue("AddMonstersHidden", false);
                StatsOpenByDefault = _preferences.LoadBoolValue("StatsOpenByDefault", false);
                CheckForUpdates = _preferences.LoadBoolValue("CheckForUpdates", true);
                DefaultHPMode = (Character.HPMode)_preferences.LoadIntValue("DefaultHPMode", 0);
                MonsterDBFilter = (MonsterSetFilter)_preferences.LoadIntValue("MonsterDBFilter", (int)MonsterSetFilter.Monsters);
                MonsterTabFilter = (MonsterSetFilter)_preferences.LoadIntValue("MonsterTabFilter", (int)MonsterSetFilter.Monsters);
                ColorScheme = _preferences.LoadIntValue("ColorScheme", 0);
                DarkScheme = _preferences.LoadBoolValue("DarkScheme", false);
                RulesSystem = (RulesSystem)_preferences.LoadIntValue("RulesSystem", 0);
                RunLocalService = _preferences.LoadBoolValue("RunLocalService", false);
                RunWebService = _preferences.LoadBoolValue("RunWebService", true);
                LocalServicePort = (ushort)_preferences.LoadIntValue("LocalServicePort", LocalCombatManagerService.DefaultPort);
                LocalServicePasscode = _preferences.LoadStringValue("LocalServicePasscode", "");

                UseTurnClock = _preferences.LoadBoolValue("UseTurnClock", false);
                CountdownToNextTurn = _preferences.LoadBoolValue("CountdownToNextTurn", true);
                MoveAutomaticallyOnTurnTimer = _preferences.LoadBoolValue("MoveAutomaticallyOnTurnTimer", false);
                TurnTimeSeconds = _preferences.LoadIntValue("TurnTimeSeconds", 180);
                WarningTimeSeconds = _preferences.LoadIntValue("WarningTimeSeconds", 10);
                PlayWarningSound = _preferences.LoadBoolValue("PlayWarningSound", true);
                WarningSound = _preferences.LoadStringValue("WarningSound", "");
                PlayTurnEndSound = _preferences.LoadBoolValue("PlayTurnEndSound", true);
                TurnEndSound = _preferences.LoadStringValue("TurnEndSound", "");
                ShowClockForMonsters = _preferences.LoadBoolValue("ShowClockForMonsters", true);

                optionsLoaded = true;
            }
            catch (System.Security.SecurityException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        public void SaveOptions()
        {
            SaveOptions(SettingsSaveSection.All);
        }
        public void SaveOptions(SettingsSaveSection section)
        {

            if (optionsLoaded)
            {
                try
                {

                    var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\CombatManager", RegistryKeyPermissionCheck.Default);


                    if (section == SettingsSaveSection.All)
                    {
                        _preferences.SaveBoolValue("RollHP", RollHP);
                        _preferences.SaveBoolValue("ConfirmCharacterDelete", ConfirmCharacterDelete);
                        _preferences.SaveBoolValue("ConfirmInitiativeRoll", ConfirmInitiativeRoll);
                        _preferences.SaveBoolValue("ConfirmClose", ConfirmClose);
                        _preferences.SaveBoolValue("ShowAllDamageDice", ShowAllDamageDice);
                        _preferences.SaveBoolValue("AlternateInit3d6", AlternateInit3d6);
                        _preferences.SaveStringValue("AlternateInitRoll", AlternateInitRoll);
                        _preferences.SaveBoolValue("RunCombatViewService", RunCombatViewService);
                        _preferences.SaveBoolValue("ShowHiddenInitValue", ShowHiddenInitValue);
                        _preferences.SaveBoolValue("AddMonstersHidden", AddMonstersHidden);
                        _preferences.SaveBoolValue("StatsOpenByDefault", StatsOpenByDefault);
                        _preferences.SaveBoolValue("CheckForUpdates", CheckForUpdates);
                        _preferences.SaveIntValue("ColorScheme", ColorScheme);
                        _preferences.SaveIntValue("DefaultHPMode", (int)DefaultHPMode);


                    }

                    if (section == SettingsSaveSection.System || section == SettingsSaveSection.All)
                    {
                        _preferences.SaveIntValue("RulesSystem", (int)RulesSystem);
                    }

                    if (section == SettingsSaveSection.WindowState || section == SettingsSaveSection.All)
                    {
                        _preferences.SaveBoolValue("PlayerMiniMode", PlayerMiniMode);
                        _preferences.SaveBoolValue("MonsterMiniMode", MonsterMiniMode);
                        _preferences.SaveIntValue("MainWindowWidth", MainWindowWidth);
                        _preferences.SaveIntValue("MainWindowHeight", MainWindowHeight);
                        _preferences.SaveIntValue("MainWindowLeft", MainWindowLeft);
                        _preferences.SaveIntValue("MainWindowTop", MainWindowTop);
                        _preferences.SaveIntValue("SelectedTab", SelectedTab);
                    }

                    if (section == SettingsSaveSection.Sources || section == SettingsSaveSection.All)
                    {

                        _preferences.SaveBoolValue("UseCore", UseCore);
                        _preferences.SaveBoolValue("UseAPG", UseAPG);
                        _preferences.SaveBoolValue("UseChronicles", UseChronicles);
                        _preferences.SaveBoolValue("UseModules", UseModules);
                        _preferences.SaveBoolValue("UseUltimateMagic", UseUltimateMagic);
                        _preferences.SaveBoolValue("UseUltimateCombat", UseUltimateCombat);
                        _preferences.SaveBoolValue("UseOther", UseOther);
                    }

                    if (section == SettingsSaveSection.All || section == SettingsSaveSection.Initiative)
                    {
                        _preferences.SaveBoolValue("InitiativeShowPlayers", InitiativeShowPlayers);
                        _preferences.SaveBoolValue("InitiativeShowMonsters", InitiativeShowMonsters);
                        _preferences.SaveBoolValue("InitiativeHideMonsterNames", InitiativeHideMonsterNames);
                        _preferences.SaveBoolValue("InitiativeHidePlayerNames", InitiativeHidePlayerNames);
                        _preferences.SaveBoolValue("InitiativeShowConditions", InitiativeShowConditions);
                        _preferences.SaveIntValue("InitiativeConditionsSize", InitiativeConditionsSize);
                        _preferences.SaveBoolValue("InitiativeAlwaysOnTop", InitiativeAlwaysOnTop);
                        _preferences.SaveDoubleValue("InitiativeScale", InitiativeScale);
                        _preferences.SaveBoolValue("InitiativeFlip", InitiativeFlip);
                    }
                    if (section == SettingsSaveSection.All || section == SettingsSaveSection.LocalService)
                    {
                        _preferences.SaveBoolValue("RunLocalService", RunLocalService);
                        _preferences.SaveBoolValue("RunWebService", RunWebService);
                        _preferences.SaveIntValue("LocalServicePort", LocalServicePort);
                        _preferences.SaveStringValue("LocalServicePasscode", LocalServicePasscode);

                    }
                    if (section == SettingsSaveSection.All || section == SettingsSaveSection.Filters)
                    {
                        _preferences.SaveIntValue("MonsterDBFilter", (int)MonsterDBFilter);
                        _preferences.SaveIntValue("MonsterTabFilter", (int)MonsterTabFilter);
                    }
                    if (section == SettingsSaveSection.All || section == SettingsSaveSection.Timer)
                    {
                        _preferences.LoadBoolValue("UseTurnClock", UseTurnClock);
                        _preferences.LoadBoolValue("CountdownToNextTurn", CountdownToNextTurn);
                        _preferences.LoadBoolValue("MoveAutomaticallyOnTurnTimer", MoveAutomaticallyOnTurnTimer);
                        _preferences.LoadIntValue("TurnTimeSeconds", TurnTimeSeconds);
                        _preferences.LoadIntValue("WarningTimeSeconds", WarningTimeSeconds);
                        _preferences.LoadBoolValue("PlayWarningSound", PlayWarningSound);
                        _preferences.LoadStringValue("WarningSound", WarningSound);
                        _preferences.LoadBoolValue("PlayTurnEndSound", PlayTurnEndSound);
                        _preferences.LoadStringValue("TurnEndSound", TurnEndSound);
                        _preferences.LoadBoolValue("ShowClockForMonsters", ShowClockForMonsters);
                    }


                }

                catch (System.Security.SecurityException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                catch (System.IO.IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        static HashSet<string> timeSettings = new HashSet<string>
    (){"UseTurnClock",
            "CountdownToNextTurn",
            "MoveAutomaticallyOnTurnTimer",
            "TurnTimeSeconds",
            "WarningTimeSeconds",
            "PlayWarningSound",
            "WarningSound",
            "PlayTurnEndSound",
            "TurnEndSound",
            "ShowClockForMonsters",
        };

        public static bool IsTimerSetting(string name)
        {
            return timeSettings.Contains(name);
        }



        public static bool Loaded
        {
            get
            {
                return (_Settings != null && _Settings.optionsLoaded);
            }
        }




        public static UserSettings Settings
        {
            get
            {
                if (_Settings == null)
                {
                    _Settings = new UserSettings();
                }
                return _Settings;
            }
        }


    }




}
