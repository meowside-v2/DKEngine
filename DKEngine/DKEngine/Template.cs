using DKBasicEngine_1_0;
using DKBasicEngine_1_0.Core;
using DKBasicEngine_1_0.Core.Components;
using DKEngine.Properties;
using NAudio.Wave;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace DKEngine
{

    sealed class TemplateScript : Script
    {

        private const int Speed = 50;
        private byte[] Sound = null;

        public TemplateScript(GameObject Parent)
            :base(Parent)
        { }

        protected override void Start()
        {
            this.Parent.Transform.Position = new Vector3(0, -5, 1);
            this.Parent.Collider.IsTrigger = true;
            this.Parent.Model = new Material(Color.BurlyWood, Parent);
            this.Parent.Transform.Scale = new Vector3(10, 10, 10);
            this.Parent.InitNewComponent<SoundSource>();
            this.Parent.SoundSource.Type = SoundSource.PlayBack.PlayNew;
            this.Sound = Resources.glock;
            
            /*this.Parent.Animator.Animations.Add("default", new AnimationNode("default", new Material(Color.BurlyWood, Parent)));
            this.Parent.Animator.Play("default");*/
        }

        protected override void Update()
        {
            if (Engine.Input.IsKeyDown(System.ConsoleKey.A))
                if(!Parent.Collider.Collision(Collider.Direction.Left))
                    this.Parent.Transform.Position += new Vector3(-(Speed * Engine.deltaTime), 0, 0);

            if (Engine.Input.IsKeyDown(System.ConsoleKey.D))
                if (!Parent.Collider.Collision(Collider.Direction.Right))
                    this.Parent.Transform.Position += new Vector3(Speed * Engine.deltaTime, 0, 0);

            if (Engine.Input.IsKeyDown(System.ConsoleKey.W))
                if (!Parent.Collider.Collision(Collider.Direction.Up))
                    this.Parent.Transform.Position += new Vector3(0, -(Speed * Engine.deltaTime), 0);

            if (Engine.Input.IsKeyDown(System.ConsoleKey.S))
                if (!Parent.Collider.Collision(Collider.Direction.Down))
                    this.Parent.Transform.Position += new Vector3(0, Speed * Engine.deltaTime, 0);

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.Enter))
                this.Parent.SoundSource.Play(Sound);
        }

        protected override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Collided");
        }
    }
}
