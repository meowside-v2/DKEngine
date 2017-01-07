using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class EmptyGameObject : I3Dimensional, ICore
    {
        public virtual EmptyGameObject Parent { get; internal set; }

        internal override float X { get { return Parent != null ? Transform.X + Parent.X : Transform.X; } }
        internal override float Y { get { return Parent != null ? Transform.Y + Parent.Y : Transform.Y; } }
        internal override float Z { get { return Parent != null ? Transform.Z + Parent.Z : Transform.Z; } }
        
        internal override float ScaleX { get { return Parent != null ? Scale.X * Parent.Scale.X : Scale.X; } }
        internal override float ScaleY { get { return Parent != null ? Scale.Y * Parent.Scale.Y : Scale.Y; } }
        internal override float ScaleZ { get { return Parent != null ? Scale.Z * Parent.Scale.Z : Scale.Z; } }
        
        internal EmptyGameObject()
        {
            this.Dimensions = new Dimensions(0, 0, 0);
            this.Scale      = new Scale(1, 1, 1);
            this.Transform  = new Transform(0, 0, 0);

            lock (Engine.ToStart)
                lock (Engine.ToUpdate)
                {
                    Engine.ToStart.Add(this);
                    Engine.ToUpdate.Add(this);
                }
        }

        public EmptyGameObject(Scene ToAddToModel)
            :this()
        {
            if (ToAddToModel != null)
                lock (ToAddToModel)
                    ToAddToModel.Model.Add(this);
        }

        public EmptyGameObject(EmptyGameObject Parent)
            : this()
        {
            if (Parent != null)
            {
                this.Parent = Parent;
            }
        }

        public virtual void Start()
        { }

        public virtual void Update()
        { }

        public virtual void Destroy()
        {
            Engine.ToUpdate.Remove(this);

            this.Parent = null;
        }
    }
}
