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
        private State _currentState;
        private bool _isDestroyed;

        public override bool IsDestroyed
        {
            get { return _isDestroyed; }
            set
            {
                _isDestroyed = value;
                if (value)
                    CurrentState = State.Dead;
            }
        }

        public bool KilledEnemy = false;
        public Trigger LeftTrigger { get; private set; }
        public Trigger RightTrigger { get; private set; }
        public Trigger TopTrigger { get; private set; }
        public Trigger BottomTrigger { get; private set; }

        public bool InitCharacterController { get; set; }
        public bool InitCameraController { get; set; }
        public bool InitCollider { get; set; }

        public State CurrentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                Shared.Mechanics.MarioCurrentState = value;

                Vector3 tmp = this.Transform.Position;

                switch (value)
                {
                    case State.Dead:
                    case State.Small:
                        this.Collider.Area = new RectangleF(2, 0, 12, 16);

                        TopTrigger.Transform.Position = tmp.Add(2.5f, -1, 0);//new Vector3(tmp.X + 2.5f, tmp.Y - 1, tmp.Z);
                        TopTrigger.Transform.Dimensions = new Vector3(11, 1, 0);

                        RightTrigger.Transform.Position = tmp.Add(14, 0, 0); //new Vector3(tmp.X + 14, tmp.Y, tmp.Z);
                        RightTrigger.Transform.Dimensions = new Vector3(1, 14, 0);

                        LeftTrigger.Transform.Position = tmp.Add(1, 0, 0); //new Vector3(tmp.X + 1, tmp.Y, tmp.Z);
                        LeftTrigger.Transform.Dimensions = new Vector3(1, 14, 0);

                        BottomTrigger.Transform.Position = tmp.Add(1, 16, 0); //new Vector3(tmp.X + 1, tmp.Y + 16, tmp.Z);
                        BottomTrigger.Transform.Dimensions = new Vector3(14, 2, 0);

                        TopTrigger.Collider.Area = new RectangleF(0, 0, 11, 1);
                        RightTrigger.Collider.Area = new RectangleF(0, 0, 1, 14);
                        LeftTrigger.Collider.Area = new RectangleF(0, 0, 1, 14);
                        BottomTrigger.Collider.Area = new RectangleF(0, 0, 14, 2);

                        break;

                    case State.Super:
                    case State.Fire:
                    case State.Invincible:
                        this.Collider.Area = new RectangleF(2, 0, 14, 32);

                        TopTrigger.Transform.Position = tmp.Add(0.5f, -1, 0); //new Vector3(tmp.X + 0.5f, tmp.Y - 1, tmp.Z + 0);
                        TopTrigger.Transform.Dimensions = new Vector3(15, 1, 0);

                        RightTrigger.Transform.Position = tmp.Add(16, 0, 0); //new Vector3(tmp.X + 16, tmp.Y + 0, tmp.Z + 0);
                        RightTrigger.Transform.Dimensions = new Vector3(1, 30, 0);

                        LeftTrigger.Transform.Position = tmp.Add(-1, 0, 0); //new Vector3(tmp.X - 1, tmp.Y + 0, tmp.Z + 0);
                        LeftTrigger.Transform.Dimensions = new Vector3(1, 30, 0);

                        BottomTrigger.Transform.Position = tmp.Add(0, 32, 0); //new Vector3(tmp.X + 0, tmp.Y + 32, tmp.Z + 0);
                        BottomTrigger.Transform.Dimensions = new Vector3(16, 2, 0);

                        TopTrigger.Collider.Area = new RectangleF(0, 0, 15, 1);
                        RightTrigger.Collider.Area = new RectangleF(0, 0, 1, 30);
                        LeftTrigger.Collider.Area = new RectangleF(0, 0, 1, 30);
                        BottomTrigger.Collider.Area = new RectangleF(0, 0, 16, 2);

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
            }
        }

        public Movement CurrentMovement { get; set; }
        public Direction PipeEnteredInDirection { get { return EnteredPipe.PipeEnterDirection; } }
        public Block EnteredPipe { get; set; }

        public WorldChangeManagerScript WorldManager { get; set; }

        public Mario()
        {
            InitTriggers();
        }

        public Mario(GameObject Parent)
            : base(Parent)
        {
            InitTriggers();
        }

        public enum State
        {
            Dead,
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
            this.Animator.AddAnimation(MARIO_IDLE_LEFT, MARIO_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_IDLE_RIGHT, MARIO_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_JUMP_LEFT, MARIO_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_JUMP_RIGHT, MARIO_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_MOVE_LEFT, MARIO_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_MOVE_RIGHT, MARIO_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_DEAD, MARIO_DEAD_MAT);
            this.Animator.AddAnimation(MARIO_CROUCHING_LEFT, MARIO_CROUCHING_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_CROUCHING_RIGHT, MARIO_CROUCHING_RIGHT_MAT);

            this.Animator.AddAnimation(MARIO_SUPER_IDLE_LEFT, MARIO_SUPER_IDLE_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_IDLE_RIGHT, MARIO_SUPER_IDLE_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_JUMP_LEFT, MARIO_SUPER_JUMP_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_JUMP_RIGHT, MARIO_SUPER_JUMP_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_MOVE_LEFT, MARIO_SUPER_MOVE_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_MOVE_RIGHT, MARIO_SUPER_MOVE_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_POWERUP_LEFT, MARIO_SUPER_POWERUP_LEFT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_POWERUP_RIGHT, MARIO_SUPER_POWERUP_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_CROUCHING_RIGHT, MARIO_SUPER_CROUCHING_RIGHT_MAT);
            this.Animator.AddAnimation(MARIO_SUPER_CROUCHING_LEFT, MARIO_SUPER_CROUCHING_LEFT_MAT);

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

            if (InitCharacterController)
                this.InitNewScript<CharacterController>();

            if (InitCameraController)
                this.InitNewScript<CameraController>();

            if (InitCollider)
            {
                this.InitNewComponent<Collider>();
            }

            
            BottomTrigger.InitNewScript<BottomMarioChecker>();
            LeftTrigger.InitNewScript<LeftMarioChecker>();
            RightTrigger.InitNewScript<RightMarioChecker>();
            TopTrigger.InitNewScript<TopMarioChecker>();

            CurrentState = Shared.Mechanics.MarioCurrentState;

            WorldManager = Behavior.Find<WorldChangeManagerScript>("worldManager");
        }

        private void InitTriggers()
        {
            BottomTrigger = new Trigger(this)
            {
                Name = "Bottom_Trigger"
            };
            LeftTrigger = new Trigger(this)
            {
                Name = "Left_Trigger"
            };
            TopTrigger = new Trigger(this)
            {
                Name = "Top_Trigger"
            };
            RightTrigger = new Trigger(this)
            {
                Name = "Right_Trigger"
            };
        }
        public void PipeEnter(Block Pipe)
        {
            ChangeState = true;
            EnteredPipe = Pipe;
        }
    }
}