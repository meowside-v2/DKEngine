using DKBasicEngine_1_0;
using System.Diagnostics;
using System.Drawing;

namespace DKEngine
{
    class Template : GameObject
    {
        private const int Speed = 5;

        public override void Start()
        {
            this.Transform.Position = new Position(0, -5, 1);
            this.Collider = new Collider(this);
            this.Collider.IsTrigger = true;
            this.Model = new Material(Color.BurlyWood, this);
            this.Transform.Scale = new Scale(10, 10, 10);
        }

        public override void Update()
        {
            this.Transform.Position += new Position(0, Speed * Engine.deltaTime, 0);
        }

        public override void OnColliderEnter(Collider e)
        { }
    }
}
