using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SoundCatcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NativeMethods.AllocConsole();
            Console.WriteLine("Result Console");
            Application.Run(new FormMain());
            NativeMethods.FreeConsole();
        }
    }
}
