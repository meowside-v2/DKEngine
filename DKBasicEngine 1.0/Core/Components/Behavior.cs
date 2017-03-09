using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public abstract class Behavior : Component
    {
        internal Engine.EngineHandler UpdateHandle;

        public Behavior(GameObject Parent)
            : base(Parent)
        {
            UpdateHandle = new Engine.EngineHandler(Update);

            if (Engine.LoadingScene != null)
            {
                Engine.LoadingScene.NewlyGeneratedComponents.Add(this);
                Engine.LoadingScene.AllComponents.Add(this);
            }
        }

        protected internal abstract void Start();
        protected internal abstract void Update();
    }
}
