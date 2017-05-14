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

using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.Ext;
using DKEngine.Core.UI;
using DKEngine.Data;
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
    /// <summary>
    /// Engine class
    /// </summary>
    public static class Engine
    {
        /// <summary>
        /// Sound subclass
        /// </summary>
        public static class Sound
        {
            /// <summary>
            /// Enables sound
            /// </summary>
            public static bool IsSoundEnabled = true;

            /// <summary>
            /// Sets volume on sound inicialization
            /// </summary>
            public static float SoundVolume = 1f;

            internal readonly static SoundPlayer Instance = new SoundPlayer();
        }

        /// <summary>
        /// Render subclass
        /// </summary>
        public static class Render
        {
            /// <summary>
            /// Sets resolution scale in %
            /// </summary>
            public const int ResolutionScale = 50;

            /// <summary>
            /// The resolution ratio
            /// </summary>
            public const float ResolutionRatio = ResolutionScale / 100f;

            /// <summary>
            /// The rendered image width
            /// </summary>
            public const int RenderWidth = (int)(640 * ResolutionRatio);

            /// <summary>
            /// The rendered image height
            /// </summary>
            public const int RenderHeight = (int)(480 * ResolutionRatio);

            internal const int ImageBufferSize = 3 * RenderWidth * RenderHeight;
            internal const int ImageKeyBufferSize = RenderWidth * RenderHeight;

            internal static byte[] imageBuffer;
            internal static byte[] imageBufferKey;
            internal static byte[] ImageOutData;

            internal static bool AbortRender = false;

            internal const int Limiter = 1000;
        }

        /// <summary>
        /// Input subclass
        /// </summary>
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

            /// <summary>
            /// Determines whether [is key pressed] [the specified key].
            /// </summary>
            /// <param name="key">The key</param>
            /// <returns>
            ///   <c>true</c> if [is key pressed] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public static bool IsKeyPressed(ConsoleKey key)
            {
                return KeysPressed[(short)key];
            }

            /// <summary>
            /// Determines whether [is key down] [the specified key].
            /// </summary>
            /// <param name="key">The key</param>
            /// <returns>
            ///   <c>true</c> if [is key down] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public static bool IsKeyDown(ConsoleKey key)
            {
                return KeysDown[(short)key];
            }

            /// <summary>
            /// Determines whether [is key up] [the specified key].
            /// </summary>
            /// <param name="key">The key</param>
            /// <returns>
            ///   <c>true</c> if [is key up] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public static bool IsKeyUp(ConsoleKey key)
            {
                return KeysUp[(short)key];
            }

            /// <summary>
            /// Determines whether [is key released] [the specified key].
            /// </summary>
            /// <param name="key">The key</param>
            /// <returns>
            ///   <c>true</c> if [is key released] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public static bool IsKeyReleased(ConsoleKey key)
            {
                return KeysReleased[(short)key];
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

        private static bool _IsInitialised = false;

        private static Thread BackgroundWorks;
        private static TextBlock FpsMeter;
        private static Stopwatch DeltaT;
        internal static Camera BaseCam;

        internal static Scene CurrentScene { get; set; }
        internal static Scene LoadingScene { get; set; }

        internal static Type LoadingSceneType { get; set; }

        internal static List<GameObject> RenderObjects;

        private static float deltaT = 0;
        public static float DeltaTime { get { return deltaT; } }

        private static TimeSpan lastUpdated = new TimeSpan();
        public static TimeSpan LastUpdated { get { return lastUpdated; } }

        public static string SceneName { get { return Engine.LoadingScene != null ? Engine.LoadingScene.Name : ""; } }

        private static readonly TimeSpan _firstTimeLoadDelay = new TimeSpan(0, 0, 1);
        private static TimeSpan FirstTimeLoadDelay = new TimeSpan();
        private static bool FirstTimeLoaded = true;

        internal static event EngineHandler UpdateEvent;

        internal delegate void EngineHandler();

        /// <summary>
        /// Sets engine to work.
        /// </summary>
        /// <exception cref="System.Exception">
        /// Engine initialisation failed\n" + e
        /// or
        /// Engine is being initialised second time
        /// </exception>
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

        public static void LoadSceneToMemory<T>(object[] argsPreLoad = null, object[] argsPostLoad = null)
            where T : Scene
        {
            Engine.LoadingScene = (T)Activator.CreateInstance(typeof(T));
            Engine.LoadingScene.argsPreLoad = argsPreLoad;
            Engine.LoadingScene.argsPostLoad = argsPostLoad;
            Engine.LoadingScene.Set(argsPreLoad);
            Engine.LoadingScene.Init();

            Database.AddScene(Engine.LoadingScene);
        }

        /// <summary>
        /// Loads the scene to memory.
        /// </summary>
        /// <typeparam name="T">Scene</typeparam>
        /// <param name="argsPreLoad">The arguments pre load.</param>
        /// <param name="argsPostLoad">The arguments post load.</param>
        public static void LoadScene<T>(object[] argsPreLoad = null, object[] argsPostLoad = null) where T : Scene
        {
            LoadingSceneType = typeof(T);
            LoadScene(LoadingSceneType, argsPreLoad, argsPostLoad);
        }

        public static void LoadScene(Type scene, object[] argsPreLoad = null, object[] argsPostLoad = null)
        {
            if (!scene.IsSubclassOf(typeof(Scene)))
                throw new Exception($"Provided type {scene} is not subclass of Scene");

            Engine.LoadingScene = (Scene)Activator.CreateInstance(LoadingSceneType);

            Engine.LoadingScene.argsPreLoad = argsPreLoad;
            Engine.LoadingScene.argsPostLoad = argsPostLoad;

            Engine.LoadingScene.Set(argsPreLoad);
            Engine.LoadingScene.Init();

            UnregisterScene();
            RegisterScene(Engine.LoadingScene, argsPostLoad);
        }

        /// <summary>
        /// Reloads the scene.
        /// </summary>
        /// <param name="Name">The name of scene</param>
        public static void ReloadScene(string Name, object[] argsPreLoad = null)
        {
            Database.RewriteWorld(Name, argsPreLoad);
        }

        /// <summary>
        /// Changes the scene.
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Reload">if set to <c>true</c> [reload]</param>
        /// <param name="args">The arguments</param>
        public static void ChangeScene(string Name, bool Reload = false, object[] argsPreLoad = null, object[] argsPostLoad = null)
        {
            UnregisterScene();
            if (Reload)
            {
                ReloadScene(Name, argsPreLoad);

                if (argsPostLoad != null)
                    Database.GetScene(Name).argsPostLoad = argsPostLoad;
            }

            RegisterScene(Database.GetScene(Name), argsPostLoad);
        }

        private static void UnregisterScene()
        {
            try
            {
                Engine.CurrentScene.Unload();

                foreach (var item in CurrentScene.AllBehaviors)
                {
                    try
                    {
                        UpdateEvent -= item.UpdateHandle;
                    }
                    catch { }
                }

                while (CurrentScene.GameObjectsAddedToRender.Count > 0)
                {
                    GameObject tmp = CurrentScene.GameObjectsAddedToRender.Pop();
                    if (Engine.RenderObjects.Contains(tmp))
                        Engine.RenderObjects.Remove(tmp);

                    CurrentScene.GameObjectsToAddToRender.Push(tmp);
                }
            }
            catch { }
        }

        private static void RegisterScene(Scene source, object[] args)
        {
            Engine.LoadingScene = source;

            if (args != null)
            {
                source.argsPostLoad = args;
                source.Set(args);
            }
            else if (source.argsPostLoad != null)
                source.Set(source.argsPostLoad);

            foreach (var item in source.AllBehaviors)
            {
                try
                {
                    UpdateEvent += item.UpdateHandle;
                }
                catch { }
            }

            Engine.BaseCam = Engine.LoadingScene.BaseCamera;
            Engine.CurrentScene = source;
        }

        public static void ReloadCurrentScene()
        {
            UnregisterScene();
            LoadScene(LoadingSceneType);
        }

        private static void SplashScreen()
        {
            if (!_IsInitialised)
            {
                Engine.LoadScene<SplashScreenScene>();

                SpinWait.SpinUntil(() => ((SplashScreenScene)Engine.CurrentScene).Splash.Animator.NumberOfPlays >= 1);
            }
        }

        private static void Update()
        {
            Task imageRender = Task.Factory.StartNew(RenderImage);

            int NumberOfFrames = 0;
            TimeSpan timeOut = new TimeSpan(0, 0, 0, 0, 500);
            Stopwatch time = Stopwatch.StartNew();
            Stopwatch fpsLimiter = Stopwatch.StartNew();

            while (true)
            {
                Input.CheckForKeys();

                lastUpdated += DeltaT.Elapsed;
                deltaT = (float)DeltaT.Elapsed.TotalSeconds;
                DeltaT?.Restart();

                if (!FirstTimeLoaded)
                {
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

                    while (Engine.CurrentScene?.DestroyObjectAwaitList.Count > 0)
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

                    if (Engine.CurrentScene != null)
                    {
                        List<Collider> VisibleTriggers = Engine.CurrentScene?.AllGameObjectsColliders.Where(obj => obj.IsTrigger).ToList();
                        List<Collider> VisibleColliders = Engine.CurrentScene?.AllGameObjectsColliders.Where(obj => !obj.IsTrigger).ToList();
                        int ColliderCount = VisibleTriggers.Count;
                        for (int i = 0; i < ColliderCount; i++)
                            VisibleTriggers[i]?.TriggerCheck(VisibleColliders);
                    }

                    Engine.CurrentScene?.BaseCamera?.BufferImage(reference);

                    Buffer.BlockCopy(Render.imageBuffer, 0, Render.ImageOutData, 0, Render.ImageBufferSize);
                }
                else
                {
                    FirstTimeLoadDelay += new TimeSpan(0, 0, 0, 0, (int)(DeltaTime * 1000));

                    if (FirstTimeLoadDelay > _firstTimeLoadDelay)
                    {
                        FirstTimeLoaded = false;
                        FirstTimeLoadDelay = new TimeSpan();
                    }
                }

                NumberOfFrames++;

                Vsync(Render.Limiter, (int)fpsLimiter.ElapsedMilliseconds);
                fpsLimiter.Restart();

                if (time.ElapsedMilliseconds > timeOut.TotalMilliseconds)
                {
                    long t = NumberOfFrames * 1000 / time.ElapsedMilliseconds;
                    FpsMeter.Text = t.ToString();
                    /*#if DEBUG
                                        Debug.WriteLine(t);
                    #endif*/
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
                }
            }
        }

        private static void Vsync(int TargetFrameRate, int ImageRenderDelay)
        {
            int targetDelay = 1000 / TargetFrameRate;

            if (ImageRenderDelay < targetDelay)
            {
                Thread.Sleep(targetDelay - ImageRenderDelay);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
    }
}