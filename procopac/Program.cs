using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace procopac
{
    class Program
    {
        //System shit:

        [DllImport("user32.dll", EntryPoint = "GetWindowLongA", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongA", SetLastError = true)]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        private const int GwlExstyle = -20;
        private const int WsExLayered = 0x80000;
        private const int LwaAlpha = 2;

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: \nprocopac [procname/PID] [opacity]\n" +
                    "[procname/PID] should be the executable name without the '.exe' extension\n" +
                    "[opacity] should be an integer number between 40 and 255");
                return;
            }

            Process p;

            //1st arg
            int iOut;
            bool Convert = int.TryParse(args[0], out iOut);

            if (Convert)
            {
                try
                {
                    p = Process.GetProcessById(iOut);
                }
                catch
                {
                    Console.WriteLine("No processes found with PID '{0}'", args[0]);
                    return;
                }
            }
            else
            {
                Process[] ps = Process.GetProcessesByName(args[0]);
                if (ps.Length == 0)
                {
                    Console.WriteLine("No processes found named '{0}'", args[0]);
                    return;
                }

                if (ps.Length > 1)
                {
                    Console.WriteLine("Many processes found named '{0}'", args[0]);
                    return;
                }

                p = ps[0];
            }

            //2nd arg
            byte bOut;
            Convert = byte.TryParse(args[1], out bOut);

            if (!Convert || bOut < 40)
            {
                Console.WriteLine("Second argument [opacity] should be a numeric value between 40 and 255");
                return;
            }

            try
            {
                IntPtr hwnd = p.MainWindowHandle;
                int windowLong = GetWindowLong(hwnd, GwlExstyle);
                SetWindowLong(hwnd, GwlExstyle, windowLong | WsExLayered);
                SetLayeredWindowAttributes(hwnd, 0, bOut, LwaAlpha);
                Console.WriteLine("Setting opacity succeeded");
                return;
            }
            catch
            {
                Console.WriteLine("Setting opacity failed");
                return;
            }
        }
    }
}
