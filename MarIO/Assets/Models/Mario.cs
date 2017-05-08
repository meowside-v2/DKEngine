using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System.Drawing;
using static DKEngine.Core.Components.Transform;

namespace MarIO.Assets.Models
{
    internal class Mario : AnimatedObject
    {
        public bool KilledEnemy = false;
        public Trigger LeftTrigger { get; private set; }
        public Trigger RightTrigger { get; private set; }
        public Trigger TopTrigger { get; private set; }
        public Trigger BottomTrigger { get; private set; }

        public bool InitCharacterController { get; set; }
        public bool InitCameraController { get; set; }

        public State CurrentState { get; set; }
        public Movement CurrentMovement { get; set; }
        public Direction PipeEnteredInDirection { get { return EnteredPipe.PipeEnterDirection; } }
        public Block EnteredPipe { get; set; }

        public WorldChangeManagerScript WorldManager { get; set; }

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
        }

        public enum Movement
        {
            Standing,
            Crouching
        }

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

            if(InitCharacterController)
                this.InitNewScript<CharacterController>();

            if(InitCameraController)
                this.InitNewScript<CameraController>();

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

            this.InitNewComponent<SoundSource>();

            WorldManager = Behavior.Find<WorldChangeManagerScript>("worldManager");
        }

        public void PipeEnter(Block Pipe)
        {
            ChangeState = true;
            EnteredPipe = Pipe;
        }
    }
}