using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Models
{
    internal class Mario : AnimatedObject
    {
        public bool KilledEnemy = false;

        public Mario()
        { }

        public Mario(GameObject Parent)
            : base(Parent)
        { }

        public enum State
        {
            Small,
            Super,
            Fire,
            Invincible
        };

        protected override void Initialize()
        {
            this.Name = "Player";

            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation(Shared.MARIO_IDLE_LEFT, Shared.MARIO_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_IDLE_RIGHT, Shared.MARIO_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_JUMP_LEFT, Shared.MARIO_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_JUMP_RIGHT, Shared.MARIO_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_MOVE_LEFT, Shared.MARIO_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_MOVE_RIGHT, Shared.MARIO_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.MARIO_DEAD, Shared.MARIO_DEAD_MAT);

            this.Animator.Play(Shared.MARIO_IDLE_RIGHT);

            this.InitNewScript<CharacterController>();

            Trigger bottom = new Trigger(this)
            {
                Name = "Bottom_Trigger"
            };
            bottom.Transform.Position += new Vector3(0, 16, 0);
            bottom.Transform.Dimensions = new Vector3(14, 1, 0);
            bottom.InitNewScript<BottomMarioChecker>();
            //bottom.Model = new Material(Color.Black, bottom);

            Trigger left = new Trigger(this)
            {
                Name = "Left_Trigger"
            };
            left.Transform.Position += new Vector3(-1, 0, 0);
            left.Transform.Dimensions = new Vector3(1, 16, 0);
            left.InitNewScript<LeftMarioChecker>();
            //left.Model = new Material(Color.Black, left);

            Trigger right = new Trigger(this)
            {
                Name = "Right_Trigger"
            };
            right.Transform.Position += new Vector3(14, 0, 0);
            right.Transform.Dimensions = new Vector3(1, 16, 0);
            right.InitNewScript<RightMarioChecker>();
            //right.Model = new Material(Color.Black, right);

            Trigger top = new Trigger(this)
            {
                Name = "Top_Trigger"
            };
            top.Transform.Position += new Vector3(0, -1, 0);
            top.Transform.Dimensions = new Vector3(14, 1, 0);
            top.InitNewScript<TopMarioChecker>();
            //top.Model = new Material(Color.Black, top);
        }
    }
}