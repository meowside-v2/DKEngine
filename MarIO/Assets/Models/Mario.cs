using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System.Drawing;

namespace MarIO.Assets.Models
{
    internal class Mario : AnimatedObject
    {
        public bool KilledEnemy = false;
        public Trigger LeftTrigger;
        public Trigger RightTrigger;
        public Trigger TopTrigger;
        public Trigger BottomTrigger;

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

            BottomTrigger = new Trigger(this)
            {
                Name = "Bottom_Trigger"
            };
            BottomTrigger.Transform.Position += new Vector3(0.5f, 16, 0);
            BottomTrigger.Transform.Dimensions = new Vector3(14, 0.5f, 0);
            BottomTrigger.InitNewScript<BottomMarioChecker>();
            BottomTrigger.Model = new Material(Color.Black, BottomTrigger);

            LeftTrigger = new Trigger(this)
            {
                Name = "Left_Trigger"
            };
            LeftTrigger.Transform.Position += new Vector3(-1, 0, 0);
            LeftTrigger.Transform.Dimensions = new Vector3(1, 16, 0);
            LeftTrigger.InitNewScript<LeftMarioChecker>();
            LeftTrigger.Model = new Material(Color.Black, LeftTrigger);

            RightTrigger = new Trigger(this)
            {
                Name = "Right_Trigger"
            };
            RightTrigger.Transform.Position += new Vector3(14, 0, 0);
            RightTrigger.Transform.Dimensions = new Vector3(1, 16, 0);
            RightTrigger.InitNewScript<RightMarioChecker>();
            RightTrigger.Model = new Material(Color.Black, RightTrigger);

            TopTrigger = new Trigger(this)
            {
                Name = "Top_Trigger"
            };
            TopTrigger.Transform.Position += new Vector3(0.5f, -0.5f, 0);
            TopTrigger.Transform.Dimensions = new Vector3(14, 0.5f, 0);
            TopTrigger.InitNewScript<TopMarioChecker>();
            TopTrigger.Model = new Material(Color.Black, TopTrigger);
        }
    }
}