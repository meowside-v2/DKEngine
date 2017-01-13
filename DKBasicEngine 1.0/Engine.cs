using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

        internal static List<Collider> Collidable;
        internal static List<GameObject> ToUpdate;
        internal static List<GameObject> ToStart;
        internal static List<GameObject> ToRender;

        internal static Scene Scene;

        private static float deltaT = 0;
        public static float deltaTime { get { return deltaT; } }

        /*internal static event BackgroundWorker UpdateEvent;
        internal static event BackgroundWorker StartEvent;
        internal static event BackgroundWorker RenderEvent;
        internal static event BackgroundWorker GUIRenderEvent;
        internal static event CollisionCheck CollisionCheckEvent;

        internal delegate void BackgroundWorker();
        internal delegate void CollisionCheck(Collider e);*/

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
                    
                    Render.imageBuffer    = new byte[Render.ImageBufferSize];
                    Render.imageBufferKey = new byte[Render.ImageKeyBufferSize];

                    _deltaT    = Stopwatch.StartNew();

                    ToStart    = new List<GameObject>();
                    ToUpdate   = new List<GameObject>();
                    ToRender   = new List<GameObject>();
                    Collidable = new List<Collider>();

                    BackgroundWorks = new Thread(Update);
                    RenderWorker    = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    RenderWorker.Start();

                    fpsMeter = new TextBlock();
                    fpsMeter.Position = new Position(1, -1, 128);
                    fpsMeter.Dimensions = new Dimensions(25, 5, 1);
                    fpsMeter.Scale = new Scale(2, 2, 1);
                    fpsMeter.VAlignment = TextBlock.VerticalAlignment.Bottom;
                    fpsMeter.HAlignment = TextBlock.HorizontalAlignment.Left;
                    fpsMeter.Text = "0";
                    fpsMeter.IsGUI = true;
                    fpsMeter.TextShadow = true;
                    fpsMeter.Foreground = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);

                    //Engine.ToRender.Add(fpsMeter);

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
        
        public static void PageChange(Scene Scene)
        {
            if (_isInitialised)
            {
                Engine.Scene = Scene;
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

        private static void SplashScreen()
        {
            if (!_isInitialised)
            {
                SplashScreen splash     = new SplashScreen();
                Camera splashScreenCam = new Camera();

                SpinWait.SpinUntil(() => splash.Animator.NumberOfPlays >= 1);

                splash.Destroy();
                splashScreenCam.Destroy();
            }
        }

        private static void Update()
        {
            while (true)
            {
                /*int referenceCount = reference.Count;
                for(int i = referenceCount - 1; i >= 0; i--)
                {
                    if (reference[i] is I3Dimensional)
                        if (!((I3Dimensional)reference[i]).IsInView())
                            reference.Remove(reference[i]);
                }*/

                /*if(Page != null)
                {
                    int controlReferenceCount = Page.PageControls.Count;
                    for (int i = 0; i < controlReferenceCount; i++)
                        Page.PageControls[i].IsFocused = i == Page.PageControls[i].FocusElementID;
                }*/
                
                int ToStartCount = ToStart.Count;
                while (ToStartCount > 0)
                {
                    ToStartCount--;
                    ToStart[ToStartCount].Start();
                    ToUpdate.Add(ToStart[ToStartCount]);
                    ToStart.Remove(ToStart[ToStartCount]);
                }

                List<GameObject> reference = ToUpdate.GetGameObjectsInView();

                deltaT = (float)_deltaT.Elapsed.TotalSeconds;
                _deltaT?.Restart();

                int refereceCount = reference.Count;
                for (int i = 0; i < refereceCount; i++)
                    reference[i].Update();
                
                List<GameObject> Triggers = reference.Where(obj => obj.Collider != null ? obj.Collider.IsTrigger : false).ToList();
                List<GameObject> VisibleWithCollider = reference.Where(obj => obj.Collider != null ? !obj.Collider.IsTrigger : false).ToList();
                int TriggersCount = Triggers.Count;
                for (int i = 0; i < TriggersCount; i++)
                    Triggers[i].Collider.TriggerCheck(VisibleWithCollider);
                
                _baseCam?.BufferImage();

                Array.Copy(Render.imageBuffer, Render.ImageOutData, Render.ImageBufferSize);

                if (Render.numRenders == 0)
                    Render.lastTime = Environment.TickCount;

                Render.numRenders++;

                if (Render.numRenders == Render.sampleSize)
                {
                    int temp = Environment.TickCount - Render.lastTime;

                    if (temp > 0)
                    {
                        fpsMeter.Text = string.Format("{0}", Render.sampleSize * 1000 / temp);
#if DEBUG
                        //Debug.WriteLine(string.Format("Buff {0}", Render.sampleSize * 1000 / temp));
#endif
                    }
                    Render.numRenders = 0;
                }
            }
        }

        private static unsafe void RenderImage()
        {
            IntPtr ConsoleWindow = GetConsoleWindow();

            using (Graphics g = Graphics.FromHwnd(ConsoleWindow))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.PixelOffsetMode    = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode      = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                
                Rectangle Screen = System.Windows.Forms.Screen.FromHandle(ConsoleWindow).Bounds;
                int Width        = Screen.Width;
                int Height       = Screen.Height;
                
                while (!Render.AbortRender)
                {
                    Rectangle ScreenResCheck = System.Windows.Forms.Screen.FromHandle(ConsoleWindow).Bounds;

                    if (ScreenResCheck != Screen)
                    {
                        Width  = ScreenResCheck.Width;
                        Height = ScreenResCheck.Height;
                    }
                    //Size imageSize = new Size(Console.WindowWidth, Console.WindowHeight); // desired image size in characters

                    fixed (byte* ptr = Render.ImageOutData)
                    {

                        using (Bitmap outFrame = new Bitmap(Render.RenderWidth,
                                                            Render.RenderHeight,
                                                            3 * Render.RenderWidth,
                                                            System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                                            new IntPtr(ptr)))
                        {
                            //Size fontSize = GetConsoleFontSize();

                            Rectangle imageRect = new Rectangle(0,
                                                                0,
                                                                Width, //imageSize.Width * fontSize.Width,
                                                                Height); //imageSize.Height * fontSize.Height);

                            g.DrawImage(outFrame, imageRect);
                        }
                    }
                }
            }
        }

        /*private static Size GetConsoleFontSize()
        {
            IntPtr outHandle = GetStdHandle(-11);
            ConsoleFontInfo cfi = new ConsoleFontInfo();

            if (!GetCurrentConsoleFont(outHandle, false, cfi))
                throw new InvalidOperationException("Unable to get font information.");

            return new Size(cfi.dwFontSize.X, cfi.dwFontSize.Y);
        }*/

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        /*[DllImport("kernel32.dll", SetLastError = true)]
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
        private class ConsoleFontInfo
        {
            public int nFont;
            public Coord dwFontSize;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct Coord
        {
            [FieldOffset(0)]
            internal short X;
            [FieldOffset(2)]
            internal short Y;
        }*/
    }
}
