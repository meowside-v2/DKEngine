using DKEngine.Core;
using MarIO.Assets.Scripts;
using System;

namespace MarIO.Assets.Models
{
    public class Delayer : GameObject
    {
        public TimeSpan TimeToWait;
        public Action CalledAction;

        public Delayer()
        {
            Name = nameof(Delayer);
        }

        protected override void Initialize()
        {
            this.InitNewScript<DelayScript>();
        }
    }
}