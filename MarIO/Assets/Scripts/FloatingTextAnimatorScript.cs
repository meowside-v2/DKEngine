using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;

namespace MarIO.Assets.Scripts
{
    internal class FloatingTextAnimatorScript : Script
    {
        private float AnimationHeight = 30;
        private float AnimationSpeed = 20;

        public FloatingTextAnimatorScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        { }

        protected override void Update()
        {
            if (Shared.AnimatedWorldReferences.FloatingTexts.Count > 0)
            {
                for (int i = 0; i < Shared.AnimatedWorldReferences.FloatingTexts.Count; i++)
                {
                    float StartTextBlockY = Shared.AnimatedWorldReferences.FloatingTextStartPosition[i];
                    TextBlock CurrentTextBlock = Shared.AnimatedWorldReferences.FloatingTexts[i];

                    if (CurrentTextBlock.Transform.Position.Y > StartTextBlockY - AnimationHeight)
                    {
                        CurrentTextBlock.Transform.Position -= new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if (CurrentTextBlock.Transform.Position.Y < StartTextBlockY - AnimationHeight)
                        {
                            Shared.AnimatedWorldReferences.FloatingTextStartPosition.RemoveAt(i);
                            Shared.AnimatedWorldReferences.FloatingTexts.RemoveAt(i);

                            CurrentTextBlock.Destroy();

                            i--;
                        }
                    }
                }
            }
        }
    }
}