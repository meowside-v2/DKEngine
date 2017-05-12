using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System.Drawing;
using static DKEngine.Core.Components.Transform;
using static MarIO.Shared.Assets.Animations;

namespace MarIO.Assets.Models
{
    public class Mario : AnimatedObject
    {
        public bool KilledEnemy = false;
        public Trigger LeftTrigger { get; private set; }
        public Trigger RightTrigger { get; private set; }
        public Trigger TopTrigger { get; private set; }
        public Trigger BottomTrigger { get; private set; }

        public bool InitCharacterController { get; set; }
        public bool InitCameraController { get; set; }
        public bool InitCollider { get; set; }

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
            
            CurrentState = Shared.Mechanics.MarioCurrentState;

            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_IDLE_LEFT, Shared.Assets.Animations.MARIO_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_IDLE_RIGHT, Shared.Assets.Animations.MARIO_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_JUMP_LEFT, Shared.Assets.Animations.MARIO_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_JUMP_RIGHT, Shared.Assets.Animations.MARIO_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_MOVE_LEFT, Shared.Assets.Animations.MARIO_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_MOVE_RIGHT, Shared.Assets.Animations.MARIO_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_DEAD, Shared.Assets.Animations.MARIO_DEAD_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_CROUCHING_LEFT, Shared.Assets.Animations.MARIO_CROUCHING_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_CROUCHING_RIGHT, Shared.Assets.Animations.MARIO_CROUCHING_RIGHT_MAT);

            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_IDLE_LEFT, Shared.Assets.Animations.MARIO_SUPER_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_IDLE_RIGHT, Shared.Assets.Animations.MARIO_SUPER_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_JUMP_LEFT, Shared.Assets.Animations.MARIO_SUPER_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_JUMP_RIGHT, Shared.Assets.Animations.MARIO_SUPER_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_MOVE_LEFT, Shared.Assets.Animations.MARIO_SUPER_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_MOVE_RIGHT, Shared.Assets.Animations.MARIO_SUPER_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_POWERUP_LEFT, Shared.Assets.Animations.MARIO_SUPER_POWERUP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_POWERUP_RIGHT, Shared.Assets.Animations.MARIO_SUPER_POWERUP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_CROUCHING_RIGHT, Shared.Assets.Animations.MARIO_SUPER_CROUCHING_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_SUPER_CROUCHING_LEFT, Shared.Assets.Animations.MARIO_SUPER_CROUCHING_LEFT_MAT);

            /*this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_IDLE_LEFT, Shared.Assets.Animations.MARIO_FIRE_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_IDLE_RIGHT, Shared.Assets.Animations.MARIO_FIRE_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_JUMP_LEFT, Shared.Assets.Animations.MARIO_FIRE_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_JUMP_RIGHT, Shared.Assets.Animations.MARIO_FIRE_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_MOVE_LEFT, Shared.Assets.Animations.MARIO_FIRE_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_MOVE_RIGHT, Shared.Assets.Animations.MARIO_FIRE_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_POWERUP_LEFT, Shared.Assets.Animations.MARIO_FIRE_POWERUP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_POWERUP_RIGHT, Shared.Assets.Animations.MARIO_FIRE_POWERUP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_CROUCHING_RIGHT, Shared.Assets.Animations.MARIO_FIRE_CROUCHING_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_FIRE_CROUCHING_LEFT, Shared.Assets.Animations.MARIO_FIRE_CROUCHING_LEFT_MAT);*/

            /*this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_IDLE_LEFT, Shared.Assets.Animations.MARIO_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_IDLE_RIGHT, Shared.Assets.Animations.MARIO_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_JUMP_LEFT, Shared.Assets.Animations.MARIO_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_JUMP_RIGHT, Shared.Assets.Animations.MARIO_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_MOVE_LEFT, Shared.Assets.Animations.MARIO_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_MOVE_RIGHT, Shared.Assets.Animations.MARIO_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(Shared.Assets.Animations.MARIO_DEAD, Shared.Assets.Animations.MARIO_DEAD_MAT);*/

            this.Animator.Play(Shared.Assets.Animations.MARIO_IDLE_RIGHT);

            if (InitCharacterController)
                this.InitNewScript<CharacterController>();

            if (InitCameraController)
                this.InitNewScript<CameraController>();

            if (InitCollider)
            {
                this.InitNewComponent<Collider>();
                //this.Collider.Area = new RectangleF(2, 0, 12, 16);
            }

            BottomTrigger = new Trigger(this)
            {
                Name = "Bottom_Trigger"
            };
            //BottomTrigger.Transform.Position += new Vector3(1, 16, 0);
            //BottomTrigger.Transform.Dimensions = new Vector3(14, 2, 0);
            BottomTrigger.InitNewScript<BottomMarioChecker>();

            LeftTrigger = new Trigger(this)
            {
                Name = "Left_Trigger"
            };
            //LeftTrigger.Transform.Position += new Vector3(1, 0, 0);
            //LeftTrigger.Transform.Dimensions = new Vector3(1, 14, 0);
            LeftTrigger.InitNewScript<LeftMarioChecker>();

            RightTrigger = new Trigger(this)
            {
                Name = "Right_Trigger"
            };
            //RightTrigger.Transform.Position += new Vector3(14, 0, 0);
            //RightTrigger.Transform.Dimensions = new Vector3(1, 14, 0);
            RightTrigger.InitNewScript<RightMarioChecker>();

            TopTrigger = new Trigger(this)
            {
                Name = "Top_Trigger"
            };
            //TopTrigger.Transform.Position += new Vector3(2.5f, -1, 0);
            //TopTrigger.Transform.Dimensions = new Vector3(11, 1, 0);
            TopTrigger.InitNewScript<TopMarioChecker>();
            

            switch (Shared.Mechanics.MarioCurrentState)
            {
                case State.Small:
                    this.Collider.Area = new RectangleF(2, 0, 12, 16);

                    TopTrigger.Transform.Position += new Vector3(2.5f, -1, 0);
                    TopTrigger.Transform.Dimensions = new Vector3(11, 1, 0);

                    RightTrigger.Transform.Position += new Vector3(14, 0, 0);
                    RightTrigger.Transform.Dimensions = new Vector3(1, 14, 0);

                    LeftTrigger.Transform.Position += new Vector3(1, 0, 0);
                    LeftTrigger.Transform.Dimensions = new Vector3(1, 14, 0);

                    BottomTrigger.Transform.Position += new Vector3(1, 16, 0);
                    BottomTrigger.Transform.Dimensions = new Vector3(14, 2, 0);

                    break;
                case State.Super:
                case State.Fire:
                case State.Invincible:
                    this.Collider.Area = new RectangleF(2, 0, 14, 32);

                    TopTrigger.Transform.Position += new Vector3(0.5f, -1, 0);
                    TopTrigger.Transform.Dimensions = new Vector3(15, 1, 0);

                    RightTrigger.Transform.Position += new Vector3(16, 0, 0);
                    RightTrigger.Transform.Dimensions = new Vector3(1, 30, 0);

                    LeftTrigger.Transform.Position += new Vector3(-1, 0, 0);
                    LeftTrigger.Transform.Dimensions = new Vector3(1, 30, 0);

                    BottomTrigger.Transform.Position += new Vector3(0, 32, 0);
                    BottomTrigger.Transform.Dimensions = new Vector3(16, 2, 0);
                    break;
                default:
                    break;
            }

#if DEBUG
            TopTrigger.Model = new Material(Color.Black, TopTrigger);
            RightTrigger.Model = new Material(Color.Black, RightTrigger);
            LeftTrigger.Model = new Material(Color.Black, LeftTrigger);
            BottomTrigger.Model = new Material(Color.Black, BottomTrigger);
#endif

            //this.InitNewComponent<SoundSource>();

            WorldManager = Behavior.Find<WorldChangeManagerScript>("worldManager");
        }

        public void PipeEnter(Block Pipe)
        {
            ChangeState = true;
            EnteredPipe = Pipe;
        }
    }
}