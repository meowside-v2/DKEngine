using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class CameraController : Script
    {
        GameObject Border;
        GameObject Player;
        Camera TargetCam;
        float PositionX;
        float MaxCameraDistance;

        Vector3 Offset;

        public CameraController(GameObject Parent)
            : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            MaxCameraDistance = Engine.Render.RenderWidth / 3;
            Offset = new Vector3(20, 0, 0);

            Player = GameObject.Find<GameObject>("Player");
            TargetCam = Component.Find<Camera>("Camera");
            TargetCam.Position = new Vector3(0, -180, 0);

            Border = new Blocker();
            Border.Transform.Dimensions = new Vector3(20, Engine.Render.RenderHeight, 0);
            Border.Transform.Position = TargetCam.Position - Offset;
        }

        protected override void Update()
        {
            if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
                Border.Transform.Position = TargetCam.Position - Offset;
            }

            PositionX = Player.Transform.Position.X;
        }
    }
}
