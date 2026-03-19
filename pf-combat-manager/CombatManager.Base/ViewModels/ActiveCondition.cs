/*
 *  ActiveCondition.cs
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

namespace CombatManager.ViewModels
{
    public class ActiveCondition : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Condition _condition;
        private int? _turns;
        private int? _endTurn;
        private int? _lastAppliedRound;
        private string _details;
        private InitiativeCount _initiativeCount;

        public ActiveCondition()
        {

        }


        public ActiveCondition(ActiveCondition a)
        {
            if (a._condition != null)
            {
                _condition = (Condition)a._condition.Clone();
            }
            _turns = a._turns;
            _endTurn = a._endTurn;
            _lastAppliedRound = a._lastAppliedRound;
            if (a._initiativeCount != null)
            {
                _initiativeCount = (InitiativeCount)a._initiativeCount.Clone();
            }
            _details = a._details;
        }

        public object Clone()
        {
            return new ActiveCondition(this);
        }

        public Condition Condition
        {
            get => _condition;
            set
            {
                if (_condition != value)
                {
                    _condition = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Condition"));
                }
            }
        }

        public int? Turns
        {
            get => _turns;
            set
            {
                if (_turns != value)
                {
                    _turns = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Turns"));
                }
            }
        }

        [XmlIgnore]
        public int? EndTurn
        {
            get => _endTurn;
            set
            {
                if (_endTurn != value)
                {
                    _endTurn = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Turns"));
                }
            }
        }


        public InitiativeCount InitiativeCount
        {
            get => _initiativeCount;
            set
            {
                if (_initiativeCount != value)
                {
                    _initiativeCount = new InitiativeCount((InitiativeCount)value) ;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InitiativeCount"));
                }
            }
        }

        /// <summary>
        /// Used to prevent a condition from being applied twice when rounds go back and fo
        /// </summary>
        public int? LastAppliedRound
        {
            get => _lastAppliedRound;
            set
            {
                if (_lastAppliedRound != value)
                {
                    _lastAppliedRound = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LastAppliedRound"));
                }
            }
        }

        [XmlIgnore]
        public ConditionBonus Bonus
        {
            get
            {
                if (Condition != null)
                {
                    if (Condition.Spell != null)
                    {
                        return Condition.Spell.Bonus;
                    }

                    return Condition.Bonus;
                }
                return null;

            }
        }

        public string Details
        {
            get => _details;
            set
            {
                if (_details != value)
                {
                    _details = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Details"));
                }
            }
        }

    }
}
