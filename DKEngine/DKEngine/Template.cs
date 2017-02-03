using DKBasicEngine_1_0;
using System.Diagnostics;
using System.Drawing;

namespace DKEngine
{

    class TemplateScript : Script
    {

        private const int Speed = 5;

        public TemplateScript(GameObject Parent)
            :base(Parent)
        { }

        public override void Start()
        {
            this.Parent.Transform.Position = new Vector3(0, -5, 1);
            this.Parent.Collider.IsTrigger = true;
            //this.Parent.Model = new Material(Color.BurlyWood, Parent);
            this.Parent.Transform.Scale = new Vector3(10, 10, 10);
            this.Parent.Animator.Animations.Add("default", new AnimationNode("default", new Material(Color.BurlyWood, Parent)));
            this.Parent.Animator.Play("default");
        }

        public override void Update()
        {
            this.Parent.Transform.Position += new Vector3(0, Speed * Engine.deltaTime, 0);
        }

        public override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Collided");
        }
    }
}
