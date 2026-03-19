/*
 *  MonsterHtmlCreator.cs
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
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.create
 *
 */

using System.Text;
using CombatManager.Utilities;
using CombatManager.ViewModels;

namespace CombatManager.Html
{
	public class MonsterHtmlCreator
	{
		public static string CreateHtml(Monster monster = null, Character ch = null, bool addDescription = true, bool completePage = true, string css = null)
		{
            Monster useMonster;

            if (ch != null)
            {
                useMonster = ch.Monster;
            }
            else
            {
                useMonster = monster;
            }

			StringBuilder blocks = new StringBuilder();
            if (completePage)
            {
                blocks.CreateHtmlHeader(css);
            }

            CreateTopSection(useMonster, ch, blocks);

            CreateDefenseSection(useMonster, blocks);

            CreateOffenseSection(useMonster, blocks);
            CreateTacticsSection(useMonster, blocks);
            CreateStatisticsSection(useMonster, blocks);
            CreateEcologySection(useMonster, blocks);
            CreateSpecialAbilitiesSection(useMonster, blocks);
            if (addDescription)
            {
                CreateDescriptionSection(useMonster, blocks);
            }

           

            if (SourceInfo.GetSourceType(useMonster.Source) != SourceType.Core)
            {
                
                blocks.CreateItemIfNotNull("Source ", SourceInfo.GetSource(useMonster.Source));
                
            }


            if (completePage)
            {
                blocks.CreateHtmlFooter();
            }
		
			return blocks.ToString();
		}


        public enum CombatVisibility
        {
            Visible = 0,
            Anonymous = 1,
            Hidden = 2
        }


        delegate CombatVisibility CombatVisFilter(Character ch, CombatState c);

        static CombatVisibility DefaultCombatFilter(Character ch, CombatState c)
        {
            return CombatListFilter(ch, c);
        }

        static CombatVisibility CombatListFilter(Character ch, CombatState c, CombatVisibility players = CombatVisibility.Visible, CombatVisibility monsters = CombatVisibility.Visible,
            CombatVisibility idle = CombatVisibility.Visible, CombatVisibility hidden = CombatVisibility.Anonymous)
        {
            CombatVisibility vis = ch.IsMonster?monsters:players;
            if (ch.IsHidden)
            {
                vis = vis.Combine(hidden);
            }
            if (ch.IsIdle)
            {
                vis = vis.Combine(idle);
            }
            return vis;
        }

        static List<(CombatVisibility, Character)> FilterCombatList(CombatState c, CombatVisFilter filter, CombatVisibility maxlevel = CombatVisibility.Anonymous)
        {
            var list = new List<(CombatVisibility, Character)>();
         
            foreach (Character ch in c.CombatList)
            {
                var vis = filter(ch, c);
                if (vis <= maxlevel)
                {
                    list.Add(( vis, ch));
                }
            }

            return list;
        }


        public static string CreateCombatListItem(Character ch, CombatState state = null, List<(CombatVisibility, Character)> filteredList = null, int index = 0, CombatVisibility visibility = CombatVisibility.Visible)
        {
            StringBuilder blocks = new StringBuilder();

            string id = (index % 2 == 0) ? "listitem1" : "listitem2";

            bool follower = ch.InitiativeLeader != null;

            var styles = new List<(string, string)>();
            if (filteredList != null && index == filteredList.Count - 1)
            {
                styles.Add(HtmlBlockCreator.StyleNoBottom);
            }


            blocks.StartParagraph(cl: "combatlistitem", id: id);

            if (state != null && ch == state.CurrentCharacter)
            {
                blocks.AppendWebIcon("next");
            }
            else
            {
                blocks.AppendTag("span", content: " ", classname: "square16");
            }

            string name;
            string hp;
            string maxhp;
            string init;

            if (visibility.IsAnonymous())
            {
                name = "??????";
                hp = "??";
                maxhp = "??";
                init = "??";
            }
            else
            {
                name = ch.Name;
                hp = ch.Hp.ToString();
                maxhp = ch.MaxHp.ToString();
                init = ch.InitiativeCount.Base.ToString();
            }

            blocks.AppendOpenTag("span", "combatlistplayer");
            StringBuilder subbuilder = new StringBuilder();
            subbuilder.AppendOpenTag("span", "playername");
            if (follower)
            {
                subbuilder.AppendWebIcon("lock");
                subbuilder.AppendSpace();
            }
            subbuilder.AppendHtml(name);
            subbuilder.AppendCloseTag("span");
            blocks.AppendSpanWithTooltip(content: subbuilder.ToString(), escaped: false, tiptext: name);
            blocks.AppendSpan(" HP: " + hp + "/" + maxhp + " ", classname: "playerhp");

            blocks.AppendSpan("Init: " + init, classname: "playerinitiative");
            if (visibility.IsVisible())
            {
                foreach (ActiveCondition cn in ch.Monster.ActiveConditions)
                {
                    blocks.AppendWebIcon(cn.Condition.Image, cl: "conditionicon");
                }
            }
            blocks.AppendCloseTag("span");



            blocks.EndParagraph();



            return blocks.ToString();
        }


        public static string CreateCombatList(CombatState state)
        {
            StringBuilder blocks = new StringBuilder();
            blocks.AppendOpenTag("div", cl:"combatlist");

            blocks.CreateHeader("Combat List");


            int index = 0;

            var filteredList = FilterCombatList(state, DefaultCombatFilter);


            foreach (var pair in filteredList)
            {
                CombatVisibility vis = pair.Item1;
                Character c = pair.Item2;
                BorderStyle bs = new BorderStyle();
                if (index != filteredList.Count - 1)
                {
                    bs.SetSideValue(BoxStyle.BoxSide.Bottom, 0);
                }
                   
                    

                blocks.AppendOpenTag("div", htmlStyles: new []{bs});

                    
                blocks.Append(CreateCombatListItem(c, state, filteredList, index, visibility: vis));

                foreach (Character f in c.InitiativeFollowers)
                {

                    blocks.Append(CreateCombatListItem(f, visibility:vis));
                }

                blocks.AppendCloseTag("div");

                index++;
            }
            blocks.AppendCloseTag("div");
            return blocks.ToString();
        }


        private static void CreateTopSection(Monster monster, Character ch, StringBuilder blocks)
        {
            string name = monster.Name;

            if ((name == null || name.Length == 0) && ch != null)
            {
                name = ch.Name;
            }

            string header = "CR " + monster.Cr;
            if (monster.Mr != null && monster.Mr > 0 && monster.IsMythic)
            {
                header += "/MR " + monster.Mr;
            }

            blocks.CreateHeader(name, header);
			
			
			blocks.AppendOpenTag("p");
			blocks.AppendTag("b", "XP: " + monster.Xp);
			blocks.AppendOpenTag ("br");
            if (monster.Race.NotNullString () && monster.Class.NotNullString ())
            {
				blocks.AppendHtml (monster.Race + " " + monster.Class);
				blocks.AppendOpenTag ("br");
        	}
			
			blocks.AppendHtml (monster.Alignment + " " + monster.Size).AppendSpace ();
            blocks.AppendLineBreak ();
			
			blocks.CreateItemIfNotNull(null, false, monster.Type, " ", false);
            blocks.CreateItemIfNotNull(null, false, monster.SubType, null, false);
            blocks.AppendLineBreak ();
            blocks.CreateItemIfNotNull("Init ", true, monster.Init.PlusFormat(), "; ", false);
            blocks.CreateItemIfNotNull("Senses ", true, monster.Senses, null, false);
            if (monster.Aura != null && monster.Aura.Length > 0)
            {
            	blocks.AppendLineBreak ();
                blocks.CreateItemIfNotNull("Aura ", true, monster.Aura, null, false);
            }

            blocks.AppendLineBreak ();

			blocks.AppendCloseTag ("p");
        }
		
		private static void CreateDefenseSection(Monster monster, StringBuilder blocks)
        {
            blocks.CreateSectionHeader("DEFENSE");

            
			blocks.AppendOpenTag("p");
            blocks.CreateItemIfNotNull("AC ", true, monster.FullAc.ToString(), ", ", false);
            blocks.CreateItemIfNotNull("touch ", false, monster.TouchAc.ToString(), ", ", false);
            blocks.CreateItemIfNotNull("flat-footed ", false, monster.FlatFootedAc.ToString(), null, false);
            blocks.CreateItemIfNotNull(" ", false, monster.AcMods, null, false);
            blocks.AppendLineBreak();
            blocks.CreateItemIfNotNull("HP ", true, monster.Hp.ToString(), null, false);
            if (monster.Hd != "1d1")
            {
                blocks.CreateItemIfNotNull(" ", false, monster.Hd, null, false);
            }
            blocks.CreateItemIfNotNull("; ", false, monster.HpMods, null, false);
            blocks.AppendLineBreak();
            blocks.CreateItemIfNotNull("Fort ", true, monster.Fort.PlusFormat(), ", ", false);
            blocks.CreateItemIfNotNull("Ref ", true, monster.Ref.PlusFormat(), ", ", false);
            blocks.CreateItemIfNotNull("Will ", true, monster.Will.PlusFormat(), null, false);
            blocks.CreateItemIfNotNull("; ", true, monster.SaveMods, null, false);
            blocks.AppendLineBreak();
            List<TitleValuePair> defLine = new List<TitleValuePair>();
            defLine.Add(new TitleValuePair { Title = "Defensive Abilities ", Value = monster.DefensiveAbilities });
            defLine.Add(new TitleValuePair { Title = "DR ", Value = monster.Dr });
            defLine.Add(new TitleValuePair { Title = "Immune ", Value = monster.Immune });
            defLine.Add(new TitleValuePair { Title = "SR ", Value = monster.Sr });
            defLine.Add(new TitleValuePair { Title = "Resist ", Value = monster.Resist });
            defLine.Add(new TitleValuePair { Title = "Weaknesses ", Value = monster.Weaknesses });

            blocks.CreateMultiValueLine(defLine, "; ");


            blocks.AppendCloseTag("p");
        }

        private static void CreateOffenseSection(Monster monster, StringBuilder blocks)
        {
            blocks.CreateSectionHeader("OFFENSE");
			
			blocks.AppendOpenTag("p");
            blocks.CreateItemIfNotNull("Speed ", monster.Speed);
            blocks.CreateItemIfNotNull("Melee ", monster.Melee);
            blocks.CreateItemIfNotNull("Ranged ", monster.Ranged);


            List<TitleValuePair> reachLine = new List<TitleValuePair>();
            reachLine.Add(new TitleValuePair { Title = "Space ", Value = monster.Space });
            reachLine.Add(new TitleValuePair { Title = "Reach ", Value = monster.Reach });
            blocks.CreateMultiValueLine(reachLine, ", ");

            
            blocks.CreateItemIfNotNull("Special Attacks ", monster.SpecialAttacks);
            
            string spellLike = monster.SpellLikeAbilities;
            if (spellLike != null && spellLike.Length > 0)
            {

                if (spellLike.IndexOf("Spell-Like Abilities ") == 0)
                {
                    spellLike = spellLike.Substring("Spell-Like Abilities ".Length);
                }

                blocks.CreateItemIfNotNull("Spell-Like Abilities ", spellLike);
                

            }
            string spellsKnown = monster.SpellsKnown;
            if (spellsKnown != null && spellsKnown.Length > 0)
            {
                if (spellsKnown.IndexOf("Spells Known ") == 0)
                {
                    spellsKnown = spellsKnown.Substring("Spells Known ".Length);

                }
                blocks.CreateItemIfNotNull("Spells Known ", spellsKnown);
                
            }
            string spellsPrepared = monster.SpellsPrepared;
            if (spellsPrepared != null && spellsPrepared.Length > 0)
            {
                if (spellsPrepared.IndexOf("Spells Prepared ") == 0)
                {
                    spellsPrepared = spellsPrepared.Substring("Spells Prepared ".Length);

                }
                blocks.CreateItemIfNotNull("Spells Prepared ", spellsPrepared);
            
            }

			
			blocks.AppendCloseTag("p");
        }

		 
        private static void CreateTacticsSection(Monster monster, StringBuilder blocks)
        {
            if (monster.BeforeCombat.NotNullString() ||
                monster.DuringCombat.NotNullString() ||
                monster.Morale.NotNullString())
            {

                blocks.CreateSectionHeader("TACTICS");

                blocks.AppendOpenTag("p");


                blocks.CreateItemIfNotNull("Before Combat ", monster.BeforeCombat);
                blocks.CreateItemIfNotNull("During Combat ", monster.DuringCombat);
                blocks.CreateItemIfNotNull("Morale ", monster.Morale);

                blocks.AppendCloseTag("p");


            }
        }

        private static void CreateStatisticsSection(Monster monster, StringBuilder blocks)
        {
            blocks.CreateSectionHeader("STATISTICS");

			
            blocks.AppendOpenTag("p");

            List<TitleValuePair> statsLine = new List<TitleValuePair>();
            statsLine.Add(new TitleValuePair
            {
                Title = "Str ",
                Value = (monster.Strength == null) ? "-" : monster.Strength.ToString()
            });
            statsLine.Add(new TitleValuePair
            {
                Title = "Dex ",
                Value = (monster.Dexterity == null) ? "-" : monster.Dexterity.ToString()
            });
            statsLine.Add(new TitleValuePair
            {
                Title = "Con ",
                Value = (monster.Constitution == null) ? "-" : monster.Constitution.ToString()
            });
            statsLine.Add(new TitleValuePair
            {
                Title = "Int ",
                Value = (monster.Intelligence == null) ? "-" : monster.Intelligence.ToString()
            });
            statsLine.Add(new TitleValuePair
            {
                Title = "Wis ",
                Value = (monster.Wisdom == null) ? "-" : monster.Wisdom.ToString()
            });
            statsLine.Add(new TitleValuePair
            {
                Title = "Cha ",
                Value = (monster.Charisma == null) ? "-" : monster.Charisma.ToString()
            });

            blocks.CreateMultiValueLine(statsLine, ", ");

            List<TitleValuePair> combatLine = new List<TitleValuePair>();
            combatLine.Add(new TitleValuePair { Title = "Base Atk ", Value = monster.BaseAtk.PlusFormat() });
            combatLine.Add(new TitleValuePair { Title = "CMB ", Value = monster.Cmb });
            combatLine.Add(new TitleValuePair { Title = "CMD ", Value = monster.Cmd });
            blocks.CreateMultiValueLine(combatLine, "; ");


            if (monster.FeatsList.Count > 0)
            {

                int count = 0;


                blocks.AppendEscapedTag("sp", "Feats", "bolded");
				blocks.AppendSpace();
				string text = "";
                foreach (string feat in monster.FeatsList)
                {
                    if (count > 0)
                    {
                        text += ", ";
                    }

                    text += feat;

                    count++;
                }
                blocks.AppendHtml(text);
				blocks.AppendLineBreak();

            }

            if (monster.SkillValueDictionary.Count > 0)
            {
                string skillList = "";

                int count = 0;

                foreach (SkillValue val in monster.SkillValueDictionary.Values)
                {
                    if (count > 0)
                    {
                        skillList += ", ";
                    }

                    skillList += val.Text;
                    count++;
                }

                blocks.CreateItemIfNotNull("Skills ", skillList);
            }

            blocks.CreateItemIfNotNull("  Racial Modifiers ", monster.RacialMods);
            blocks.CreateItemIfNotNull("Languages ", monster.Languages);
            blocks.CreateItemIfNotNull("SQ ", monster.Sq);
            blocks.CreateItemIfNotNull("Gear ", monster.Gear);


            
            blocks.AppendCloseTag("p");
        }

        private static void CreateEcologySection(Monster monster, StringBuilder  blocks)
        {
            if ((monster.Environment != null && monster.Environment.Length > 0) ||
                (monster.Organization != null && monster.Organization.Length > 0) ||
                (monster.Treasure != null && monster.Treasure.Length > 0))
            {

                blocks.CreateSectionHeader("ECOLOGY");

				
				blocks.AppendOpenTag("p");
				
				blocks.CreateItemIfNotNull("Environment ", monster.Environment);
				blocks.CreateItemIfNotNull("Organization ", monster.Organization);
				blocks.CreateItemIfNotNull("Treasure ", monster.Treasure);
				
				
				blocks.AppendCloseTag("p");

            }
        }

        private static void CreateSpecialAbilitiesSection(Monster monster, StringBuilder  blocks)
        {
            try
            {
                if (monster.SpecialAbilitiesList.Count > 0)
                {

                    blocks.CreateSectionHeader("SPECIAL ABILITIES");

                    blocks.AppendOpenTag("p");


                    foreach (SpecialAbility spec in monster.SpecialAbilitiesList)
                    {
                        if (spec.Name != null && spec.Name.Length > 0)
                        {

                            string type = spec.Type;

                            if (spec.ConstructionPoints != null)
                            {
                                type += ", " + spec.ConstructionPoints + " CP";
                            }

                            blocks.CreateItemIfNotNull(spec.Name + " (" + type + ") "
                                , spec.Text);
                        }
                        else
                        {

                            blocks.CreateItemIfNotNull(null, spec.Text);
                        }

                    }

                    blocks.AppendCloseTag("p");


                } 
            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine(ex.ToString());
            }
        }

        private static void CreateDescriptionSection(Monster monster, StringBuilder  blocks)
        {

            if (monster.DescriptionVisual != null && monster.DescriptionVisual.Length > 0)
            {
				
				blocks.AppendOpenTag("p", "visualdescription");
				
                blocks.AppendHtml(monster.DescriptionVisual);
				
				blocks.AppendCloseTag("p");

            }



            if (monster.DescHtml != null && monster.DescHtml.Length > 0)
            {
				
				blocks.AppendOpenTag("p", "description");
                blocks.Append(monster.DescHtml);
				blocks.AppendCloseTag("p");

            }
            else if (monster.Description != null && monster.Description.Length > 0)
            {
				
				blocks.AppendOpenTag("span", "description");

                blocks.AppendHtml(monster.Description);


				blocks.AppendCloseTag("span");

            }
			
        }

	}

    static class CombatVisibilityHelper
    {

        public static MonsterHtmlCreator.CombatVisibility Combine(this MonsterHtmlCreator.CombatVisibility a, MonsterHtmlCreator.CombatVisibility b)
        {
            return (a > b) ? a : b;
        }


        public static bool IsVisible(this MonsterHtmlCreator.CombatVisibility vis)
        {
            return vis == MonsterHtmlCreator.CombatVisibility.Visible;
        }

        public static bool IsAnonymous(this MonsterHtmlCreator.CombatVisibility vis)
        {
            return vis == MonsterHtmlCreator.CombatVisibility.Anonymous;
        }

        public static bool IsHidden(this MonsterHtmlCreator.
            CombatVisibility vis)
        {
            return vis == MonsterHtmlCreator.CombatVisibility.Hidden;
        }

    }
}

