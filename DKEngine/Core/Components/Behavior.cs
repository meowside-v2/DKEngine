using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        }

        internal sealed override void Init()
        {
            try
            {
                Engine.LoadingScene.AllBehaviors.Add(this);
                Engine.LoadingScene.NewlyGeneratedBehaviors.Push(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        protected internal abstract void Start();
        protected internal abstract void Update();
    }
}
