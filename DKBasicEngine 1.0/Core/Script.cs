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
        internal Engine.EngineHandler UpdateHandle;
        internal Collider.CollisionEnterHandler CollisionHandler;

        public Script(GameObject Parent)
        {
            this.Parent = Parent;
            Engine.InitScripts.Add(this);
            UpdateHandle = new Engine.EngineHandler(Update);
            CollisionHandler = new Collider.CollisionEnterHandler(OnColliderEnter);
            
            Parent.Collider.CollisionEvent += CollisionHandler;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void OnColliderEnter(Collider e);

        internal void Destroy()
        {
            Engine.UpdateEvent -= UpdateHandle;
            Parent.Collider.CollisionEvent -= CollisionHandler;

            Parent.Scripts.Remove(this);
            Parent = null;
            UpdateHandle = null;
        }
    }
}
