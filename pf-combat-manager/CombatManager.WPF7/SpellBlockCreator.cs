/*
 *  SpellBlockCreator.cs
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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CombatManager.WPF7
{
    public class SpellBlockCreator : BlockCreator
    {



        public delegate void SpellLinkHander(object sender, DocumentLinkEventArgs e);

        SpellLinkHander _LinkHandler;

        public SpellBlockCreator(FlowDocument document, SpellLinkHander linkHandler)
            : base(document)
        {
            _LinkHandler = linkHandler;
        }

        public List<Block> CreateBlocks(Spell spell)
        {
            return CreateBlocks(spell, false, true);
        }

        public List<Block> CreateBlocks(Spell spell, bool shortForm, bool showTitle)
        {

            var blocks = new List<Block>();

            if (!shortForm)
            {
                if (showTitle)
                {
                    blocks.Add(CreateHeaderParagraph(spell.name));
                }

                var details = new Paragraph
                {
                    Margin = new Thickness(0, 4, 0, 0)
                };
                var span1 = new Span();
                span1.Inlines.Add(new Bold(new Run("School ")));

                if (_LinkHandler == null)
                {
                    span1.Inlines.Add(new Run(spell.School));
                }
                else
                {
                    var link = new Hyperlink(new Run(spell.School));
                    link.Click += new RoutedEventHandler(link_Click);
                    link.DataContext = spell.School;

                    var rule = Rule.Rules.FirstOrDefault
                        (a => String.Compare(a.Name, spell.School, true) == 0 &&
                          String.Compare(a.Type, "Magic") == 0);

                    if (rule != null)
                    {
                        link.Tag = rule;
                        var t = (ToolTip)Application.Current.MainWindow.FindResource("ObjectToolTip");

                        if (t != null)
                        {

                            ToolTipService.SetShowDuration(link, 360000);
                            link.ToolTip = t;
                            link.ToolTipOpening += new ToolTipEventHandler(SpellBlockLink_ToolTipOpening);

                        }
                    }

                    span1.Inlines.Add(link);
                }

                CreateItemIfNotNull(span1.Inlines, " (", false, spell.Subschool, ")", false);
                CreateItemIfNotNull(span1.Inlines, " [", false, spell.Descriptor, "]", false);

                if (showTitle)
                {
                    span1.Inlines.Add("     ");
                }
                else
                {
                    span1.Inlines.Add(new LineBreak());
                }
                span1.Inlines.Add(new Bold(new Run("Level ")));
                span1.Inlines.Add(spell.SpellLevel);
                span1.Inlines.Add(new LineBreak());
                details.Inlines.Add(span1);
                CreateItemIfNotNull(details.Inlines, "Preparation Time ", spell.PreparationTime);
                CreateItemIfNotNull(details.Inlines, "Casting Time ", spell.CastingTime);
                CreateItemIfNotNull(details.Inlines, "Components ", spell.Components);
                if (spell.Range != null && spell.Range.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Range ")));
                    details.Inlines.Add(spell.Range);
                    details.Inlines.Add(new LineBreak());
                }
                if (spell.Area != null && spell.Area.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Area ")));
                    details.Inlines.Add(spell.Area);
                    details.Inlines.Add(new LineBreak());
                }
                if (spell.Targets != null && spell.Targets.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Targets ")));
                    details.Inlines.Add(spell.Targets);
                    details.Inlines.Add(new LineBreak());
                }
                if (spell.Effect != null && spell.Effect.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Effect ")));
                    details.Inlines.Add(spell.Effect);
                    details.Inlines.Add(new LineBreak());
                }

                CreateItemIfNotNull(details.Inlines, "Duration ", spell.Duration);
                if (spell.SavingThrow != null && spell.SavingThrow.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Saving Throw ")));
                    details.Inlines.Add(spell.SavingThrow);
                    details.Inlines.Add(new LineBreak());
                }
                if (spell.SpellResistence != null && spell.SpellResistence.Length > 0)
                {
                    details.Inlines.Add(new Bold(new Run("Spell Resistance ")));
                    details.Inlines.Add(spell.SpellResistence);
                    details.Inlines.Add(new LineBreak());
                }

                if (spell.Source != "PFRPG Core")
                {
                    CreateItemIfNotNull(details.Inlines, "Source ", SourceInfo.GetSource(spell.Source));
                }


                blocks.Add(details);

                var flow = new List<Block>();

                var th =  new Thickness(0, showTitle ? 5 : 0, 0, 0);

                if (spell.DescriptionFormated != null && spell.DescriptionFormated.Length > 0)
                {
                    flow = CreateFlowFromDescription(spell.DescriptionFormated);

                    if (flow.Count > 0)
                    {
                        flow[0].Margin = th;
                    }

                    blocks.AddRange(flow);
                }
                else if (spell.Description != null && spell.Description.Length > 0)
                {
                    var p = new Paragraph
                    {
                        Margin = th
                    };

                    p.Inlines.Add(new Run(spell.Description));
                    blocks.Add(p);

                }
            }
            else
            {
                if (showTitle)
                {
                    var titleParagraph = new Paragraph();
                    titleParagraph.Inlines.Add(new Bold(new Run(spell.name)));
                    titleParagraph.Margin = new Thickness(0);
                    titleParagraph.Padding = new Thickness(0);


                    blocks.Add(titleParagraph);
                }
                var flow = new List<Block>(); ;
                if (spell.DescriptionFormated != null && spell.DescriptionFormated.Length > 0)
                {
                    flow = CreateFlowFromDescription(spell.DescriptionFormated);


                    var block = flow[0];
                    var m = block.Margin;
                    m.Top = 0;
                    block.Margin = m;

                    blocks.AddRange(flow);
                }
                else if (spell.Description != null && spell.Description.Length > 0)
                {
                    var p = new Paragraph();

                    var th = new Thickness(0, showTitle ? 5 : 0, 0, 0);
                    p.Margin = th;

                    p.Inlines.Add(new Run(spell.Description));
                    flow.Add(p);

                }

                foreach (var b in flow)
                {
                    b.TextAlignment = TextAlignment.Left;
                    
                }

                blocks.AddRange(flow);


            }



            return blocks;

        }

        void SpellBlockLink_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var l = (Hyperlink)sender;
            ((ToolTip)l.ToolTip).DataContext = l.Tag;
        }

        void link_Click(object sender, RoutedEventArgs e)
        {
            if (_LinkHandler != null)
            {
                var school = (string)((Hyperlink)sender).DataContext;
                _LinkHandler(this, new DocumentLinkEventArgs(school, "School"));
            }
        }

    }
}
