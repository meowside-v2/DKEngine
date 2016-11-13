using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class WindowControl
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


        private IntPtr hConsole = GetStdHandle(-11);
        private COORD xy = new COORD(100, 100);
        private const int STD_OUTPUT_HANDLE = -11;
        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public void WindowInit()
        {
            Console.SetWindowSize(20, 20);
            Console.SetBufferSize(21, 21);

            Console.CursorVisible = false;
            Console.Title = "MarIO";
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            Timer windowCheck = new Timer(WindowSizeChecker, null, 0, 100);
        }

        private void WindowSizeChecker(object state)
        {
            if (Console.WindowHeight != Console.LargestWindowHeight || Console.WindowWidth != Console.LargestWindowWidth)
            {
                SetConsoleDisplayMode(hConsole, 1, out xy);
            }
        }
    }
}
