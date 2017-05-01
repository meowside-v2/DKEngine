using DKEngine.Core;
using DKEngine.Core.Scripts;

namespace DKEngine_Tester
{
    internal class PlayerControl : CharacterController
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