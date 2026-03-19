/*
 *  SpellBlockInfo.cs
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

using CombatManager.Utilities;

namespace CombatManager
{
    public class SpellBlockInfo : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;


        private string _class;
        private int? _meleeTouch;
        private int? _rangedTouch;
        private int? _concentration;
        private int? _casterLevel;
        private bool _spellLikeAbilities;
        private string _blockType;
        private int? _spellFailure;

        private ObservableCollection<SpellLevelInfo> _levels = new ObservableCollection<SpellLevelInfo>();

        public SpellBlockInfo() { }
        public SpellBlockInfo(SpellBlockInfo old)
        {
            _class = old._class;
            _meleeTouch = old._meleeTouch;
            _rangedTouch = old._rangedTouch;
            _concentration = old.Concentration;
            _casterLevel = old._casterLevel;
            _spellLikeAbilities = old._spellLikeAbilities;
            _blockType = old._blockType;
            _spellFailure = old.SpellFailure;
            foreach (SpellLevelInfo info in old._levels)
            {
                _levels.Add(new SpellLevelInfo(info));
            }
        }



        public object Clone()
        {
            return new SpellBlockInfo(this);
        }

        static string _classRegexString;
        static SpellBlockInfo()
        {
            
            bool first = true;
            _classRegexString = "(";
            foreach (Rule rule in Rule.Rules.Where(a => (a.Type == "Classes"||a.Type =="Races")))
            {
                //remove plural from race name - this is for SLA {Dwarven may be incorrect} may need fixed if dwarves ever get a racial SLA
                if (rule.Type == "Races")
                {
                    rule.Name = rule.Name.TrimEnd('s');
                }
                if (!first)
                {
                    _classRegexString += "|";
                }
                first = false;
                _classRegexString += rule.Name;
            }
            //Wizard Specialist "as class names"
            _classRegexString += "|Abjurer|Conjurer|Diviner|Enchanter|Evoker|Illusionist|Necromancer|Transmuter";
            // special SLA names
            _classRegexString += "|Domain" + "|Bloodline" + "|Arcane School";
            
            _classRegexString += ")";
               
        }

        private static string _spellcountblock = "((?<spellcount>[0-9]+(?=[;,\\)])),?)";
        private static string _dcblock = "(DC (?<DC>[0-9]+),?)";
        private static string _castblock = "((; )?((?<spellcast>[0-9]+|already)) cast)";
        private static string _otherblock = "((; )?(?<othertext>[\\p{Pd}+',/\\p{L}0-9:\\. #%\\*\\[\\]]+))";
        private static string _countdcblock = "((" + _spellcountblock + "|" + _dcblock + "|" + _castblock + "|" + _otherblock + ") *)+";


        private static string _spellblock = "((?<spellname>[\\p{Pd}'\\p{L}/ ]+)([*)])? *(?<superscript>\\[[MS]\\])? *(?<countdc>\\(" + _countdcblock + "\\))?,?)";
        private static string _levelblock = " *((?<level>([0-9]))(st|nd|rd|th)?) *(\\((((?<daily>[0-9]+)/day(; (?<levelcast>[0-9]+) cast)?|at will))\\))?-?(?<spellblocks>" + _spellblock + "+) *((?<more>[0-9]+) more)? *";


        public static ObservableCollection<SpellBlockInfo> ParseInfo(string spellBlock)
        {
            ObservableCollection<SpellBlockInfo> blocklist = new ObservableCollection<SpellBlockInfo>();


            Regex group = new Regex("((" + _classRegexString + ") )?(?<sla>Spell-Like Abilities)|((" + _classRegexString + ") )?Spells (?<blocktype>(Known|Prepared))");

            List<string> list = new List<string>();
            List<string> slablock = new List<string>();
            foreach (Match spellMatch in group.Matches(spellBlock))
            {
                int start = spellMatch.Index;
                int length;
                Match next = spellMatch.NextMatch();

                if (next.Success)
                {
                    length = next.Index - start;
                }
                else
                {
                    length = spellBlock.Length - start;
                }

                string text = spellBlock.Substring(start, length);
                if (spellMatch.Groups["sla"].Success)
                {
                    slablock.Add(text);
                }
                else
                {
                    list.Add(text);
                }

            }

            foreach (string spells in list)
            {


                Regex regSpell = new Regex(_spellblock);
                Regex regSpells = new Regex("((?<classname>" + _classRegexString + " )?(?<sla>Spell-Like Abilities)|(?<classname>" + _classRegexString + " )?(Spells (?<blocktype>(Known|Prepared)) +))(\\(CL ((?<cl>([0-9]+))(st|nd|rd|th)?)(?<altcl> \\[[0-9]+(st|nd|rd|th)? [A-Za-z]+\\.\\])?([,;] *concentration *[\\p{Pd}+]?(?<concentration>[0-9]+)( \\[[\\p{Pd}+][0-9]+ [\\p{L}. ]+\\])?)?([,;] *[\\p{Pd}+]?(?<spellfailure>[0-9]+)% spell failure)?([,;] *[\\p{Pd}+]?(?<meleetouch>[0-9]+) melee touch)?([,;] *[\\p{Pd}+]?(?<rangedtouch>[0-9]+) ranged touch)?\\))[:\r\n]*" +
                    "(?<levelblocks>" + _levelblock + "\r?\n?)+");
                Regex regLevel = new Regex(_levelblock);


                foreach (Match m in regSpells.Matches(spells))
                {
                    SpellBlockInfo blockInfo = new SpellBlockInfo();
                        blockInfo.ParseBlockHeader(m);
                        

                    foreach (Capture cap in m.Groups["levelblocks"].Captures)
                    {
                            
                        Match levelMatch = regLevel.Match(cap.Value);

                        SpellLevelInfo levelInfo = new SpellLevelInfo();

                        levelInfo.Level = levelMatch.IntValue("level");

                        levelInfo.Cast = levelMatch.IntValue("levelcast");

                        if (levelMatch.Groups["daily"].Success)
                            {
                                if (string.Compare(levelMatch.Groups["daily"].Value.Trim(), "At Will", true) == 0)
                                {
                                    levelInfo.AtWill = true;
                                }
                                else if (string.Compare(levelMatch.Groups["daily"].Value.Trim(), "Constant", true) == 0)
                                {
                                    levelInfo.Constant = true;
                                }
                                else
                                {

                                    levelInfo.PerDay = int.Parse(levelMatch.Groups["daily"].Value);
                                }
                            }

                        levelInfo.More = levelMatch.IntValue("more");

                        SpellInfo prevInfo = null;
                            foreach (Match spell in regSpell.Matches(levelMatch.Groups["spellblocks"].Value))
                            {

                                SpellInfo spellInfo = ParseSpell(spell, prevInfo);

                                if (spellInfo != prevInfo && spellInfo.Name.Length > 0)
                                {
                                    levelInfo.Spells.Add(spellInfo);
                                }

                                prevInfo = spellInfo;
                            }

                        if (levelInfo.Spells.Count > 0)
                            {
                                blockInfo.Levels.Add(levelInfo);
                            }          
                    }

                    if (blockInfo.Levels.Count > 0)
                    {
                        blocklist.Add(blockInfo);
                    }

                }
            }

            foreach (string slatext in slablock)
            {
                ParseSlaBlocks(blocklist, slatext);
            }

            return blocklist;
        }

        private void ParseBlockHeader( Match m)
        {
            SpellBlockInfo blockInfo = this;
            if (m.Groups["classname"].Success)
            {
                blockInfo.Class = m.Groups["classname"].Value.Trim();
            }

            if (m.Groups["sla"].Success)
            {
                blockInfo._spellLikeAbilities = true;
            }

            else if (m.Groups["blocktype"].Success)
            {
                blockInfo.BlockType = m.Groups["blocktype"].Value.Trim();
            }


            blockInfo.Concentration = m.IntValue("concentration");
            blockInfo.MeleeTouch = m.IntValue("meleetouch");
            blockInfo.RangedTouch = m.IntValue("rangedtouch");
            blockInfo.CasterLevel = m.IntValue("cl");
            blockInfo.SpellFailure = m.IntValue("spellfailure");

            
        }


        private static string _sladcblock = "((" + _dcblock + "|" + _otherblock + ") *)+";
        private static string _slaspellblock = "((?<spellname>[\\p{Pd}'\\p{L}/ ]+)\\*? *(?<countdc>\\(" + _sladcblock + "\\))?,?)";
        private static string _slaheader = "(((?<daily>[0-9]+)/day)|(?<constant>[Cc]onstant)|(?<atwill>At [W|w]ill))-";
        private static string _slablock = " *" + _slaheader + "(?<spellblocks>" + _slaspellblock + "+) *";



        private static void ParseSlaBlocks(ObservableCollection<SpellBlockInfo> info, string text)
        {
            //DebugTimer t = new DebugTimer("SLA Blocks", false, false);

            Regex regSpell = new Regex(_slaspellblock);
            Regex regSpells = new Regex("(?<classname>" + _classRegexString + " )?(?<sla>Spell-Like Abilities) *\\(CL ((?<cl>[0-9]+)(st|nd|rd|th)?)(?<altcl> \\[[0-9]+(st|nd|rd|th)? [A-Za-z]+\\.\\])?([,;] *concentration *[\\p{Pd}+]?(?<concentration>[0-9]+)( \\[[\\p{Pd}+][0-9]+ [\\p{L} ]+\\])?)?([,;] *[\\p{Pd}+]?(?<spellfailure>[0-9]+)% spell failure)?([,;] *[\\p{Pd}+]?(?<meleetouch>[0-9]+) melee touch)?([,;] *[\\p{Pd}+]?(?<rangedtouch>[0-9]+) ranged touch)?\\)[:\r\n]*");
            Regex regLevel = new Regex(_slablock);

          
            

            Match m = regSpells.Match(text);


            //t.MarkEventIf("First", 20);

            if (m.Success)
            {
                SpellBlockInfo blockInfo = new SpellBlockInfo();
                blockInfo.ParseBlockHeader(m);

                Regex regSlaHeader = new Regex(_slaheader, RegexOptions.IgnoreCase);
                string spellBlock = text;
                List<string> spellblockList = new List<string>();
                MatchCollection mc = regSlaHeader.Matches(spellBlock);


                for (int i = 0; i < mc.Count; i++ )
                {
                    Match spellMatch = mc[i];
                    int start = spellMatch.Index;
                    int length;
                    Match next = null;

                    if (i + 1 < mc.Count)
                    {
                        next = mc[i + 1];
                    }

                    if (next != null)
                    {
                        length = next.Index - start;
                    }
                    else
                    {
                        length = spellBlock.Length - start;
                    }


                    string btext = spellBlock.Substring(start, length);
                    spellblockList.Add(btext);


                }

                //t.MarkEventIf("SpellBlocklist", 20);


                foreach (string block in spellblockList)
                {
                    //t.MarkEventIf("levmatch s", 100);
                    Match levelMatch = regLevel.Match(block);
                    //t.MarkEventIf("levmatch: " + block, 10);

                    SpellLevelInfo levelInfo = new SpellLevelInfo();
                  

                    if (levelMatch.Groups["daily"].Success)
                    {
                        if (string.Compare(levelMatch.Groups["daily"].Value.Trim(), "At Will", true) == 0)
                        {
                            levelInfo.AtWill = true;
                        }
                        else if (string.Compare(levelMatch.Groups["daily"].Value.Trim(), "Constant", true) == 0)
                        {
                            levelInfo.Constant = true;
                        }
                        else
                        {

                            levelInfo.PerDay = int.Parse(levelMatch.Groups["daily"].Value);
                        }
                    }
                    else if (levelMatch.Groups["atwill"].Success)
                    {
                        levelInfo.AtWill = true;
                    }
                    else if (levelMatch.Groups["constant"].Success)
                    {
                        levelInfo.Constant = true;
                    }

                    levelInfo.More = levelMatch.IntValue("more");

                    SpellInfo prevInfo = null;
                    Match spell = regSpell.Match(levelMatch.Groups["spellblocks"].Value);
                    while (spell.Success)
                    {

                        SpellInfo spellInfo = ParseSpell(spell, prevInfo);

                        if (spellInfo != prevInfo && spellInfo.Name.Length > 0)
                        {
                            levelInfo.Spells.Add(spellInfo);
                        }

                        prevInfo = spellInfo;
                        spell = spell.NextMatch();
                    }

                    if (levelInfo.Spells.Count > 0)
                    {
                        blockInfo.Levels.Add(levelInfo);
                    }

                }

                if (blockInfo.Levels.Count > 0)
                {
                    info.Add(blockInfo);
                }
            }

            //t.MarkEventIfTotal("Long: " + text , 10);
        }

        private static SpellInfo ParseSpell(Match spell, SpellInfo prevInfo)
        {
           
            string spellname = spell.Groups["spellname"].Value.TrimEnd(new char[] { 'D', ' ' });
            spellname = StringCapitalizer.Capitalize(spellname).Trim();

            SpellInfo spellInfo = null;
            if ((string.Compare(spellname, "lesser", true) == 0 ||
                string.Compare(spellname, "greater", true) == 0) && prevInfo != null)
            {
                spellInfo = prevInfo;
                spellname = spellInfo.Name + ", " + spellname;
            }
            else
            {

                spellInfo = new SpellInfo();
            }

            spellInfo.Count = spell.IntValue("spellcount");

            spellInfo.Name = spellname;

            spellInfo.Spell = Spell.ByName(spellname);

            spellInfo.Dc = spell.IntValue("DC");

            if (spell.Groups["spellcast"].Success)
            {
                if (string.Compare(spell.Groups["spellcast"].Value, "already", true) == 0)
                {
                    spellInfo.AlreadyCast = true;
                }
                else
                {
                    spellInfo.Cast = spell.IntValue("spellcast");
                }
            }

            if (spell.Groups["othertext"].Success)
            {
                spellInfo.Other = spell.Groups["othertext"].Value;
            }

            if (spell.Groups["onlytext"].Success)
            {
                spellInfo.Only = spell.Groups["onlytext"].Value;
            }

            
            if (spell.Groups["superscript"].Success)
            {
                string superscript = spell.Groups["superscript"].Value;
                if (superscript.Contains("M"))
                {
                    spellInfo.Mythic = true;
                }
            }

            return spellInfo;
        }


        public string Class
        {
            get => _class;
            set
            {
                if (_class != value)
                {
                    _class = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Class")); }
                }
            }
        }
        public int? MeleeTouch
        {
            get => _meleeTouch;
            set
            {
                if (_meleeTouch != value)
                {
                    _meleeTouch = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("MeleeTouch")); }
                }
            }
        }
        public int? RangedTouch
        {
            get => _rangedTouch;
            set
            {
                if (_rangedTouch != value)
                {
                    _rangedTouch = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("RangedTouch")); }
                }
            }
        }
        public int? Concentration
        {
            get => _concentration;
            set
            {
                if (_concentration != value)
                {
                    _concentration = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Concentration")); }
                }
            }
        }
        public int? CasterLevel
        {
            get => _casterLevel;
            set
            {
                if (_casterLevel != value)
                {
                    _casterLevel = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("CasterLevel")); }
                }
            }
        }

        public string BlockType
        {
            get => _blockType;
            set
            {
                if (_blockType != value)
                {
                    _blockType = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("BlockType")); }
                }
            }
        }


        public ObservableCollection<SpellLevelInfo> Levels
        {
            get => _levels;
            set
            {
                if (_levels != value)
                {
                    _levels = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("Levels")); }
                }
            }
        }


        public int? SpellFailure
        {
            get => _spellFailure;
            set
            {
                if (_spellFailure != value)
                {
                    _spellFailure = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SpellFailure")); }
                }
            }
        }


        public bool SpellLikeAbilities
        {
            get => _spellLikeAbilities;
            set
            {
                if (_spellLikeAbilities != value)
                {
                    _spellLikeAbilities = value;
                    if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs("SpellLikeAbilities")); }
                }
            }
        }


    }
}
