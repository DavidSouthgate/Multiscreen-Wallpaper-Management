using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MultiScreenWallpaper
{
    static class Program
    {
        //Mutex used to ensure no two concurrent processes of the program run
        static Mutex mutex = new Mutex(true, "{aa69aa30-09fa-47ad-893a-19204598bdfa}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //If an instance of the program isn't running
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                new frmWallpaperManagement();
                Application.Run();

                //Relase Mutex
                mutex.ReleaseMutex();
            }

            else
            {
                //Declare string array used to get command line arguments
                string[] args = Environment.GetCommandLineArgs();

                bool noArgUsed = true;

                //For each argument
                foreach (string arg in args)
                {

                    //If argument is for update
                    if (arg == "-update")
                    {

                        //Send update message to already running process
                        NativeMethods.PostMessage(
                            (IntPtr)NativeMethods.HWND_BROADCAST,
                            NativeMethods.UPDATE,
                            IntPtr.Zero,
                            IntPtr.Zero);

                        noArgUsed = false;
                    }

                    //If argument is for debug
                    else if (arg == "-debug")
                    {

                        //Send debug message to already running process
                        NativeMethods.PostMessage(
                            (IntPtr)NativeMethods.HWND_BROADCAST,
                            NativeMethods.DEBUG,
                            IntPtr.Zero,
                            IntPtr.Zero);

                        noArgUsed = false;
                    }
                }

                if(noArgUsed == true)
                {
                    MessageBox.Show("Program already running and/or invalid command line option");
                }
            }
        }
    }
}
