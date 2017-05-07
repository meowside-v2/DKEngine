using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using MarIO.Assets.Models.Miscellaneous;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class FloatingCoinAnimatorScript : Script
    {
        float AnimationHeight = 60;
        float AnimationSpeed = 20;

        public FloatingCoinAnimatorScript(GameObject Parent)
            :base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        { }

        protected override void Update()
        {
            if(Shared.FloatingCoins.Count > 0)
            {
                for (int i = 0; i < Shared.FloatingCoins.Count; i++)
                {
                    Coin currentCoin = Shared.FloatingCoins[i];
                    float currentCoinStartPosition = Shared.FloatingCoinsStartPosition[i];

                    if(currentCoin.Transform.Position.Y > currentCoinStartPosition - AnimationHeight)
                    {
                        currentCoin.Transform.Position -= new Vector3(0, Engine.DeltaTime * AnimationSpeed, 0);

                        if(currentCoin.Transform.Position.Y <= currentCoinStartPosition - AnimationHeight)
                        {
                            currentCoin.Destroy();

                            Shared.FloatingCoins.RemoveAt(i);
                            Shared.FloatingCoinsStartPosition.RemoveAt(i);

                            i--;
                        }
                    }
                }
            }
        }
    }
}
