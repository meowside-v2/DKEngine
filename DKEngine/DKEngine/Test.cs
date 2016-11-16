using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine
{
    class Test : GameObject
    {
        private double Speed = 1f;

        public override void Update()
        {
            this.X += Speed * Engine.deltaTime;
        }
    }
}
