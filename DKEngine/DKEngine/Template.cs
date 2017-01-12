using DKBasicEngine_1_0;
using System.Diagnostics;
using System.Drawing;

namespace DKEngine
{
    class Template : GameObject
    {
        public override void Start()
        {
            this.Position = new Position(0, -5, 1);
            this.Collider = new Collider(this);
            this.Collider.IsTrigger = true;
            this.Model = new Material(Color.BurlyWood, this);
            this.Scale = new Scale(10, 10, 10);
        }

        public override void Update()
        {
            
            this.Position = new Position(0, this.Position.Y + (1 * Engine.deltaTime), 1);
            Debug.WriteLine($"X:{this.Position.X} Y:{this.Position.Y} Z:{this.Position.Z}");
        }

        public override void OnColliderEnter(Collider e)
        {
            Debug.WriteLine("Triggered");
        }
    }
}
