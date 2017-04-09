using DKEngine.Core.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core;

namespace DKEngine_Tester
{
    class PlayerControl : CharacterController
    {
        public PlayerControl(GameObject Parent)
            : base(Parent)
        {
            Acceleration = 10f;
            MovementSpeed = 100f;
            FloatSpeed = 100f;
        }
    }
}
