using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models.Miscellaneous
{
    public class PowerUp : GameObject
    {
        public Mario PlayerReference;

        public enum PowerUpType
        {
            Mushroom,
            Flower,
            Star
        }

        public Action OnPickedUp { get; private set; }
        public PowerUpType Type { get; private set; }

        protected override void Initialize()
        {
            this.Name = nameof(PowerUp);
            
            this.InitNewComponent<Collider>();
            this.Collider.Area = new RectangleF(0, 0, 16, 16);
            this.Collider.Enabled = false;

            this.InitNewScript<PowerUpScript>();
            
            switch (Shared.Mechanics.MarioCurrentState)
            {
                case Mario.State.Small:
                    this.TypeName = "mushroom";
                    Type = PowerUpType.Mushroom;
                    OnPickedUp = () =>
                    {
                        Shared.Mechanics.GameScore += Shared.Mechanics.MUSHROOM_SCORE;
                        TextBlock FloatingText = new TextBlock()
                        {
                            Text = string.Format("{0}", Shared.Mechanics.MUSHROOM_SCORE),
                            TextShadow = true
                        };
                        FloatingText.Transform.Position = this.Transform.Position;
                        FloatingText.Transform.Dimensions = new Vector3(20, 6, 0);
                        FloatingText.AddAsFloatingText();
                        PlayerReference.CurrentState = Mario.State.Super;

                        OnPickedUp = null;

                        this.Destroy();
                    };
                    break;
                case Mario.State.Super:
                    this.TypeName = "flower";
                    Type = PowerUpType.Flower;
                    this.InitNewComponent<Animator>();
                    this.Animator.AddAnimation("default", "flower");
                    this.Animator.Play("default");
                    OnPickedUp = () =>
                    {
                        Shared.Mechanics.GameScore += Shared.Mechanics.FLOWER_SCORE;
                        TextBlock FloatingText = new TextBlock()
                        {
                            Text = string.Format("{0}", Shared.Mechanics.FLOWER_SCORE),
                            TextShadow = true
                        };
                        FloatingText.Transform.Position = this.Transform.Position;
                        FloatingText.Transform.Dimensions = new Vector3(20, 6, 0);
                        FloatingText.AddAsFloatingText();

                        PlayerReference.CurrentState = Mario.State.Fire;

                        OnPickedUp = null;

                        this.Destroy();
                    }; 
                    this.Collider.IsTrigger = true;
                    break;

                case Mario.State.Fire:
                case Mario.State.Invincible:
                    this.TypeName = "1-UP";
                    Type = PowerUpType.Star;
                    this.InitNewComponent<Animator>();
                    this.Animator.AddAnimation("default", "star");
                    this.Animator.Play("default");
                    OnPickedUp = () =>
                    {
                        Shared.Mechanics.GameScore += Shared.Mechanics.STAR_SCORE;
                        TextBlock FloatingText = new TextBlock()
                        {
                            Text = string.Format("{0}", Shared.Mechanics.STAR_SCORE),
                            TextShadow = true
                        };
                        FloatingText.Transform.Position = this.Transform.Position;
                        FloatingText.Transform.Dimensions = new Vector3(20, 6, 0);
                        FloatingText.AddAsFloatingText();

                        PlayerReference.CurrentState = Mario.State.Invincible;
                        Shared.Mechanics.Lives++;

                        OnPickedUp = null;

                        this.Destroy();
                    };
                    break;
                default:
                    throw new Exception("JAK");
            }
        }
    }
}
