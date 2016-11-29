using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            private static int _width = 640;
            private static int _height = 360;

            public static int RenderWidth { get { return _width; } }
            public static int RenderHeight { get { return _height; } }

            internal static byte[] imageBuffer;
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
        internal static List<IControl> PageControls;
        internal static int UpdateStartTime;

        internal static IPage Page;

        public static double deltaTime { get { return _deltaT.Elapsed.TotalSeconds; } }
        

        public static void Init()
        {
            if (!_isInitialised)
            {
                try
                {
                    Console.CursorVisible = false;
                    Console.SetOut(TextWriter.Null);
                    Console.SetIn(TextReader.Null);

                    Database.InitDatabase();
                    WindowControl.WindowInit();

                    Render.imageBuffer = new byte[4 * Render.RenderHeight * Render.RenderWidth];

                    _deltaT = Stopwatch.StartNew();
                    ToUpdate = new List<ICore>();
                    PageControls = new List<IControl>();

                    BackgroundWorks = new Thread(() => Update());
                    BackgroundWorks.Start();

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
            while (true)
            {
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
                
                foreach(IControl c in controlReference)
                {
                    bool result = Page.FocusSelection == controlReference.FindIndex(obj => ReferenceEquals(obj, c));

                    if(c.IsFocused != result)
                        c.IsFocused = result;
                }
                
                foreach (I3Dimensional obj in reference.Where(obj => obj is I3Dimensional).ToList())
                {
                    if (!obj.IsInView())
                        reference.Remove((ICore)obj);
                }

                _deltaT?.Stop();

                foreach (ICore g in reference)
                {
                    g.Update();
                }

                _deltaT?.Restart();
                
                _baseCam?.BufferImage();
            }
        }

        public static void PageChange(IPage Page)
        {
            if (_isInitialised)
            {
                Engine.Page = Page;

                lock (PageControls)
                {
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
    }
}
