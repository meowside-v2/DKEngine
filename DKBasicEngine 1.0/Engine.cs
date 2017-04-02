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
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.Ext;
using DKEngine.Core.UI;

namespace DKEngine
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
            internal static bool[] KeysReleased;
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

            public static bool IsKeyReleased(ConsoleKey key)
            {
                return KeysReleased[(short)key];
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
                            KeysUp[key] = false;
                            KeysReleased[key] = false;
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
                        if (KeysDown[key])
                        {
                            KeysPressed[key] = false;
                            KeysDown[key] = false;
                            KeysReleased[key] = true;
                            KeysUp[key] = true;
                        }
                        else if (KeysReleased[key])
                        {
                            KeysReleased[key] = false;
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

        internal static Scene CurrentScene { get; set; }
        internal static Scene LoadingScene { get; set; }

        internal static List<GameObject> RenderObjects;

        private static float deltaT = 0;
        public static float deltaTime { get { return deltaT; } }

        internal static long LastUpdated = 0;

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
                    Input.KeysPressed  = new bool[Input.NumberOfKeys];
                    Input.KeysDown     = new bool[Input.NumberOfKeys];
                    Input.KeysUp       = new bool[Input.NumberOfKeys];
                    Input.KeysReleased = new bool[Input.NumberOfKeys];

                    DeltaT = Stopwatch.StartNew();

                    RenderObjects = new List<GameObject>(0xFFFF);

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

                    FpsMeter.InitInternal();

                    UpdateEvent += FpsMeter.Scripts[0].UpdateHandle;

                    BackgroundWorks = new Thread(Update);
                    RenderWorker    = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    RenderWorker.Start();



#if !DEBUG
                    SplashScreen();
#endif

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
        
        public static void LoadSceneToMemory<T>() where T : Scene
        {
            Engine.LoadingScene = (T)Activator.CreateInstance(typeof(T));
            Engine.LoadingScene.Init();

            while (Engine.LoadingScene.NewlyGeneratedGameObjects.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedGameObjects.Pop().InitInternal();
            }

            while (Engine.LoadingScene.NewlyGeneratedBehaviors.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedBehaviors.Pop().Start();
            }

            Database.AddScene(Engine.LoadingScene);
            
            //Engine.LoadingScene.Init();
        }

        public static void LoadScene<T>() where T : Scene
        {
            Engine.LoadingScene = (T)Activator.CreateInstance(typeof(T));
            Engine.LoadingScene.Init();

            while (Engine.LoadingScene.NewlyGeneratedGameObjects.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedGameObjects.Pop().InitInternal();
            }

            while (Engine.LoadingScene.NewlyGeneratedBehaviors.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedBehaviors.Pop().Start();
            }

            RegisterScene(Engine.LoadingScene);
        }

        public static void ReloadScene(string Name)
        {
            ReloadScene(Database.GetScene(Name));
        }
        
        public static void ChangeScene(string Name, bool Reload = false, params string[] args)
        {
            UnregisterScene();
            if (Reload)
                ReloadScene(Name);
            RegisterScene(Database.GetScene(Name), args);
        }

        private static void UnregisterScene()
        {
            try
            {
                foreach (var item in CurrentScene.AllBehaviors)
                {
                    try
                    {
                        UpdateEvent -= item.UpdateHandle;
                    }
                    catch { }
                }
            }
            catch { }
        }

        private static void RegisterScene(Scene source, params string[] args)
        {
            Engine.CurrentScene = Engine.LoadingScene;
            source.Set(args);
            
            foreach (var item in source.AllBehaviors)
            {
                try
                {
                    UpdateEvent += item.UpdateHandle;
                }
                catch { }
            }
        }

        private static void ReloadScene(Scene source)
        {

        }

        /*public static void ChangeScene<T>() where T : Scene
        {
            if (CurrentScene != null)
            {
                foreach (var pair in Engine.CurrentScene.AllComponents)
                    pair.Value.Destroy();

                int ComponentCount = Engine.CurrentScene.AllBehaviors.Count;
                for (int i = ComponentCount - 1; i >= 0; i--)
                    Engine.CurrentScene.AllBehaviors[i].Destroy();
            }

            Engine.LoadingScene = (T)Activator.CreateInstance(typeof(T));
            Engine.LoadingScene.Init();
            
            Engine.CurrentScene = Engine.LoadingScene;
            Engine.NewGameobjects = Engine.CurrentScene.NewlyGeneratedGameObjects;
            Engine.NewComponents = Engine.CurrentScene.NewlyGeneratedComponents;
                
            //Engine._LoadingNewPage = false;
        }*/

        private static void SplashScreen()
        {
            if (!_IsInitialised)
            {
                Engine.LoadScene<Scene>();

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
                Engine.LastUpdated = Environment.TickCount;

                Input.CheckForKeys();

                deltaT = (float)DeltaT.Elapsed.TotalSeconds;
                DeltaT?.Restart();

                UpdateEvent?.Invoke();
                
                while(Engine.CurrentScene?.GameObjectsToAddToRender.Count > 0)
                {
                    GameObject tmp = Engine.CurrentScene.GameObjectsToAddToRender.Pop();
                    Engine.RenderObjects.Add(tmp);
                    Engine.CurrentScene.GameObjectsAddedToRender.Push(tmp);
                }

                List<GameObject> reference = Engine.RenderObjects.GetGameObjectsInView();
                
                List<GameObject> VisibleTriggers = reference.Where(obj => obj.Collider != null ? obj.Collider.IsTrigger : false).ToList();
                List<GameObject> VisibleColliders = reference.Where(obj => obj.Collider != null ? !obj.Collider.IsTrigger : false).ToList();
                int ColliderCount = VisibleTriggers.Count;
                for (int i = 0; i < ColliderCount; i++)
                    VisibleTriggers[i].Collider.TriggerCheck(VisibleColliders);
                
                BaseCam?.BufferImage(reference);

                Buffer.BlockCopy(Render.imageBuffer, 0, Render.ImageOutData, 0, Render.ImageBufferSize);
                
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

                    Thread.Sleep(1);
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}
