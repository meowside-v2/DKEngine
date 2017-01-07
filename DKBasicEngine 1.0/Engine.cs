using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace DKBasicEngine_1_0
{
    public static class Engine
    {
        public static class Render
        {
            public const int RenderWidth  = 640;
            public const int RenderHeight = 360;
            internal const int ImageBufferSize = 3 * RenderWidth * RenderHeight;
            internal const int ImageKeyBufferSize = RenderWidth * RenderHeight;

            internal static byte[] imageBuffer;
            internal static byte[] imageBufferKey;

            internal static readonly byte[] ImageOutData = new byte[ImageBufferSize];
            
            internal const short sampleSize = 100;
            internal static int lastTime = 0;
            internal static int numRenders = 0;

            internal static bool AbortRender = false;
        }

        public static class Input
        {
            [DllImport("user32.dll")]
            private static extern ushort GetKeyState(short nVirtKey);

            private const ushort keyDownBit = 0x80;

            public static bool IsKeyPressed(ConsoleKey key)
            {
                return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
            }
        }

        private static bool _isInitialised = false;

        private static Thread BackgroundWorks;
        private static Thread RenderWorker;

        private static TextBlock fpsMeter;
        private static Stopwatch _deltaT;
        internal static Camera _baseCam;

        internal static List<ICore> ToUpdate;
        internal static List<ICore> ToStart;
        internal static List<GameObject> ToRender;
        internal static List<IControl> PageControls;

        internal static IPage Page;

        public static float deltaTime { get { return (float)_deltaT.Elapsed.TotalSeconds; } }

        

        public static void Init()
        {
            if (!_isInitialised)
            {
                try
                {
                    Console.CursorVisible = false;
                    Console.SetOut(TextWriter.Null);
                    Console.SetIn(TextReader.Null);

                    WindowControl.WindowInit();
                    Database.InitDatabase();
                    
                    Render.imageBuffer      = new byte[Render.ImageBufferSize];
                    Render.imageBufferKey   = new byte[Render.ImageKeyBufferSize];

                    _deltaT         = Stopwatch.StartNew();
                    ToStart         = new List<ICore>();
                    ToUpdate        = new List<ICore>();
                    ToRender        = new List<GameObject>();
                    PageControls    = new List<IControl>();

                    BackgroundWorks = new Thread(Update);
                    RenderWorker    = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    RenderWorker.Start();

                    fpsMeter = new TextBlock()
                    {
                        Transform = new Transform(1, -1, 128),
                        Dimensions = new Dimensions(20, 5, 1),
                        VAlignment = TextBlock.VerticalAlignment.Bottom,
                        HAlignment = TextBlock.HorizontalAlignment.Left,
                        Text = "0",
                        IsGUI = true
                    };

                    Engine.ToRender.Add(fpsMeter);

                    SplashScreen();

                    _isInitialised = true;
                }
                catch (Exception e)
                {
                    throw new Exception("Engine initialisation failed\n" + e);
                }
            }
            else
                throw new Exception("Engine is being initialised second time");
        }
        
        public static void PageChange(IPage Page)
        {
            if (_isInitialised)
            {
                Engine.Page = Page;
                
                Engine.PageControls = Page.PageControls;
            }
            else
                throw new Exception("Engine not initialised \n Can't change page");
        }

        public static void Pause()
        {
            if (_deltaT.IsRunning) _deltaT?.Stop();
            if (BackgroundWorks.IsAlive) BackgroundWorks?.Abort();
            if (RenderWorker.IsAlive)
            {
                RenderWorker.Abort();
                Render.AbortRender = true;
            }
        }

        public static void Resume()
        {
            if(!_deltaT.IsRunning) _deltaT?.Start();
            if(!BackgroundWorks.IsAlive) BackgroundWorks?.Start();
            if (!RenderWorker.IsAlive)
            {
                Render.AbortRender = false;
                RenderWorker.Start();
            }
        }

        internal static void SplashScreen()
        {
            if (!_isInitialised)
            {
                Camera splashScreenCam  = new Camera();
                SplashScreen splash     = new SplashScreen(null, null);
                
                splashScreenCam.Init(0, 0);

                SpinWait.SpinUntil(() => splash.Animator.NumberOfPlays >= 1);

                splash.Destroy();
                splashScreenCam.Destroy();
            }
        }

        private static void Update()
        {
            while (true)
            {
                List<ICore> reference;
                List<IControl> controlReference;

                lock (ToUpdate)
                {
                    reference = ToUpdate.ToList();
                }

                lock (PageControls)
                {
                    controlReference = PageControls.ToList();
                }

                int ToStartCount = ToStart.Count;
                
                while (ToStartCount > 0)
                {
                    ToStartCount--;
                    ToStart[ToStartCount].Start();
                    ToStart.Remove(ToStart[ToStartCount]);
                }

                int controlReferenceCount = controlReference.Count;
                for (int i = 0; i < controlReferenceCount; i++)
                {
                    bool result = Page.FocusSelection == controlReference.FindIndex(obj => ReferenceEquals(obj, controlReference[i]));

                    if (controlReference[i].IsFocused != result)
                        controlReference[i].IsFocused = result;
                }

                List<ICore> tempReference = reference.ToList();

                int tempReferenceCount = tempReference.Count;
                for (int i = 0; i < tempReferenceCount; i++)
                {
                    ICore tmp = tempReference[i];
                    if (tmp is I3Dimensional)
                        if (!((I3Dimensional)tempReference[i]).IsInView())
                            reference.Remove(tempReference[i]);
                }


                _deltaT?.Stop();

                int refereceCount = reference.Count;
                for (int i = 0; i < refereceCount; i++)
                    reference[i].Update();

                _deltaT?.Restart();

                _baseCam?.BufferImage();

                Array.Copy(Render.imageBuffer, Render.ImageOutData, Render.ImageBufferSize);

                if (Render.numRenders == 0)
                {
                    Render.lastTime = Environment.TickCount;
                }

                Render.numRenders++;

                if (Render.numRenders == Render.sampleSize)
                {
                    int temp = Environment.TickCount - Render.lastTime;

                    if (temp > 0)
                    {
                        fpsMeter.Text = string.Format("{0}", Render.sampleSize * 1000 / temp);
#if DEBUG
                        Debug.WriteLine(string.Format("Buff {0}", Render.sampleSize * 1000 / temp));
#endif
                    }

                    Render.numRenders = 0;
                }
            }
        }

        private static unsafe void RenderImage()
        {

            using (Graphics g = Graphics.FromHwnd(GetConsoleWindow()))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

                Point location = new Point(0, 0);

                while (!Render.AbortRender)
                {
                    Size imageSize = new Size(Console.WindowWidth, Console.WindowHeight); // desired image size in characters

                    fixed (byte* ptr = Render.ImageOutData)
                    {

                        using (Bitmap outFrame = new Bitmap(Engine.Render.RenderWidth,
                                                            Engine.Render.RenderHeight,
                                                            3 * Engine.Render.RenderWidth,
                                                            System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                                            new IntPtr(ptr)))
                        {
                            Size fontSize = GetConsoleFontSize();

                            Rectangle imageRect = new Rectangle(location.X * fontSize.Width,
                                                                location.Y * fontSize.Height,
                                                                imageSize.Width * fontSize.Width,
                                                                imageSize.Height * fontSize.Height);

                            g.DrawImage(outFrame, imageRect);
                        }
                    }
                }
            }
        }

        private static Size GetConsoleFontSize()
        {
            IntPtr outHandle = GetStdHandle(-11);

            ConsoleFontInfo cfi = new ConsoleFontInfo();

            if (!GetCurrentConsoleFont(outHandle, false, cfi))
            {
                throw new InvalidOperationException("Unable to get font information.");
            }

            return new Size(cfi.dwFontSize.X, cfi.dwFontSize.Y);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetCurrentConsoleFont(
            IntPtr hConsoleOutput,
            bool bMaximumWindow,
            [Out][MarshalAs(UnmanagedType.LPStruct)]ConsoleFontInfo lpConsoleCurrentFont);

        [DllImport("kernel32.dll",
         EntryPoint = "GetStdHandle",
         SetLastError = true,
         CharSet = CharSet.Auto,
         CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [StructLayout(LayoutKind.Sequential)]
        internal class ConsoleFontInfo
        {
            internal int nFont;
            internal Coord dwFontSize;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct Coord
        {
            [FieldOffset(0)]
            internal short X;
            [FieldOffset(2)]
            internal short Y;
        }
    }
}
