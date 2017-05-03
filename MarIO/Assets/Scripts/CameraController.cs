using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Scripts
{
    internal class CameraController : Script
    {
        //GameObject Border;
        private GameObject Player;

        private Camera TargetCam;
        private float PositionX;
        private float MaxCameraDistance;

        private Vector3 Offset;

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

            /*Border = new Blocker();
            Border.Transform.Dimensions = new Vector3(20, Engine.Render.RenderHeight, 0);
            Border.Transform.Position = TargetCam.Position - Offset;*/
        }

        protected override void Update()
        {
            if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
            }

            if (Player.Transform.Position.X < TargetCam.Position.X)
            {
                Player.Transform.Position = Player.Transform.Position.Add(TargetCam.Position.X - Player.Transform.Position.X, 0, 0);
            }

            PositionX = Player.Transform.Position.X;

            /*if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position.X += Player.Transform.Position.X - PositionX;
                //Border.Transform.Position = TargetCam.Position - Offset;
            }

            if (Player.Transform.Position.X < TargetCam.Position.X)
            {
                Player.Transform.Position += new Vector3(TargetCam.Position.X - Player.Transform.Position.X, 0, 0);
            }

            PositionX = Player.Transform.Position.X;*/
        }
    }
}