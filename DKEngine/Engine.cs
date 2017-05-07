using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.Ext;
using DKEngine.Core.UI;

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
using System.Threading.Tasks;

namespace DKEngine
{
    public static class Engine
    {
        public static class Sound
        {
            public static bool IsSoundEnabled = false;
            public static float SoundVolume = 1f;

            internal readonly static SoundPlayer Instance = new SoundPlayer();
        }

        public static class Render
        {
            public const int ResolutionScale = 40;
            public const float ResolutionRatio = ResolutionScale / 100f;

            public const int RenderWidth = (int)(640 * ResolutionRatio);
            public const int RenderHeight = (int)(480 * ResolutionRatio);

            internal const int ImageBufferSize = 3 * RenderWidth * RenderHeight;
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
                for (int key = 0; key < NumberOfKeys; key++)
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
        //private static Thread RenderWorker;

        private static TextBlock FpsMeter;
        private static Stopwatch DeltaT;
        internal static Camera BaseCam;

        internal static Scene CurrentScene { get; set; }
        internal static Scene LoadingScene { get; set; }

        internal static List<GameObject> RenderObjects;

        private static float deltaT = 0;
        public static float DeltaTime { get { return deltaT; } }

        private static TimeSpan lastUpdated = new TimeSpan();
        public static TimeSpan LastUpdated { get { return lastUpdated; } }

        public static string SceneName { get { return Engine.LoadingScene != null ? Engine.LoadingScene.Name : ""; } }

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

                    Render.imageBuffer = new byte[Render.ImageBufferSize];
                    Render.imageBufferKey = new byte[Render.ImageKeyBufferSize];
                    Render.ImageOutData = new byte[Render.ImageBufferSize];

                    Input.NumberOfKeys = (short)Enum.GetNames(typeof(ConsoleKey)).Length;
                    Input.KeysPressed = new bool[Input.NumberOfKeys];
                    Input.KeysDown = new bool[Input.NumberOfKeys];
                    Input.KeysUp = new bool[Input.NumberOfKeys];
                    Input.KeysReleased = new bool[Input.NumberOfKeys];

                    DeltaT = Stopwatch.StartNew();

                    RenderObjects = new List<GameObject>(0xFFFF);

                    //Sound.OutputDevice = new WaveOut();

                    FpsMeter = new TextBlock();
                    FpsMeter.Transform.Position = new Vector3(4, -4, 128);
                    FpsMeter.Transform.Dimensions = new Vector3(50, 5, 1);
                    FpsMeter.VAlignment = Text.VerticalAlignment.Bottom;
                    FpsMeter.HAlignment = Text.HorizontalAlignment.Left;
                    FpsMeter.Text = "0";
                    FpsMeter.IsGUI = true;
                    FpsMeter.TextShadow = true;
                    FpsMeter.Foreground = Color.FromArgb(0xFF, 0x00, 0xFF, 0xFF);

                    FpsMeter.InitInternal();

                    UpdateEvent += FpsMeter.Scripts[0].UpdateHandle;

                    BackgroundWorks = new Thread(Update);
                    //RenderWorker    = new Thread(RenderImage);
                    BackgroundWorks.Start();
                    //RenderWorker.Start();

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

            /*while (Engine.LoadingScene.NewlyGeneratedComponents.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedComponents.Pop().InitInternal();
            }

            while (Engine.LoadingScene.NewlyGeneratedBehaviors.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedBehaviors.Pop().Start();
            }*/

            Database.AddScene(Engine.LoadingScene);

            //Engine.LoadingScene.Init();
        }

        public static void LoadScene<T>() where T : Scene
        {
            Engine.LoadingScene = (T)Activator.CreateInstance(typeof(T));
            Engine.LoadingScene.Init();

            /*while (Engine.LoadingScene.NewlyGeneratedComponents.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedComponents.Pop().InitInternal();
            }

            while (Engine.LoadingScene.NewlyGeneratedBehaviors.Count > 0)
            {
                Engine.LoadingScene.NewlyGeneratedBehaviors.Pop().Start();
            }*/

            UnregisterScene();
            RegisterScene(Engine.LoadingScene);
            //ChangeScene(Engine.LoadingScene.Name);
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

                while(CurrentScene.GameObjectsAddedToRender.Count > 0)
                {
                    GameObject tmp = CurrentScene.GameObjectsAddedToRender.Pop();
                    if (Engine.RenderObjects.Contains(tmp))
                        Engine.RenderObjects.Remove(tmp);

                    CurrentScene.GameObjectsToAddToRender.Push(tmp);
                }
            }
            catch { }
        }

        private static void RegisterScene(Scene source, params string[] args)
        {
            Engine.LoadingScene = source;
            source.Set(args);

            foreach (var item in source.AllBehaviors)
            {
                try
                {
                    UpdateEvent += item.UpdateHandle;
                }
                catch { }
            }

            Engine.CurrentScene = source;
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

                SplashScreen splash = new SplashScreen();
                Camera splashScreenCam = new Camera();

                SpinWait.SpinUntil(() => splash.Animator.NumberOfPlays >= 1);

                splash.Destroy();
                splashScreenCam.Destroy();
            }
        }

        private static void Update()
        {
            Task imageRender = Task.Factory.StartNew(RenderImage);

            int NumberOfFrames = 0;
            TimeSpan timeOut = new TimeSpan(0, 0, 0, 0, 500);
            Stopwatch time = Stopwatch.StartNew();

            while (true)
            {
                Input.CheckForKeys();

                lastUpdated += DeltaT.Elapsed;
                deltaT = (float)DeltaT.Elapsed.TotalSeconds;
                DeltaT?.Restart();

                UpdateEvent?.Invoke();
                
                while (Engine.CurrentScene?.NewlyGeneratedComponents.Count > 0)
                {
                    Engine.CurrentScene.NewlyGeneratedComponents.Pop().InitInternal();
                }

                while (Engine.CurrentScene?.NewlyGeneratedBehaviors.Count > 0)
                {
                    Behavior tmp = Engine.CurrentScene.NewlyGeneratedBehaviors.Pop();
                    UpdateEvent += tmp.UpdateHandle;
                    tmp.Start();
                }

                while(Engine.CurrentScene?.DestroyObjectAwaitList.Count > 0)
                {
                    GameObject tmp = Engine.CurrentScene.DestroyObjectAwaitList[0];
                    Engine.CurrentScene.DestroyObjectAwaitList.RemoveAt(0);
                    tmp.Destroy();
                }

                while (Engine.CurrentScene?.GameObjectsToAddToRender.Count > 0)
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
                    VisibleTriggers[i].Collider?.TriggerCheck(VisibleColliders);

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

        private static async void RenderImage()
        {
            IntPtr ConsoleWindow = GetConsoleWindow();

            using (Graphics g = Graphics.FromHwnd(ConsoleWindow))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                Rectangle Screen = System.Windows.Forms.Screen.FromHandle(ConsoleWindow).Bounds;
                int Width = Screen.Width;
                int Height = Screen.Height;

                float ScaleRatio = Height / Engine.Render.RenderHeight;

                int RasteredHeight = (int)(Engine.Render.RenderHeight * ScaleRatio);
                int RasteredWidth = (int)(Engine.Render.RenderWidth * ScaleRatio);

                int XOffset = (int)(Width - RasteredWidth) / 2;
                int YOffset = (int)(Height - RasteredHeight) / 2;

                while (!Render.AbortRender)
                {
                    Rectangle ScreenResCheck = System.Windows.Forms.Screen.FromHandle(ConsoleWindow).Bounds;

                    if (ScreenResCheck != Screen)
                    {
                        Width = ScreenResCheck.Width;
                        Height = ScreenResCheck.Height;

                        XOffset = (int)(Width - (Engine.Render.RenderWidth * ScaleRatio)) / 2;
                        YOffset = (int)(Height - (Engine.Render.RenderHeight * ScaleRatio)) / 2;
                    }

                    unsafe
                    {
                        fixed (byte* ptr = Render.ImageOutData)
                        {
                            using (Bitmap outFrame = new Bitmap(Render.RenderWidth,
                                                                Render.RenderHeight,
                                                                3 * Render.RenderWidth,
                                                                System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                                                new IntPtr(ptr)))
                            {
                                Rectangle imageRect = new Rectangle(XOffset,
                                                                    YOffset,
                                                                    RasteredWidth,
                                                                    RasteredHeight);

                                g.DrawImage(outFrame, imageRect);
                            }
                        }
                    }

                    await Task.Delay(1);
                    //Thread.Sleep(1);
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}