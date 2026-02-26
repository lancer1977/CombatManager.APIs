/*
 *  CharacterAttacks.cs
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

namespace CombatManager
{



    public class CharacterAttacks : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;



        private ObservableCollection<AttackSet> _meleeAttacks;
        private ObservableCollection<ViewModels.Attack> _rangedAttacks;

        private List<List<WeaponItem>> _meleeWeaponSets;
        private ObservableCollection<WeaponItem> _rangedWeapons;
        private ObservableCollection<WeaponItem> _naturalAttacks;

        private int _hands;

        
        public CharacterAttacks()
        {
            _meleeAttacks = new ObservableCollection<AttackSet>();


        }

        public CharacterAttacks(ObservableCollection<AttackSet> melee, ObservableCollection<ViewModels.Attack> ranged)
        {

            SetupAttacks(melee, ranged);
        }


        public CharacterAttacks(Monster stats)
        {
            ObservableCollection<AttackSet> melee = new ObservableCollection<AttackSet>(stats.MeleeAttacks) ;
            ObservableCollection<ViewModels.Attack> ranged = new ObservableCollection<ViewModels.Attack>(stats.RangedAttacks);

            SetupAttacks(melee, ranged);
        }

        public void SetupAttacks(ObservableCollection<AttackSet> melee, ObservableCollection<ViewModels.Attack> ranged)
        {
            MeleeAttacks = new ObservableCollection<AttackSet>();

            foreach (AttackSet set in melee)
            {
                MeleeAttacks.Add((AttackSet)set.Clone());
            }

            RangedAttacks = new ObservableCollection<ViewModels.Attack>();
            foreach (ViewModels.Attack attack in ranged)
            {
                RangedAttacks.Add((ViewModels.Attack)attack.Clone());
            }

            //find melee weapon sets
            MeleeWeaponSets = new List<List<WeaponItem>>();

            NaturalAttacks = new ObservableCollection<WeaponItem>();

            _hands = 2;
            
            foreach (AttackSet set in MeleeAttacks)
            {
                List<WeaponItem> weapons = new List<WeaponItem>();

                bool main = true;
                //find melee attacks
                foreach (ViewModels.Attack attack in set.WeaponAttacks)
                {
                    if (attack.Weapon != null)
                    {
                        WeaponItem item = new WeaponItem(attack);

                        int count = item.Count;
                        item.Count = 1;
                        for (int i=0; i<count; i++)
                        {
                            WeaponItem newItem = (WeaponItem)item.Clone();

                            if (main)
                            {
                                newItem.MainHand = true;
                                main = false;
                            }

                            weapons.Add(newItem);
                        }
                    }

                    _hands = Math.Max(_hands, set.Hands);
                }
                

                if (weapons.Count > 0)
                {
                    MeleeWeaponSets.Add(weapons);
                }

                ObservableCollection<WeaponItem> newAttacks = new ObservableCollection<WeaponItem>();

                //find natural attacks
                foreach (ViewModels.Attack attack in set.NaturalAttacks)
                {
                    if (attack.Weapon != null)
                    {                                                   
                        WeaponItem item = new WeaponItem(attack);

                        newAttacks.Add(item);                        
                    }
                }
                if (newAttacks.Count >= NaturalAttacks.Count)
                {
                    NaturalAttacks = newAttacks;
                }

            }


            //find ranged weapons
            RangedWeapons = new ObservableCollection<WeaponItem>();

            foreach (ViewModels.Attack attack in RangedAttacks)
            {
                if (attack.Weapon != null)
                {
                    WeaponItem item = new WeaponItem(attack);
                    RangedWeapons.Add(item);
                }
            }

        }

        public ObservableCollection<AttackSet> MeleeAttacks
        {
            get => _meleeAttacks;
            set
            {
                if (_meleeAttacks != value)
                {
                    _meleeAttacks = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MeleeAttacks")); }
                }
            }
        }
        public ObservableCollection<ViewModels.Attack> RangedAttacks
        {
            get => _rangedAttacks;
            set
            {
                if (_rangedAttacks != value)
                {
                    _rangedAttacks = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedAttacks")); }
                }
            }
        }
        public List<List<WeaponItem>> MeleeWeaponSets
        {
            get => _meleeWeaponSets;
            set
            {
                if (_meleeWeaponSets != value)
                {
                    _meleeWeaponSets = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MeleeWeaponSets")); }
                }
            }
        }
        public ObservableCollection<WeaponItem> RangedWeapons
        {
            get => _rangedWeapons;
            set
            {
                if (_rangedWeapons != value)
                {
                    _rangedWeapons = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedWeapons")); }
                }
            }
        }
        public ObservableCollection<WeaponItem> NaturalAttacks
        {
            get => _naturalAttacks;
            set
            {
                if (_naturalAttacks != value)
                {
                    _naturalAttacks = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("NaturalAttacks")); }
                }
            }
        }
        public int Hands
        {
            get => _hands;
            set
            {
                if (_hands != value)
                {
                    _hands = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Hands")); }
                }
            }
        }

    }
}
