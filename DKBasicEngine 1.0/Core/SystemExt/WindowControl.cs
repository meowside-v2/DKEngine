using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace DKBasicEngine_1_0
{
    public static class WindowControl
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public short X;
            public short Y;
            public COORD(short x, short y)
            {
                this.X = x;
                this.Y = y;
            }

        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleDisplayMode(
            IntPtr ConsoleOutput
            , uint Flags
            , out COORD NewScreenBufferDimensions
            );


        private static IntPtr hConsole = GetStdHandle(-11);
        private static COORD xy = new COORD(100, 100);
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        internal static void WindowInit()
        {
            Console.SetWindowSize(20, 20);
            Console.SetBufferSize(21, 21);

            Console.CursorVisible = false;
            Console.Title = "MarIO";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Timer windowCheck = new Timer(WindowSizeChecker, null, 0, 100);
        }

        private static void WindowSizeChecker(object state)
        {
            if (Console.WindowHeight != Console.LargestWindowHeight || Console.WindowWidth != Console.LargestWindowWidth)
            {
                SetConsoleDisplayMode(hConsole, 1, out xy);
            }
        }
    }
}
