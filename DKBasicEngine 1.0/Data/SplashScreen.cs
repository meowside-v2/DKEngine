using DKBasicEngine_1_0.Core;
using DKBasicEngine_1_0.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    sealed class SplashScreen : GameObject
    {
        public SplashScreen()
        {
            this.TypeName = "splashScreen";
        }

        public SplashScreen(GameObject Parent)
            : base(Parent)
        {
            this.TypeName = "splashScreen";
        }
    }
}
