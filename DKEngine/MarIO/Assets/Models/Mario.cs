using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Mario : GameObject
    {
        protected override void Init()
        {
            this.Name = "Player";
            this.TypeName = "mario";
            this.Transform.Position = new Vector3(50, -10, 0);
            this.InitNewComponent<PlayerControl>();
        }
    }
}
