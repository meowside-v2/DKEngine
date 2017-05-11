using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DKEngine.Data
{
    class SplashScreenScene : Scene
    {
        internal SplashScreen Splash;

        public override void Init()
        {
            Splash = new SplashScreen();
            Splash.Transform.Position = new Vector3(-32, 0, 0);
            Splash.Transform.Scale = new Vector3(0.5f, 0.5f, 0);
            Camera splashScreenCam = new Camera();
        }
        
        public override void Unload()
        { }
    }
}
