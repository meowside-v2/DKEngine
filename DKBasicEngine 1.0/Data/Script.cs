using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public abstract class Script
    {
        internal  Collider.CollisionEnterHandler ColliderDel;
        internal  Engine.UpdateHandler UpdateDel;
        protected GameObject Parent;

        public Script(GameObject Parent)
        {
            this.Parent = Parent;

            UpdateDel = new Engine.UpdateHandler(Update);
            ColliderDel = new Collider.CollisionEnterHandler(OnColliderEnter);
            
            if(Parent.Collider != null)
                Parent.Collider.CollisionEvent += ColliderDel;
        }

        public virtual void Start()
        { }
        public virtual void Update()
        { }
        public virtual void OnColliderEnter(Collider e)
        { }

        internal void Destroy()
        {
            Engine.UpdateEvent -= UpdateDel;

            if (Parent.Collider != null) 
                Parent.Collider.CollisionEvent -= ColliderDel;

            Parent.Scripts.Remove(this);

            Parent = null;
            UpdateDel = null;
            ColliderDel = null;
        }
    }
}
