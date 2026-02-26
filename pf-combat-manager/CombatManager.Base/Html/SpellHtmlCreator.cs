/*
 *  SpellHtmlCreator.cs
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

using System.Text;

namespace CombatManager.Html
{
	public class SpellHtmlCreator
	{
		public SpellHtmlCreator ()
		{
		}

        public static string CreateHtml(Spell spell, bool shortForm = false,
            bool showTitle = true, bool completepage = true,  string css = null)
        {

            StringBuilder blocks = new StringBuilder();
            if (completepage)
            {
                blocks.CreateHtmlHeader(css);
            }


            if (!shortForm)
            {
                if (showTitle)
                {
                    blocks.CreateHeader(spell.name);
                }

               	
                blocks.CreateItemIfNotNull("School ", true, spell.School, null, false);


                blocks.CreateItemIfNotNull(" (", false, spell.Subschool, ")", false);
                blocks.CreateItemIfNotNull(" [", false, spell.Descriptor, "]", false);

                blocks.AppendLineBreak();
				
				blocks.CreateItemIfNotNull("Level ", spell.SpellLevel);
                blocks.CreateItemIfNotNull("Preparation Time ", spell.PreparationTime);
                blocks.CreateItemIfNotNull("Casting Time ", spell.CastingTime);
                blocks.CreateItemIfNotNull("Components ", spell.Components);
                blocks.CreateItemIfNotNull("Range ", spell.Range);
                blocks.CreateItemIfNotNull("Area ", spell.Area);
                blocks.CreateItemIfNotNull("Targets ", spell.Targets);
                blocks.CreateItemIfNotNull("Effects ", spell.Effect);
				
                blocks.CreateItemIfNotNull("Duration ", spell.Duration);
                blocks.CreateItemIfNotNull("Saving Throw ", spell.SavingThrow);

                blocks.CreateItemIfNotNull("Spell Resistance ", spell.SpellResistence);

                if (spell.Source != "PFRPG Core")
                {
                    blocks.CreateItemIfNotNull("Source ", SourceInfo.GetSource(spell.Source));
                }

                if (spell.DescriptionFormated != null && spell.DescriptionFormated.Length > 0)
                {
                    blocks.Append(spell.DescriptionFormated);
                }
                else if (spell.Description != null && spell.Description.Length > 0)
                {
                    blocks.AppendHtml(spell.Description);
                }
            }
            else
            {
                if (showTitle)
                {
                   blocks.AppendEscapedTag("p", spell.name, "bolded");
                }

                if (spell.DescriptionFormated != null && spell.DescriptionFormated.Length > 0)
                {
                    blocks.Append(spell.DescriptionFormated);
                }
                else if (spell.Description != null && spell.Description.Length > 0)
                {
                    blocks.AppendHtml(spell.Description);
                }


            }

            if (completepage)
            {
                blocks.CreateHtmlFooter();
            }


            return blocks.ToString();

        }

    }
}

