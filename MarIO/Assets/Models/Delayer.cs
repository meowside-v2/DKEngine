using DKEngine.Core;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Delayer : GameObject
    {
        public TimeSpan TimeToWait;
        public Action CalledAction;

        protected override void Initialize()
        {
            this.InitNewScript<DelayScript>();
        }
    }
}
