using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models.Miscellaneous
{
    class PowerUp : GameObject
    {
        public Action OnPickedUp { get; private set; }

        protected override void Initialize()
        {
            this.Name = nameof(PowerUp);

            switch (Shared.Mechanics.MarioCurrentState)
            {
                case Mario.State.Small:
                    this.TypeName = "Mushroom";
                    OnPickedUp = () => Shared.Mechanics.MarioCurrentState = Mario.State.Super;
                    break;
                case Mario.State.Super:
                    this.TypeName = "Flower";
                    OnPickedUp = () => Shared.Mechanics.MarioCurrentState = Mario.State.Fire;
                    break;

                case Mario.State.Fire:
                case Mario.State.Invincible:
                    this.TypeName = "1-UP";
                    OnPickedUp = () =>
                    {
                        Shared.Mechanics.MarioCurrentState = Mario.State.Invincible;
                        Shared.Mechanics.Lives++;
                    };
                    break;
                default:
                    throw new Exception("JAK");
            }
        }
    }
}
