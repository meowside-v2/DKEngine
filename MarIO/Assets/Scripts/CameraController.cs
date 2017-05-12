using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;

namespace MarIO.Assets.Scripts
{
    public class CameraController : Script
    {
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
            TargetCam.Position = new Vector3(0, -160, 0);
        }

        protected override void Update()
        {
            if (Player.Transform.Position.X - TargetCam.Position.X > MaxCameraDistance)
            {
                TargetCam.Position += new Vector3(Player.Transform.Position.X - PositionX, 0, 0);
            }

            if (Player.Transform.Position.X < TargetCam.Position.X)
            {
                Player.Transform.Position = Player.Transform.Position.Add(TargetCam.Position.X - Player.Transform.Position.X, 0, 0);
            }

            PositionX = Player.Transform.Position.X;
        }
    }
}