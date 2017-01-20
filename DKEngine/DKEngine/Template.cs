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
            this.Parent.Collider = new Collider(this.Parent);
            this.Parent.Collider.IsTrigger = true;
            this.Parent.Model = new Material(Color.BurlyWood, Parent);
            this.Parent.Transform.Scale = new Vector3(10, 10, 10);
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
