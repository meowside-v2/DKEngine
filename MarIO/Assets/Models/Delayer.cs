using DKEngine.Core;
using MarIO.Assets.Scripts;
using System;

namespace MarIO.Assets.Models
{
    public class Delayer : GameObject
    {
        public TimeSpan TimeToWait;
        public Action CalledAction;

        protected override void Initialize()
        {
            this.InitNewScript<DelayScript>();
        }
    }
}