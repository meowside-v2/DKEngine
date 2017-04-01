using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine
{
    sealed class SplashScreen : GameObject
    {
        public SplashScreen()
        {
            this.TypeName = "splashScreen";
            this.InitNewComponent<Animator>();
            //this.Animator = new Core.Components.Animator(this);
        }

        public SplashScreen(GameObject Parent)
            : base(Parent)
        {
            this.TypeName = "splashScreen";
            this.InitNewComponent<Animator>();
            //this.Animator = new Core.Components.Animator(this);
        }
    }
}
