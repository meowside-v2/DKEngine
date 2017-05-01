﻿using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Models
{
    internal class Blocker : GameObject
    {
        public Blocker()
            : base()
        { }

        public Blocker(GameObject Parent)
            : base(Parent)
        { }

        protected override void Initialize()
        {
            this.InitNewComponent<Collider>();
        }
    }
}