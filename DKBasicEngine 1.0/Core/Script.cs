using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core
{
    public abstract class Script
    {
        protected GameObject Parent;

        public Script(GameObject Parent)
        {
            this.Parent = Parent;
            
            Parent.Collider.CollisionEvent += OnColliderEnter;
        }

        public virtual void Start()
        { }
        public virtual void Update()
        { }
        public virtual void OnColliderEnter(Collider e)
        { }

        internal void Destroy()
        {
            Engine.UpdateEvent -= Update;
            Parent.Collider.CollisionEvent -= OnColliderEnter;

            Parent.Scripts.Remove(this);

            Parent = null;
        }
    }
}
