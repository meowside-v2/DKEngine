using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            public static readonly int RenderWidth = 640;
            public static readonly int RenderHeight = 360;

            internal static byte[] imageBuffer;
            internal static byte[] imageBufferKey;
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

        private static Stopwatch _deltaT;
        internal static Camera _baseCam;

        internal static List<ICore> ToUpdate;
        internal static List<ICore> ToStart;
        internal static List<IGraphics> ToRender;
        internal static List<IControl> PageControls;
        internal static int UpdateStartTime;

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
                    
                    Render.imageBuffer = new byte[3 * Render.RenderWidth * Render.RenderHeight];
                    Render.imageBufferKey = new byte[Render.RenderWidth * Render.RenderHeight];

                    _deltaT = Stopwatch.StartNew();
                    ToStart = new List<ICore>();
                    ToUpdate = new List<ICore>();
                    ToRender = new List<IGraphics>();
                    PageControls = new List<IControl>();

                    BackgroundWorks = new Thread(() => Update());
                    BackgroundWorks.Start();

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
        
        private static void Update()
        {

/*#if DEBUG
            Stopwatch t = Stopwatch.StartNew();
#endif*/

            while (true)
            {
/*#if DEBUG
                t.Restart();
                long _startUpdate = t.ElapsedTicks;
#endif*/

                UpdateStartTime = Environment.TickCount;
                
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

                while(ToStart.Count > 0)
                {
                    ToStart[0].Start();
                    ToStart.Remove(ToStart[0]);
                }
                
                for(int i = 0; i < controlReference.Count; i++)
                {
                    bool result = Page.FocusSelection == controlReference.FindIndex(obj => ReferenceEquals(obj, controlReference[i]));

                    if (controlReference[i].IsFocused != result)
                        controlReference[i].IsFocused = result;
                }

                List<ICore> tempReference = reference.Where(obj => obj is I3Dimensional).ToList();

                for(int i = 0; i < tempReference.Count; i++)
                    if (!((I3Dimensional)tempReference[i]).IsInView())
                        reference.Remove(tempReference[i]);

                _deltaT?.Stop();

                for (int i = 0; i < reference.Count; i++)
                    reference[i].Update();

/*#if DEBUG
                long _endUpdate = t.ElapsedTicks;
#endif*/

                _deltaT?.Restart();

/*#if DEBUG
                long _startRender = t.ElapsedTicks;
#endif*/

                _baseCam?.BufferImage();

/*#if DEBUG
                long _endRender = t.ElapsedTicks;
                Debug.WriteLine($"Update: {_endUpdate - _startUpdate}\nRender: {_endRender - _startRender}\nAll: {_endRender - _startUpdate}");
#endif*/
            }
        }

        public static void PageChange(IPage Page)
        {
            if (_isInitialised)
            {
                Engine.Page = Page;
                
                Engine.PageControls = Page.PageControls.ToList();

                for (int i = 0; i < PageControls.Count - 1; i++)
                {
                    for (int j = 0; j < PageControls.Count - 1; j++)
                    {
                        if (((I3Dimensional)PageControls[j]).X > ((I3Dimensional)PageControls[j + 1]).X)
                        {

                            var temp = PageControls[j];
                            PageControls[j] = PageControls[j + 1];
                            PageControls[j + 1] = temp;
                        }
                    }
                }

                for (int i = 0; i < PageControls.Count - 1; i++)
                {
                    for (int j = 0; j < PageControls.Count - 1; j++)
                    {
                        if (((I3Dimensional)PageControls[j]).X == ((I3Dimensional)PageControls[j + 1]).X)
                            if (((I3Dimensional)PageControls[j]).Y < ((I3Dimensional)PageControls[j + 1]).Y)
                            {
                                var temp = PageControls[j];
                                PageControls[j] = PageControls[j + 1];
                                PageControls[j + 1] = temp;
                            }
                    }
                }
            }
            else
                throw new Exception("Engine not initialised \n Can't change page");
        }

        public static void Pause()
        {
            if (_deltaT.IsRunning) _deltaT?.Stop();
            if (BackgroundWorks.IsAlive) BackgroundWorks?.Abort();
        }

        public static void Resume()
        {
            if(!_deltaT.IsRunning) _deltaT?.Start();
            if(!BackgroundWorks.IsAlive) BackgroundWorks?.Start();
        }

        internal static void SplashScreen()
        {
            if (!_isInitialised)
            {
                Camera splashScreenCam = new Camera();
                SplashScreen splash = new SplashScreen(null);

                splashScreenCam.Init(0, 0);

                SpinWait.SpinUntil(() => splash.Animator.NumberOfPlays >= 1);

                splash.Destroy();
                splashScreenCam.Destroy();
            }
        }
    }
}
