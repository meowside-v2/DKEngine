using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core
{
    public abstract class Script : Behavior
    {
        internal Collider.CollisionEnterHandler CollisionHandler;

        public Script(GameObject Parent)
            :base(Parent)
        {
            if (Parent.Collider == null)
                Parent.Collider = new Collider(Parent);

            CollisionHandler = new Collider.CollisionEnterHandler(OnColliderEnter);
            Parent.Collider.CollisionEvent += CollisionHandler;
        }

        protected internal abstract void OnColliderEnter(Collider e);

        protected internal override void Destroy()
        {
            Engine.UpdateEvent -= UpdateHandle;
            Parent.Collider.CollisionEvent -= CollisionHandler;

            Parent.Scripts.Remove(this);
            Parent = null;
            UpdateHandle = null;
        }
    }
}
