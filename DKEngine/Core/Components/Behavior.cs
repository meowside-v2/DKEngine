using System;
using System.Diagnostics;

namespace DKEngine.Core.Components
{
    /// <summary>
    /// Base class for updated components
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Component" />
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

        /// <summary>
        /// Called before parent object is rendered for first time
        /// </summary>
        protected internal abstract void Start();

        /// <summary>
        /// Updates each frame once
        /// </summary>
        protected internal abstract void Update();
    }
}