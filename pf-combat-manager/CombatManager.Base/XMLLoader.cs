/*
 *  XMLLoader.cs
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

using System.Diagnostics;
using System.Xml;
 using System.IO;
#if ANDROID
using Android.Content;
#endif

namespace CombatManager
{

    public static class XmlListLoader<T>
    {
        public static List<T> Load(string filename)
        {
            return XmlLoader<List<T>>.Load(filename);
        }

        public static List<T> Load(string filename, bool appData)
        {
            return XmlLoader<List<T>>.Load(filename, appData);
        }

        public static void Save(List<T> list, string filename, bool appData)
        {
            XmlLoader<List<T>>.Save(list, filename, appData);
        }

        public static void Save(List<T> list, string filename)
        {
            XmlLoader<List<T>>.Save(list, filename);
        }
    }

    public static class XmlLoader<T>
    {

        private static Dictionary<string, string> _xmlAttributeErrors;
        private static string _lastFile;

        public const string AppDataSubDir = "Combat Manager";

        static string _assemblyDir;
        static string _appDataDir;

        static XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public static string AssemblyDir
        {
            get
            {
                if (_assemblyDir == null)
                {
                    System.Diagnostics.Debug.WriteLine("AssemblyDir");
                    _assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);


                    #if MONO
                    int loc = _assemblyDir.IndexOf("/.monotouch");
                    if (loc > 0) 
                    {
                        _assemblyDir = _assemblyDir.Substring(0, loc);
                    }

                    #endif
                }
                Debug.WriteLine(_assemblyDir);
                return _assemblyDir;
            }
        }

        public static string AppDataDir
        {
            get
            {
                if (_appDataDir == null)
                {
                    System.Diagnostics.Debug.WriteLine("AppDataDir");
                    #if ANDROID
                    _AppDataDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    #else
                    _appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    #endif
                    _appDataDir = Path.Combine(_appDataDir, AppDataSubDir);

                    if (!Directory.Exists(_appDataDir))
                    {
                        Directory.CreateDirectory(_appDataDir);
                    }

                }
                return _appDataDir;
            }
        }
        

        public static T Load(string filename, bool appData = false)
        {
            T set = default(T);

            _lastFile = filename;
			
#if MONO
			DateTime startTime = DateTime.Now;
			DebugLogger.WriteLine("Loading [" + filename + "]");
#endif

            try
            {
                // Open document
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                serializer.UnknownNode += serializer_UnknownNode;
                serializer.UnknownAttribute += serializer_UnknownAttribute;

#if ANDROID
                string path = filename;
                Stream io;
                StreamReader fs;
                if (!appData)
                {
                    if (path.StartsWith("/"))
                    {
                        io = File.Open(path, FileMode.Open);
                        fs = new StreamReader(io);

                    }
                    else
                    {
                      
                        io = CoreContext.Context.Assets.Open(path);
                        fs = new StreamReader(io);
                    }
                    
                }
                else
                {
                    path = Path.Combine(AppDataDir, filename);
                    io = File.Open(path, FileMode.Open);
                    fs = new StreamReader(io);

                }
                using(io)
                {
                    using (fs)
                    {
                    
#else

                string file = SaveFileName(filename, appData);

                if (new FileInfo(file).Exists)
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
#endif

                        _xmlAttributeErrors = new Dictionary<string, string>();

                        set = (T)serializer.Deserialize(fs);

                        if (_xmlAttributeErrors.Count > 0)
                        {
                            DebugLogger.WriteLine("XmlListLoader: " + _lastFile);
                            foreach (string st in _xmlAttributeErrors.Keys)
                            {
                                DebugLogger.WriteLine(st);
                            }
                        }

#if !ANDROID
                        fs.Close();
#endif  
                    }
                }
            }
            catch (Exception ex)
            {

                DebugLogger.WriteLine(ex.ToString());
                if (!appData)
                {
                    throw;
                }
            }
			
#if MONO
			DebugLogger.WriteLine("Finished [" + filename + "]  Time: " + 
				(DateTime.Now - startTime).TotalSeconds.ToString() + " secs");
#endif

            return set;
        }

        public static void Save(T list, string filename, bool appData)
        {

            string file = SaveFileName(filename, appData);
            Save(list, file);
        }

        public static void Save(T list, string filename)
        {
            FileInfo fi = new FileInfo(filename);


            #if MONO
            //DateTime startTime = DateTime.Now;
            //DebugLogger.WriteLine("Saving [" + fi.Name + "]");
            #endif

            //lastFile = filename;


            TextWriter writer = new StreamWriter(filename);

            XmlTextWriter xmlWriter = new XmlTextWriter(writer);


            _serializer.Serialize(xmlWriter, list);
            writer.Close();


            #if MONO
            //DebugLogger.WriteLine("Finished [" + fi.Name + "]  Time: " + 
            //    (DateTime.Now - startTime).TotalSeconds.ToString() + " secs");
            #endif


        }

        public static void Delete(string filename, bool appData)
        {
            string file = SaveFileName(filename, appData);

            FileInfo info = new FileInfo(file);
            if (info.Exists)
            {
                info.Delete();
            }
        }

        public static string SaveFileName(string filename, bool appData)
        {
            string path;
            if (!appData)
            {
                path = AssemblyDir;
            }
            else
            {
                path = AppDataDir;

            }


            return Path.Combine(path, filename);
            
        }

        static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            if (_xmlAttributeErrors != null)
            {
                if (!_xmlAttributeErrors.ContainsKey(e.Attr.Name))
                {
                    _xmlAttributeErrors[e.Attr.Name] = e.Attr.Name;
                }
            }
        }

        static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            if (_xmlAttributeErrors != null)
            {
                if (!_xmlAttributeErrors.ContainsKey(e.Name))
                {
                    _xmlAttributeErrors[e.Name] = e.Name;
                }
            }
        }

    }
}
