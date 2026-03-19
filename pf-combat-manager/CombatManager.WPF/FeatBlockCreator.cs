/*
 *  FeatBlockCreator.cs
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

using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace CombatManager
{
    public class FeatBlockCreator : BlockCreator
    {
        public FeatBlockCreator(FlowDocument document)
            : base(document)
        {
        }


        public List<Block> CreateBlocks(Feat feat)
        {
            return CreateBlocks(feat, true);
        }

        public List<Block> CreateBlocks(Feat feat, bool showTitle)
        {
            var blocks = new List<Block>();

            string featString = null;
            if (feat.Types != null)
            {
                featString = "";
                var first = true;
                foreach (var featType in feat.Types)
                {
                    if (feat.Type != "General")
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            featString += ", ";
                        }

                        featString += featType;
                    }
                }
            }

            var details = new Paragraph();
            details.Margin = new Thickness(0, 2, 0, 0);
            if (showTitle)
            {
                var titleRun = new Run(feat.Name);
                titleRun.FontStyle = FontStyles.Italic;
                titleRun.FontWeight = FontWeights.Bold;
                titleRun.FontSize = titleRun.FontSize * 1.8;

                var text = feat.Name;
                

                if (featString != null && featString.Length > 0)
                {

                    text += " (" + featString + ")";
                }
                


                blocks.Add(CreateHeaderParagraph(text));
            }
            else
            {
                if (featString != null && featString.Length > 0)
                {
                    details.Inlines.Add(new Italic(new Run(featString)));
                    details.Inlines.Add(new LineBreak());
                }
            }



            details.Inlines.AddRange(CreateItemIfNotNull(null, feat.Summary));
            if (feat.Prerequistites != null)
            {
                details.Inlines.AddRange(CreateItemIfNotNull("Prerequisitites: ", feat.Prerequistites.Trim(new char[] { '-' })));
            }
            details.Inlines.AddRange(CreateItemIfNotNull("Benefit: ", feat.Benefit));
            details.Inlines.AddRange(CreateItemIfNotNull("Normal: ", feat.Normal));
            details.Inlines.AddRange(CreateItemIfNotNull("Special: ", feat.Special));

            if (feat.Source != "Pathfinder Core Rulebook")
            {
                CreateItemIfNotNull(details.Inlines, "Source: ", SourceInfo.GetSource(feat.Source));
            }


            blocks.Add(details);



            return blocks;

        }
    }
}
