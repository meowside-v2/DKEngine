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

    class TemplateScript : Script
    {

        private const int Speed = 50;
        private SoundSource Source;
        private byte[] Sound = null;

        public TemplateScript(GameObject Parent)
            :base(Parent)
        { }

        public override void Start()
        {
            this.Parent.Transform.Position = new Vector3(0, -5, 1);
            this.Parent.Collider.IsTrigger = true;
            this.Parent.Model = new Material(Color.BurlyWood, Parent);
            this.Parent.Transform.Scale = new Vector3(10, 10, 10);
            this.Parent.Collider.IsCollidable = true;
            this.Source = new SoundSource();
            this.Source.Type = SoundSource.PlayBack.PlayNew;
            this.Sound = Resources.glock;
            
            /*this.Parent.Animator.Animations.Add("default", new AnimationNode("default", new Material(Color.BurlyWood, Parent)));
            this.Parent.Animator.Play("default");*/
        }

        public override void Update()
        {
            if (Engine.Input.IsKeyPressed(System.ConsoleKey.A) || Engine.Input.IsKeyDown(System.ConsoleKey.A))
                this.Parent.Transform.Position += new Vector3(-(Speed * Engine.deltaTime), 0, 0);

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.D) || Engine.Input.IsKeyDown(System.ConsoleKey.D))
                this.Parent.Transform.Position += new Vector3(Speed * Engine.deltaTime, 0, 0);

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.W) || Engine.Input.IsKeyDown(System.ConsoleKey.W))
                this.Parent.Transform.Position += new Vector3(0, -(Speed * Engine.deltaTime), 0);

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.S) || Engine.Input.IsKeyDown(System.ConsoleKey.S))
                this.Parent.Transform.Position += new Vector3(0, Speed * Engine.deltaTime, 0);

            if (Engine.Input.IsKeyPressed(System.ConsoleKey.Enter))
                this.Source.Play(Sound);
        }

        public override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Collided");
        }
    }
}
