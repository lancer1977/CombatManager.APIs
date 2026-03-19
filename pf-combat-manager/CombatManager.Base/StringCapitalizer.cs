/*
 *  StringCapitalizer.cs
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
    public class StringCapitalizer
    {

        public static SortedDictionary<string, string> IgnoreWords;

        static StringCapitalizer()
        {
            IgnoreWords = new SortedDictionary<string, string>(new InsensitiveComparer());

            string[] ignore = { "the", "of", "from", "to", "and" };

            foreach (string str in ignore)
            {
                IgnoreWords[str] = str;
            }
        }




        public static string Capitalize(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {

                Regex regWord = new Regex("\\w+('s)?");

                text = regWord.Replace(text, delegate(Match m)
                {
                    string x = m.Value;

                    if (!IgnoreWords.ContainsKey(x))
                    {

                        x = x.Substring(0, 1).ToUpper() + x.Substring(1);
                    }

                    return x;
                });
            }

            return text;
        }
    }
}
