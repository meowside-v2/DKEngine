using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Border : GameObject
    {
        protected override void Init()
        {
            this.Transform.Dimensions = new Vector3(20, Engine.Render.RenderHeight, 0);

            this.InitNewComponent<Collider>();
        }
    }
}
