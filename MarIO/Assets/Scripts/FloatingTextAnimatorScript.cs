using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class FloatingTextAnimatorScript : Script
    {
        float AnimationHeight = 30;
        float AnimationSpeed = 20;

        public FloatingTextAnimatorScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        { }

        protected override void Update()
        {
            if (Shared.FloatingTexts.Count > 0)
            {
                for (int i = 0; i < Shared.FloatingTexts.Count; i++)
                {
                    float StartTextBlockY = Shared.FloatingTextStartPosition[i];
                    TextBlock CurrentTextBlock = Shared.FloatingTexts[i];

                    if (CurrentTextBlock.Transform.Position.Y > StartTextBlockY - AnimationHeight)
                    {
                        CurrentTextBlock.Transform.Position -= new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if(CurrentTextBlock.Transform.Position.Y < StartTextBlockY - AnimationHeight)
                        {
                            Shared.FloatingTextStartPosition.RemoveAt(i);
                            Shared.FloatingTexts.RemoveAt(i);

                            CurrentTextBlock.Destroy();

                            i--;
                        }
                    }
                }
            }
        }
    }
}
