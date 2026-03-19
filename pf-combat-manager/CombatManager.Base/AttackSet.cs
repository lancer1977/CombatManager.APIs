/*
 *  AttackSet.cs
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
    public class AttackSet
    {
        List<ViewModels.Attack> _weaponAttacks;
        List<ViewModels.Attack> _naturalAttacks;
		
		string _name;
        public AttackSet()
        {
            _weaponAttacks = new List<ViewModels.Attack>();
            _naturalAttacks = new List<ViewModels.Attack>();
        }
        public List<ViewModels.Attack> WeaponAttacks
        {
            get => _weaponAttacks;
            set => _weaponAttacks = value;
        }
        public List<ViewModels.Attack> NaturalAttacks
        {
            get => _naturalAttacks;
            set => _naturalAttacks = value;
        }		
		public string Name
        {
            get => _name;
            set => _name = value;
        }
        public int Hands
        {
            get
            {
                int hands = 0;

                foreach (ViewModels.Attack attack in WeaponAttacks)
                {
                    if (attack.Weapon == null)
                    {
                        hands += attack.Count;
                    }
                    else
                    {
                        hands += attack.Weapon.HandsUsed * attack.Count;

                    }
                }

                foreach (ViewModels.Attack attack in NaturalAttacks)
                {
                    if (attack.Weapon != null)
                    {
                        hands += attack.Weapon.HandsUsed * attack.Count;
                    }
                }

                return Math.Max(hands, 0);
            }
        }
        public object Clone()
        {
            AttackSet set = new AttackSet();

            set.NaturalAttacks = new List<ViewModels.Attack>();

            foreach (ViewModels.Attack attack in NaturalAttacks)
            {
                set.NaturalAttacks.Add((ViewModels.Attack)attack.Clone());
            }
            
            set.WeaponAttacks = new List<ViewModels.Attack>();

            foreach (ViewModels.Attack attack in WeaponAttacks)
            {
                set.WeaponAttacks.Add((ViewModels.Attack)attack.Clone());
            }

            return set;
        }
        public override string ToString()
        {
            string text = "";
            bool firstAttack = true;
            List<ViewModels.Attack> attacks = new List<ViewModels.Attack>();
            attacks.AddRange(_weaponAttacks);
            attacks.AddRange(_naturalAttacks);

            foreach (ViewModels.Attack atk in attacks)
            {
                if (firstAttack)
                {
                    firstAttack = false;
                }
                else
                {
                    text += ", ";
                }

                text += atk.Text;
            }

            return text;
        }

    }
}
