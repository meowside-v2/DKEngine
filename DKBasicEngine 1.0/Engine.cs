using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        }

        private static bool _isInitialised = false;

        private static Thread BackgroundWorks;

        private static Stopwatch _deltaT;
        internal static Camera _baseCam;

        internal static List<GameObject> ToUpdate;
        internal static int UpdateStartTime;

        public static double deltaTime { get { return _deltaT.Elapsed.TotalSeconds; } }
        

        public static void Init()
        {
            if (!_isInitialised)
            {
                try
                {
                    Database.InitDatabase();
                    WindowControl.WindowInit();

                    _deltaT = Stopwatch.StartNew();
                    ToUpdate = new List<GameObject>();

                    BackgroundWorks = new Thread(() => Update());
                    BackgroundWorks.Start();
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
                
                List<GameObject> reference;

                lock (ToUpdate)
                {
                    reference = ToUpdate.ToList();
                }

                _deltaT.Stop();

                foreach (GameObject g in reference)
                {
                    g.Update();
                }

                _deltaT.Restart();

                _baseCam?.BufferImage();
            }
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
