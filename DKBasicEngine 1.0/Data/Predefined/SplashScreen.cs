using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    class SplashScreen : GameObject
    {
        public SplashScreen(Scene ToParentToAdd, I3Dimensional Parent)
            : base(ToParentToAdd, Parent)
        {
            this.TypeName = "splashScreen";
        }
    }
}
