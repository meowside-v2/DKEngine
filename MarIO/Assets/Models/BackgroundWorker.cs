﻿using DKEngine.Core;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class BackgroundWorker : GameObject
    {
        protected override void Initialize()
        {
            this.InitNewScript<BlockAnimatorScript>();
            this.InitNewScript<FloatingCoinAnimatorScript>();
            this.InitNewScript<FloatingTextAnimatorScript>();
            this.InitNewScript<SpecialBlocksUpdateScript>();
            this.InitNewScript<WorldChangeManagerScript>();
        }
    }
}
