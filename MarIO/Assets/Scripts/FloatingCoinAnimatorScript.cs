using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models.Miscellaneous;

namespace MarIO.Assets.Scripts
{
    public class FloatingCoinAnimatorScript : Script
    {
        private float AnimationHeight = 60;
        private float AnimationSpeed = 20;

        public FloatingCoinAnimatorScript(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        { }

        protected override void Update()
        {
            if (Shared.AnimatedWorldReferences.FloatingCoins.Count > 0)
            {
                for (int i = 0; i < Shared.AnimatedWorldReferences.FloatingCoins.Count; i++)
                {
                    Coin currentCoin = Shared.AnimatedWorldReferences.FloatingCoins[i];
                    float currentCoinStartPosition = Shared.AnimatedWorldReferences.FloatingCoinsStartPosition[i];

                    if (currentCoin.Transform.Position.Y > currentCoinStartPosition - AnimationHeight)
                    {
                        currentCoin.Transform.Position -= new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if (currentCoin.Transform.Position.Y <= currentCoinStartPosition - AnimationHeight)
                        {
                            currentCoin.Destroy();

                            Shared.AnimatedWorldReferences.FloatingCoins.RemoveAt(i);
                            Shared.AnimatedWorldReferences.FloatingCoinsStartPosition.RemoveAt(i);

                            i--;
                        }
                    }
                }
            }
        }
    }
}