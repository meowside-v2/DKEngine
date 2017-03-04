using DKBasicEngine_1_0;
using DKBasicEngine_1_0.Core;
using DKBasicEngine_1_0.Core.Components;
using DKBasicEngine_1_0.Core.UI;
using DKEngine.Properties;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace DKEngine
{

    sealed class TemplateScript : Script
    {

        private const int Speed = 5;
        private float Acceleration = 1f;
        private byte[] Sound = null;

        TextBlock DepthMeter;
        TextBlock SpeedMeter;

        public TemplateScript(GameObject Parent)
            :base(Parent)
        { }

        protected override void Start()
        {
            this.Parent.Transform.Position = new Vector3(0, -5, 1);
            this.Parent.Model = new Material(Color.BurlyWood, Parent);
            this.Parent.Transform.Scale = new Vector3(10, 10, 10);
            this.Parent.InitNewComponent<SoundSource>();
            this.Parent.SoundSource.Type = SoundSource.PlayBack.PlayNew;
            this.Sound = Resources.glock;

            DepthMeter = new TextBlock(this.Parent);
            DepthMeter.Name = "Depth meter";
            DepthMeter.Transform.Position += new Vector3(150, 0, 0);
            DepthMeter.Foreground = Color.BlanchedAlmond;
            DepthMeter.Transform.Dimensions = new Vector3(30, 20, 1);
            DepthMeter.Transform.Scale = new Vector3(1, 1, 1);
            DepthMeter.TextHAlignment = Text.HorizontalAlignment.Right;
            DepthMeter.Background = Color.ForestGreen;

            SpeedMeter = new TextBlock(this.Parent);
            SpeedMeter.Name = "Speed meter";
            SpeedMeter.Transform.Position += new Vector3(150, 20, 0);
            SpeedMeter.Foreground = Color.BlanchedAlmond;
            SpeedMeter.Transform.Dimensions = new Vector3(30, 20, 1);
            SpeedMeter.Transform.Scale = new Vector3(1, 1, 1);
            SpeedMeter.TextHAlignment = Text.HorizontalAlignment.Right;
            SpeedMeter.Background = Color.Firebrick;

            SpeedMeter.Text = string.Format("{0:F2}", Acceleration);
            DepthMeter.Text = string.Format("{0:F2}", Parent.Transform.Position.Y);

            /*this.Parent.Animator.Animations.Add("default", new AnimationNode("default", new Material(Color.BurlyWood, Parent)));
            this.Parent.Animator.Play("default");*/
        }

        protected override void Update()
        {
            if (Engine.Input.IsKeyDown(System.ConsoleKey.A))
                //if(!Parent.Collider.Collision(Collider.Direction.Left))
                    this.Parent.Transform.Position += new Vector3(-(Speed * Engine.deltaTime), 0, 0);

            if (Engine.Input.IsKeyDown(System.ConsoleKey.D))
                //if (!Parent.Collider.Collision(Collider.Direction.Right))
                    this.Parent.Transform.Position += new Vector3(Speed * Engine.deltaTime, 0, 0);

            if (Engine.Input.IsKeyDown(System.ConsoleKey.S))
                Acceleration += Engine.deltaTime * 3;

            if (Engine.Input.IsKeyDown(System.ConsoleKey.W))
                if (!Parent.Collider.Collision(Collider.Direction.Up))
                    this.Parent.Transform.Position += new Vector3(0, -(Speed * Engine.deltaTime), 0);
                    

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.Enter))
                this.Parent.SoundSource.Play(Sound);
            
            if (!Parent.Collider.Collision(Collider.Direction.Down))
            {
                this.Parent.Transform.Position += new Vector3(0, Speed * Acceleration * Engine.deltaTime, 0);
                Acceleration += Engine.deltaTime;

                DepthMeter.Text = string.Format("{0:F2}", Parent.Transform.Position.Y);
            }
            else
            {
                Acceleration = 1;
            }

            SpeedMeter.Text = string.Format("{0:F2}", Acceleration);
        }

        protected override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Collided");
        }
    }
}
