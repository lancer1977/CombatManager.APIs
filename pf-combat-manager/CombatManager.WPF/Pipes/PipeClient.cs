using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;

namespace CombatManager.Pipes
{
    static class PipeClient
    {
        public static void SendFile(string filename)
        {
            using (var pipeClient =
                new NamedPipeClientStream(".", "CombatManager.FilePipe", PipeDirection.InOut))
            {

                // Connect to the pipe or wait until the pipe is available.
                pipeClient.Connect();
                var sw= new StreamWriter(pipeClient);
                
                // Display the read text to the console 
                sw.WriteLine(filename);
                sw.Flush();
                using (var sr = new StreamReader(pipeClient))
                {
                    var ptr = new IntPtr(int.Parse(sr.ReadLine()));
                    SetForegroundWindow(ptr);

                }
                
            }

        }

        [DllImportAttribute("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);
    }
}