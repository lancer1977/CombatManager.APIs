/*
 *  DieRoll.cs
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

using System.Runtime.Serialization;
using CombatManager.Utilities;

namespace CombatManager
{
    [DataContract]
    public class RollResult
    {
        public RollResult()
        {
            Rolls = new List<DieResult>();
        }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public List<DieResult> Rolls { get; set; }

        [DataMember]
        public int Mod { get; set; }

        public static RollResult operator+ (RollResult a, RollResult r)
        {
            RollResult nr = new RollResult();
            nr.Total = a.Total + r.Total;
            nr.Rolls.AddRange(a.Rolls);
            nr.Rolls.AddRange(r.Rolls);
            nr.Mod = a.Mod + r.Mod;

            return nr;
        }

    }


    [DataContract]
    public class DieResult
    {
        [DataMember]
        public int Die { get; set; }

        [DataMember]
        public int Result { get; set; }
    }


    [DataContract]
    public class DieRoll : ICloneable
    {
        private int _count;
        private int _fraction;
        private int _die;
        private int _mod;
        private List<DieStep> _extraRolls;




        private static Random _rand = new Random();
        public static Random Rand => _rand;

        private class DieStepComparer : IEqualityComparer<DieStep>
        {
            public bool Equals(DieStep a, DieStep b)
            {
                return (a.Count == b.Count && a.Die == b.Die);
            }

            public int GetHashCode(DieStep a)
            {
                return a.Count << 16 | a.Die;
            }
        }

        private static Dictionary<DieStep, DieStep> _stepUpList;
        private static Dictionary<DieStep, DieStep> _stepDownList;

        static DieRoll()
        {
            //create steps
            _stepUpList = new Dictionary<DieStep, DieStep>(new DieStepComparer());
            _stepDownList = new Dictionary<DieStep, DieStep>(new DieStepComparer());

            _stepUpList.Add(new DieStep(0, 1), new DieStep(1, 1));
            _stepUpList.Add(new DieStep(1, 1), new DieStep(1, 2));
            _stepUpList.Add(new DieStep(1, 2), new DieStep(1, 3));
            _stepUpList.Add(new DieStep(1, 3), new DieStep(1, 4));
            _stepUpList.Add(new DieStep(1, 4), new DieStep(1, 6));
            _stepUpList.Add(new DieStep(1, 6), new DieStep(1, 8));
            _stepUpList.Add(new DieStep(1, 8), new DieStep(2, 6));
            _stepUpList.Add(new DieStep(2, 6), new DieStep(2, 8));
            _stepUpList.Add(new DieStep(2, 8), new DieStep(4, 6));
            _stepUpList.Add(new DieStep(4, 6), new DieStep(4, 8));

            foreach (KeyValuePair<DieStep, DieStep> step in _stepUpList)
            {
                _stepDownList.Add(step.Value, step.Key);
            }


            //other
            _stepUpList.Add(new DieStep(2, 4), new DieStep(2, 6));
            _stepDownList.Add(new DieStep(2, 4), new DieStep(1, 6));

            _stepUpList.Add(new DieStep(1, 10), new DieStep(2, 8));
            _stepDownList.Add(new DieStep(1, 10), new DieStep(1, 8));

            _stepUpList.Add(new DieStep(1, 12), new DieStep(3, 6));
            _stepDownList.Add(new DieStep(1, 12), new DieStep(1, 10));

            _stepUpList.Add(new DieStep(3, 6), new DieStep(5, 6));
            _stepDownList.Add(new DieStep(3, 6), new DieStep(1, 12));
        }

        public DieRoll()
        {
            Fraction = 1;
        }

        public DieRoll(int count, int die, int mod)
        {
            this.Count = count;
            this.Die = die;
            this.Mod = mod;
            Fraction = 1;
        }
        public DieRoll(int count, int fraction, int die, int mod)
        {
            this.Count = count;
            this.Die = die;
            this.Mod = mod;
            this.Fraction = fraction;
        }

        public DieRoll(DieRoll old)
        {
            CopyFrom(old);
        }

        private void CopyFrom(DieRoll old)
        {

            if (old == null)
            {
                Count = 0;
                Die = 0;
                Mod = 0;
            }
            else
            {
                Count = old.Count;
                Fraction = old.Fraction;
                Die = old.Die;
                Mod = old.Mod;


                if (old.ExtraRolls != null)
                {
                    ExtraRolls = new List<DieStep>();

                    foreach (DieStep step in old.ExtraRolls)
                    {
                        ExtraRolls.Add(new DieStep(old.Count, old.Die));
                    }
                }
                else
                {
                    ExtraRolls = null;
                }
            }
        }
		
		public override string ToString ()
		{
			return Text;	
		}

        public override int GetHashCode()
        {
            int val = (Die << 12) ^ (Count << 8) ^ (Fraction << 4) ^ Mod;

            if (ExtraRolls != null && ExtraRolls.Count > 0)
            {
                int extra = 0;

                foreach (DieStep step in ExtraRolls)
                {
                    extra ^= step.GetHashCode();
                }

                val |= (extra << 16);
            }

            return val;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(DieRoll))
            {
                return false;
            }
            DieRoll roll = (DieRoll)obj;

            if (roll.ExtraRolls == null ^ ExtraRolls == null)
            {
                return false;
            }

            if (ExtraRolls != null)
            {
                if (roll.ExtraRolls.Count != ExtraRolls.Count)
                {
                    return false;
                }

                for (int i = 0; i < ExtraRolls.Count; i++)
                {
                    if (roll.ExtraRolls[i] != ExtraRolls[i])
                    {
                        return false;
                    }
                }
            }

            return (roll.Count == Count && roll.Die == Die && roll.Fraction == Fraction && roll.Mod == Mod);
        }



        public static bool operator ==(DieRoll s1, DieRoll s2)
        {
            if ((((object)s1) == null) ^ (((object)s2) == null))
            {
                return false;
            }
            if (((object)s1) == null)
            {
                return true;
            }

            return s1.Equals(s2);
        }
        
        public static bool operator !=(DieRoll s1, DieRoll s2)
        {


            return !(s1 == s2);
        }

        public object Clone()
        {
            return new DieRoll(this);
        }

        [XmlIgnore]
        public DieStep Step
        {
            get => new(this);
            set
            {
                this.Count = value.Count;
                this.Die = value.Die;
            }
        }

        [XmlIgnore]
        public int TotalCount
        {
            get
            {
                int total = Count;

                if (ExtraRolls != null)
                {
                    foreach (DieStep step in ExtraRolls)
                    {
                        total += step.Count;
                    }
                }

                return total;
            }
        }

        [DataMember]
        public string Text
        {
            get
            {
                string text = "";


                text += Count;

                if (Fraction > 1)
                {
                    text += "/" + Fraction;
                }

                text += "d" +Die;

                if (ExtraRolls != null && ExtraRolls.Count > 0)
                {
                    foreach (DieStep step in ExtraRolls)
                    {
                        text += "+" + step.Count + "d" + step.Die;
                    }
                }


                if (Mod != 0)
                {
                    text += Mod.PlusFormat();
                }


                return text;
            }
            set => CopyFrom(DieRoll.FromString(value));
        }

        public static DieRoll StepDie(DieRoll roll, int diff)
        {
            DieRoll outRoll = new DieRoll(roll);
            if (diff > 0)
            {
                for (int i = 0; i < diff; i++)
                {
                    outRoll = StepUpDieRoll(outRoll);
                }
            }
            if (diff < 0)
            {
                for (int i = 0; i > diff; i--)
                {
                    outRoll = StepDownDieRoll(outRoll);
                }
            }

            return outRoll;
        }

        public static DieStep StepDie(DieStep step, int diff)
        {
            DieRoll r = new DieRoll(step.Count, step.Die, 0);
            return StepDie(r, diff).Step;

        }


        private static DieRoll StepUpDieRoll(DieRoll roll)
        {
            DieRoll outRoll = roll;

            DieStep step = new DieStep(roll.Count, roll.Die);

            if (_stepUpList.ContainsKey(step))
            {
                step = _stepUpList[step];
            }
            else
            {
                if (step.Count < 2)
                {
                    step.Count += 1;
                }
                else
                {
                    step.Count += 2;
                }
            }

            outRoll.Count = step.Count;
            outRoll.Die = step.Die;

            return outRoll;

        }

        private static DieRoll StepDownDieRoll(DieRoll roll)
        {
            DieRoll outRoll = roll;

            DieStep step = new DieStep(roll.Count, roll.Die);

            if (_stepDownList.ContainsKey(step))
            {
                step = _stepDownList[step];
            }
            else
            {
                if (step.Count > 3)
                {
                    step.Count -= 3;
                }
                else if (step.Count > 1)
                {
                    step.Count -= 1;
                }
                else if (step.Die > 1)
                {
                    {
                        step.Die -= 1;
                    }
                }
                else
                {
                    step.Count = 0;
                    step.Die = 1;
                }
            }

            outRoll.Count = step.Count;
            outRoll.Die = step.Die;

            return outRoll;

        }

        public static DieRoll FromString(string text)
        {
            return FromString(text, 0);
        }

        private const string DieRollRegexString = "([0-9]+)(/[0-9]+)?d([0-9]+)(?<extra>(\\+([0-9]+)d([0-9]+))*)((\\+|-)[0-9]+)?";


        public static DieRoll FromString(string text, int start)
        {
            DieRoll roll = null;
            if (text != null)
            {
                try
                {

                    Regex regRoll = new Regex(DieRollRegexString);

                    Match match = regRoll.Match(text, start);

                    if (match.Success)
                    {
                        roll = new DieRoll();

                        roll.Count = int.Parse(match.Groups[1].Value);


                        if (match.Groups[2].Success)
                        {
                            roll.Fraction = int.Parse(match.Groups[2].Value.Substring(1));
                        }
                        else
                        {
                            roll.Fraction = 1;
                        }

                        roll.Die = int.Parse(match.Groups[3].Value);


                        if (roll.Die == 0)
                        {
                            throw new FormatException("Invalid Die Roll");
                        }

                        if (match.Groups["extra"].Success)
                        {
                            roll.ExtraRolls = new List<DieStep>();

                            Regex extraReg = new Regex("([0-9]+)d([0-9]+)");

                            foreach (Match d in extraReg.Matches(match.Groups["extra"].Value))
                            {
                                DieStep step = new DieStep();
                                step.Count = int.Parse(d.Groups[1].Value);
                                step.Die = int.Parse(d.Groups[2].Value);


                                if (step.Die == 0)
                                {
                                    throw new FormatException("Invalid Die Roll");
                                }

                                roll.ExtraRolls.Add(step);
                            }
                        }

                        if (match.Groups[7].Success)
                        {
                            roll.Mod = int.Parse(match.Groups[7].Value);
                        }
                    }
                }
                catch(FormatException)
                {
                    roll = null;
                }
                catch (OverflowException)
                {
                    roll = null;
                }
            }

            return roll;
        }

        public int AverageRoll()
        {
            //return ((die + 1) * count) / 2 + mod;
            return (int) (AllRolls.Sum(roll => (Math.Floor((decimal) ((roll.Die + 1)*roll.Count))/2))+Mod);
            
        }

        public int HighestDie()
        {
            int high = 0;
            foreach (DieStep step in AllRolls)
            {
                high = Math.Max(step.Die, high);
            }
            return high;
        }


        [XmlIgnore]
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                }
            }
        }


        [XmlIgnore]
        public int Fraction
        {
            get => _fraction;
            set
            {
                if (_fraction != value)
                {
                    _fraction = value;
                }
            }
        }


        [XmlIgnore]
        public int Die
        {
            get => _die;
            set
            {
                if (_die != value)
                {
                    _die = value;
                }
            }
        }


        [XmlIgnore]
        public int Mod
        {
            get => _mod;
            set
            {
                if (_mod != value)
                {
                    _mod = value;
                }
            }
        }


        [XmlIgnore]
        public List<DieStep> ExtraRolls
        {
            get => _extraRolls;
            set
            {
                if (_extraRolls != value)
                {
                    _extraRolls = value;
                }
            }
        }

        [XmlIgnore]
        public List<DieStep> AllRolls
        {
            get
            {
                List<DieStep> steps = new List<DieStep>() { Step };

                if (_extraRolls != null)
                {
                    foreach (DieStep step in _extraRolls)
                    {
                        steps.Add(step);
                    }

                }
                return steps;

            }
			set
			{
				System.Diagnostics.Debug.Assert(value.Count > 0);
				
				Count = value[0].Count;
				Die = value[0].Die;
				
				_extraRolls = new List<DieStep>();
				for (int i=1; i<value.Count; i++)
				{
					_extraRolls.Add(value[i]);
				}
			}
        }

        public RollResult Roll()
        {
            RollResult roll = new RollResult();
            roll.Mod = Mod;
            roll.Total = Mod;


            foreach (DieStep step in AllRolls)
            {
                for (int i = 0; i < step.Count; i++)
                {
                    DieResult res = new DieResult();
                    res.Die = step.Die;
                    res.Result += _rand.Next(1, step.Die + 1);
                    roll.Total += res.Result;
                    roll.Rolls.Add(res);
                }
            }

            return roll;
           
        }

        public int Max
        {
            get
            {
                int total = this._mod;

                foreach (DieStep step in AllRolls)
                {
                    total += step.Die * step.Count;
                }
                return total; 
            }
        }

        public void AddDie(int newdie)
        {
            AddDie(new DieStep(1, newdie));
        }

        public void AddDie(DieStep step)
        {
            if (this.Die == step.Die)
            {
                this.Count += step.Count;
            }
            else
            {
                bool added = false;
                if (_extraRolls != null)
                {
                    foreach (DieStep ex in _extraRolls)
                    {
                        if (ex.Die == step.Die)
                        {
                            ex.Count += step.Count;
                            added = true;
                            break;
                        }
                    }
                }

                if (!added)
                {
                    if (_extraRolls == null)
                    {
                        _extraRolls = new List<DieStep>();
                    }
                    _extraRolls.Add(new DieStep(step.Count, step.Die));
                }
            }
        }

        public void RemoveDie(DieStep step)
        {
            if (this.Die == step.Die)
            {
                this.Count -= step.Count;
                if (this.Count < 0)
                {
                    this.Count = 0;
                }
            }
            else
            {
                if (_extraRolls != null)
                {
                    for (int i=0; i<_extraRolls.Count; i++)
                    {
                        DieStep ex = ExtraRolls[i];
                        if (ex.Die == step.Die)
                        {
                            ex.Count -= step.Count;
                            if (ex.Count <= 0)
                            {
                                _extraRolls.RemoveAt(i);
                            }
                            break;
                        }
                    }
                }
            }
        }
        
        public int DieCount(int die)
        {
            int count = 0;

            foreach (DieStep d in this.AllRolls)
            {
                if (d.Die == die)
                {
                    count += d.Count;
                }
            }
            return count;
        }

    }

    
    [DataContract]
    public class DieStep : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public DieStep() {}
        public DieStep(int count, int die) { this.Count = count; this.Die = die; }
        public DieStep(DieRoll roll) { this.Count = roll.Count; this.Die = roll.Die; }

        private int _count;
        private int _die;



        [XmlIgnore]
        public string Text => Count + "d" + Die;

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(DieStep))
            {
                return false;
            }

            DieStep step = (DieStep)obj;

            return (Count == step.Count && Die == step.Die);
        }

        public static bool operator ==(DieStep s1, DieStep s2)
        {
            if (((object)s1) == null ^ ((object)s2) == null)
            {
                return false;
            }
            if (((object)s1) == null)
            {
                return true;
            }

            return s1.Equals(s2);
        }

        public static bool operator !=(DieStep s1, DieStep s2)
        {
            return !(s1 == s2);
        }

        public override int GetHashCode()
        {
            return (Count << 8) | (Die);
        }

        public int Roll()
        {

            int val = 0;

            if (Die == 1)
            {
                val = Die * Count;
            }
            else if (Die > 1)
            {
                for (int i = 0; i < Count; i++)
                {
                    val += DieRoll.Rand.Next(1, Die + 1);
                }
            }

            return val;

        }

        public double RollDouble(bool min1)
        {
            double val = 0;

            if (Die == 1 && min1)
            {
                val = Die * Count;
            }
            else if (Die > 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    double d = DieRoll.Rand.NextDouble();

                    if (min1)
                    {
                        val += 1.0 + ((double)(Count - 1)) * d;
                    }
                    else
                    {
                        val += ((double)Count) * d;
                    }

                }
            }

            return val;
        }


        [DataMember]
        public int Count
        {
            get => _count;
            set
            {
                if (_count != value)
                {
                    _count = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Count")); }
                }
            }
        }

        [DataMember]
        public int Die
        {
            get => _die;
            set
            {
                if (_die != value)
                {
                    _die = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Die")); }
                }
            }
        }


    }
}
