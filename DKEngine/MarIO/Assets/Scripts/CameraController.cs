using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class CameraController : Script
    {
        GameObject Player;
        Camera TargetCam;
        float PositionX;

        public CameraController(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Player = GameObject.Find<GameObject>("Player");
            TargetCam = Component.Find<Camera>("Camera");
            TargetCam.Position = new Vector3(0, -180, 0); ;
        }

        protected override void Update()
        {
            if (Player.Transform.Position.X - TargetCam.Position.X > Engine.Render.RenderWidth / 3)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
            }

            PositionX = Player.Transform.Position.X;
        }
    }
}
