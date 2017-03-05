using DKBasicEngine_1_0.Core.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKBasicEngine_1_0.Core;

namespace DKEngine
{
    class PlayerControl : CharacterController
    {
        public PlayerControl(GameObject Parent)
            : base(Parent)
        {
            Acceleration = 10;
            MovementSpeed = 100;
            FloatSpeed = 100;
        }
    }
}
