/*
* (C) 2017 David Knieradl 
*/

/**
* For the brave souls who get this far: You are the chosen ones,
* the valiant knights of programming who toil away, without rest,
* fixing our most awful code. To you, true saviors, kings of men,
* I say this: never gonna give you up, never gonna let you down,
* never gonna run around and desert you. Never gonna make you cry,
* never gonna say goodbye. Never gonna tell a lie and hurt you.
*/

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

        private static bool _LoadingNewPage = false;
        private static bool _IsInitialised = false;

        private static Thread BackgroundWorks;
        private static Thread RenderWorker;

        private static TextBlock FpsMeter;
        private static Stopwatch DeltaT;
        internal static Camera BaseCam;

        internal static List<Collider> Collidable;
        internal static List<GameObject> ToStart;
        internal static List<GameObject> ToRender;

        internal static Scene Scene;

        private static float deltaT = 0;
        public static float deltaTime { get { return deltaT; } }

        internal static event UpdateHandler UpdateEvent;
        internal delegate void UpdateHandler();

        /*internal static event BackgroundWorker UpdateEvent;
        internal static event BackgroundWorker StartEvent;
        internal static event BackgroundWorker RenderEvent;
        internal static event BackgroundWorker GUIRenderEvent;
        internal static event CollisionCheck CollisionCheckEvent;

        internal delegate void BackgroundWorker();
        internal delegate void CollisionCheck(Collider e);*/

        public static void Init()
        {
            if (!_IsInitialised)
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

                    DeltaT    = Stopwatch.StartNew();

                    ToStart    = new List<GameObject>();
                    ToRender   = new List<GameObject>();
                    Collidable = new List<Collider>();

                    BackgroundWorks = new Thread(Update);
                    RenderWorker    = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    RenderWorker.Start();

                    FpsMeter = new TextBlock();
                    FpsMeter.Transform.Position = new Position(0, 0, 128);
                    FpsMeter.Transform.Dimensions = new Dimensions(50, 5, 1);
                    FpsMeter.Transform.Scale = new Scale(2, 2, 1);
                    FpsMeter.VAlignment = TextBlock.VerticalAlignment.Bottom;
                    FpsMeter.HAlignment = TextBlock.HorizontalAlignment.Left;
                    FpsMeter.Text = "0";
                    FpsMeter.IsGUI = true;
                    FpsMeter.TextShadow = true;
                    FpsMeter.Foreground = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);
                    
                    SplashScreen();

                    _IsInitialised = true;
                }
                catch (Exception e)
                {
                    throw new Exception("Engine initialisation failed\n" + e);
                }
            }
            else
                throw new Exception("Engine is being initialised second time");
        }
        
        public static void ChangeScene(Scene Scene)
        {
            if (_IsInitialised)
            {
                Engine._LoadingNewPage = true;
                Engine.Scene = Scene;
                Scene.Init();

                Engine._LoadingNewPage = false;
            }
            else
                throw new Exception("Engine not initialised \n Can't change page");
        }

        public static void Pause()
        {
            if (DeltaT.IsRunning) DeltaT?.Stop();
            if (BackgroundWorks.IsAlive) BackgroundWorks?.Abort();
            if (RenderWorker.IsAlive)
            {
                RenderWorker.Abort();
                Render.AbortRender = true;
            }
        }

        public static void Resume()
        {
            if(!DeltaT.IsRunning) DeltaT?.Start();
            if(!BackgroundWorks.IsAlive) BackgroundWorks?.Start();
            if (!RenderWorker.IsAlive)
            {
                Render.AbortRender = false;
                RenderWorker.Start();
            }
        }

        private static void SplashScreen()
        {
            if (!_IsInitialised)
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

                if (Engine._LoadingNewPage)
                {
                    SpinWait.SpinUntil(() => !Engine._LoadingNewPage);
                }

                int ToStartCount = ToStart.Count;
                lock(ToStart)
                    while (ToStartCount > 0)
                    {
                        ToStartCount--;
                        ToStart[0].Start();
                        UpdateEvent += ToStart[0].UpdateHandler;
                        ToStart.Remove(ToStart[0]);
                    }

                //List<GameObject> reference = ToUpdate.GetGameObjectsInView();

                deltaT = (float)DeltaT.Elapsed.TotalSeconds;
                DeltaT?.Restart();

                UpdateEvent?.Invoke();

                /*int refereceCount = reference.Count;
                for (int i = 0; i < refereceCount; i++)
                    reference[i].Update();*/

                List<GameObject> reference = ToRender.GetGameObjectsInView();

                List<GameObject> Triggers = reference.Where(obj => obj.Collider != null ? obj.Collider.IsTrigger : false).ToList();
                List<GameObject> VisibleWithCollider = reference.Where(obj => obj.Collider != null ? !obj.Collider.IsTrigger : false).ToList();
                int TriggersCount = Triggers.Count;
                for (int i = 0; i < TriggersCount; i++)
                    Triggers[i].Collider.TriggerCheck(VisibleWithCollider);
                
                BaseCam?.BufferImage(reference);

                Array.Copy(Render.imageBuffer, Render.ImageOutData, Render.ImageBufferSize);

                if (Render.numRenders == 0)
                    Render.lastTime = Environment.TickCount;

                Render.numRenders++;

                if (Render.numRenders == Render.sampleSize)
                {
                    int temp = Environment.TickCount - Render.lastTime;

                    if (temp > 0)
                    {
                        FpsMeter.Text = string.Format("{0}", Render.sampleSize * 1000 / temp);
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
