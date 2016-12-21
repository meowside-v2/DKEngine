using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    class SplashScreen : GameObject
    {
        public SplashScreen(I3Dimensional Parent)
            :base(Parent)
        {
            this.TypeName = "splashScreen";
        }
    }
}
