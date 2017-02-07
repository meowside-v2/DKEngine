/**
* (C) 2017 David Knieradl 
*
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using DKBasicEngine_1_0.Core;
using DKBasicEngine_1_0.Core.Components;
using DKBasicEngine_1_0.Core.Ext;
using DKBasicEngine_1_0.Core.UI;
using NAudio.Wave;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public static class Engine
    {
        public static class Render
        {
            public   const int RenderWidth        = 640;
            public   const int RenderHeight       = 360;
            internal const int ImageBufferSize    = 3 * RenderWidth * RenderHeight;
            internal const int ImageKeyBufferSize = RenderWidth * RenderHeight;

            internal static byte[] imageBuffer;
            internal static byte[] imageBufferKey;
            internal static byte[] ImageOutData;
            
            /*internal const  short sampleSize = 100;
            internal static int   lastTime   = 0;
            internal static int   numRenders = 0;*/

            internal static bool AbortRender = false;
        }

        /*public static class Sound
        {
            public static WaveOut OutputDevice;
            public static bool IsSoundAvailable
            {
                get { return OutputDevice != null; }
            }
        }*/

        public static class Input
        {
            [DllImport("user32.dll")]
            private static extern ushort GetKeyState(short nVirtKey);
            private const ushort keyDownBit = 0x80;

            internal static bool[] KeysPressed;
            internal static bool[] KeysDown;
            internal static bool[] KeysUp;

            internal static short NumberOfKeys;

            public static bool IsKeyPressed(ConsoleKey key)
            {
                return KeysPressed[(short)key];
                //return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
            }

            public static bool IsKeyDown(ConsoleKey key)
            {
                return KeysDown[(short)key];
                //return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
            }

            public static bool IsKeyUp(ConsoleKey key)
            {
                return KeysUp[(short)key];
                //return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
            }

            internal static void CheckForKeys()
            {
                for(int key = 0; key < NumberOfKeys; key++)
                {
                    bool IsDown = ((GetKeyState((short)key) & keyDownBit) == keyDownBit);

                    if (IsDown)
                    {
                        if (!KeysDown[key])
                        {
                            KeysPressed[key] = true;
                            KeysDown[key] = true;
                        }
                        else if (KeysPressed[key])
                        {
                            KeysPressed[key] = false;
                        }
                    }
                    else
                    {
                        if (KeysPressed[key] || KeysDown[key])
                        {
                            KeysPressed[key] = false;
                            KeysDown[key] = false;
                            KeysUp[key] = true;
                        }
                        else if (KeysUp[key])
                        {
                            KeysUp[key] = false;
                        }
                    }
                }
            }
        }

        //private static bool _LoadingNewPage = false;
        private static bool _IsInitialised = false;

        private static Thread BackgroundWorks;
        private static Thread RenderWorker;

        private static TextBlock FpsMeter;
        private static Stopwatch DeltaT;
        internal static Camera BaseCam;

        internal static List<Script> InitScripts;
        internal static List<Collider> Collidable;
        internal static List<GameObject> ToStart;
        internal static List<GameObject> ToRender;

        internal static Scene CurrentScene;

        private static float deltaT = 0;
        public static float deltaTime { get { return deltaT; } }
        
        internal static event EngineHandler UpdateEvent;
        internal delegate void EngineHandler();

        public static void Init()
        {
            if (!_IsInitialised)
            {
                try
                {
                    WindowControl.WindowInit();
                    Database.InitDatabase();
                    
                    Render.imageBuffer    = new byte[Render.ImageBufferSize];
                    Render.imageBufferKey = new byte[Render.ImageKeyBufferSize];
                    Render.ImageOutData   = new byte[Render.ImageBufferSize];

                    Input.NumberOfKeys = (short)Enum.GetNames(typeof(ConsoleKey)).Length;
                    Input.KeysPressed = new bool[Input.NumberOfKeys];
                    Input.KeysDown    = new bool[Input.NumberOfKeys];
                    Input.KeysUp      = new bool[Input.NumberOfKeys];

                    DeltaT    = Stopwatch.StartNew();

                    ToStart    = new List<GameObject>();
                    ToRender   = new List<GameObject>();
                    Collidable = new List<Collider>();
                    InitScripts = new List<Script>();

                    //Sound.OutputDevice = new WaveOut();

                    FpsMeter = new TextBlock();
                    FpsMeter.Transform.Position = new Vector3(0, 0, 128);
                    FpsMeter.Transform.Dimensions = new Vector3(50, 5, 1);
                    FpsMeter.Transform.Scale = new Vector3(2, 2, 1);
                    FpsMeter.VAlignment = Text.VerticalAlignment.Bottom;
                    FpsMeter.HAlignment = Text.HorizontalAlignment.Left;
                    FpsMeter.Text = "0";
                    FpsMeter.IsGUI = true;
                    FpsMeter.TextShadow = true;
                    FpsMeter.Foreground = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);
                    ToRender.Add(FpsMeter);

                    BackgroundWorks = new Thread(Update);
                    RenderWorker = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    RenderWorker.Start();

                    SplashScreen();
                }
                catch (Exception e)
                {
                    throw new Exception("Engine initialisation failed\n" + e);
                }
            }
            else
                throw new Exception("Engine is being initialised second time");
        }
        
        public static void ChangeScene(Type type)
        {
            if (_IsInitialised)
            {
                if(type.GetInterface("IPage") == typeof(IPage))
                {
                    if(CurrentScene != null)
                    {
                        int SceneModelCount = CurrentScene.Model.Count;
                        for (int i = 0; i < SceneModelCount; i++)
                            CurrentScene.Model[i].Destroy();
                    }
                    
                    Engine.CurrentScene = (Scene)Activator.CreateInstance(type);
                    Engine.CurrentScene.Init();

                    Engine.ToStart = CurrentScene.NewlyGenerated;

                    //Engine._LoadingNewPage = false;
                }
            }
            else
                throw new Exception("Engine not initialised \n Can't change page");
        }

        private static void SplashScreen()
        {
            if (!_IsInitialised)
            {
                _IsInitialised = true;

                Engine.ChangeScene(typeof(Scene));
                SplashScreen splash    = new SplashScreen();
                Camera splashScreenCam = new Camera();

                SpinWait.SpinUntil(() => splash.Animator.NumberOfPlays >= 1);

                splash.Destroy();
                splashScreenCam.Destroy();
            }
        }

        private static void Update()
        {
            
            int       NumberOfFrames = 0;
            TimeSpan  timeOut        = new TimeSpan(0, 0, 0, 0, 500);
            Stopwatch time           = Stopwatch.StartNew();

            while (true)
            {

                int ToStartCount = ToStart.Count - 1;
                while (ToStartCount >= 0)
                {
                    ToRender.Add(ToStart[ToStartCount]);
                    ToStart.Remove(ToStart[ToStartCount--]);
                }

                int InitScriptsCount = InitScripts.Count - 1;
                while (InitScriptsCount >= 0)
                {
                    InitScripts[InitScriptsCount].Start();
                    Engine.UpdateEvent += InitScripts[InitScriptsCount].UpdateHandle;
                    InitScripts.Remove(InitScripts[InitScriptsCount--]);
                }

                Input.CheckForKeys();

                deltaT = (float)DeltaT.Elapsed.TotalSeconds;
                DeltaT?.Restart();

                UpdateEvent?.Invoke();

                List<GameObject> reference = ToRender.GetGameObjectsInView();
                
                List<GameObject> VisibleTriggers = reference.Where(obj => obj.Collider != null ? obj.Collider.IsTrigger : false).ToList();
                List<GameObject> VisibleColliders = reference.Where(obj => obj.Collider != null ? obj.Collider.IsCollidable : false).ToList();
                int ColliderCount = VisibleColliders.Count;
                for (int i = 0; i < ColliderCount; i++)
                    VisibleColliders[i].Collider.TriggerCheck(VisibleTriggers);
                
                BaseCam?.BufferImage(reference);

                Buffer.BlockCopy(Render.imageBuffer, 0, Render.ImageOutData, 0, Render.ImageBufferSize);
                //Array.Copy(Render.imageBuffer, Render.ImageOutData, Render.ImageBufferSize);
                
                NumberOfFrames++;
                
                if (time.ElapsedMilliseconds > timeOut.TotalMilliseconds)
                {
                    long t = NumberOfFrames * 1000 / time.ElapsedMilliseconds;
                    FpsMeter.Text = t.ToString();
#if DEBUG
                    Debug.WriteLine(t);
#endif
                    time.Restart();
                    NumberOfFrames = 0;
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

                    fixed (byte* ptr = Render.ImageOutData)
                    {

                        using (Bitmap outFrame = new Bitmap(Render.RenderWidth,
                                                            Render.RenderHeight,
                                                            3 * Render.RenderWidth,
                                                            System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                                            new IntPtr(ptr)))
                        {
                            Rectangle imageRect = new Rectangle(0,
                                                                0,
                                                                Width,
                                                                Height);

                            g.DrawImage(outFrame, imageRect);
                        }
                    }
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}
