using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core
{
    public abstract class Script : Behavior
    {
        internal Collider.CollisionEnterHandler CollisionHandler;

        public Script(GameObject Parent)
            :base(Parent)
        { }

        protected internal abstract void OnColliderEnter(Collider e);

        protected internal override void Destroy()
        {
            if(CollisionHandler != null)
            {
                Engine.UpdateEvent -= UpdateHandle;
                Parent.Collider.CollisionEvent -= CollisionHandler;
            }
            
            Parent.Scripts.Remove(this);
            Parent = null;
            UpdateHandle = null;
        }
    }
}
