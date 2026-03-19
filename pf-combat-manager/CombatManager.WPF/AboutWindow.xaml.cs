/*
 *  AboutWindow.xaml.cs
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
 using System.Windows;
 using System.Windows.Documents;
 using System.IO;

namespace CombatManager
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            using (var textStream = Application.GetResourceStream(new Uri("pack://application:,,,/version")).Stream)
            {

                var reader = new StreamReader(textStream);

                var text = reader.ReadToEnd();



                RevisionFlowViewer.Document.Blocks.Add(new Paragraph(new Run(text)));
            }


            using (var textStream = Application.GetResourceStream(new Uri("pack://application:,,,/license.txt")).Stream)
            {

                var reader = new StreamReader(textStream);

                var text = reader.ReadToEnd();



                GPLScrollViewer.Document.Blocks.Add(new Paragraph(new Run(text)));
            }


            using (var textStream = Application.GetResourceStream(new Uri("pack://application:,,,/Supporters.txt")).Stream)
            {

                var reader = new StreamReader(textStream);

                var text = reader.ReadToEnd();



                SupporterFlowViewer.Document.Blocks.Add(new Paragraph(new Run(text)));
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://combatmanager.com");
        }


        private void APIHyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://combatmanager.com/localapi.html");
        }

    }
}
