using System.Collections;
using System.IO;

namespace CombatManager.LocalService
{
    class BinaryResourceManager
    {
        static BinaryResourceManager _manager;

        public static BinaryResourceManager Manager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new BinaryResourceManager();
                }
                return _manager;

            }
            
        }

        public  string[] GetResourceNames()
        {
            var asm = Assembly.GetEntryAssembly();
            string resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream))
            {
                return reader.Cast<DictionaryEntry>().Select(entry => (string)entry.Key
             ).ToArray();
            }
        }

        public  UnmanagedMemoryStream FindResource(string resource)
        {
            var asm = Assembly.GetEntryAssembly();
            string resName = asm.GetName().Name + ".g.resources";
            using (var stream = asm.GetManifestResourceStream(resName))
            using (var reader = new System.Resources.ResourceReader(stream))
            {
                try
                {
                    return (UnmanagedMemoryStream)reader.Cast<DictionaryEntry>().First(entry => (string)entry.Key == resource).Value;
                }
                catch (InvalidOperationException)
                {
                    return null;
                }

            }
        }
    }
}
