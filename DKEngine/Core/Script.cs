using DKEngine.Core.Components;

namespace DKEngine.Core
{
    /// <summary>
    /// Script base class
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Behavior" />
    public abstract class Script : Behavior
    {
        internal Collider.CollisionEnterHandler CollisionHandler;

        public Script(GameObject Parent)
            : base(Parent)
        {
            if (Parent.Collider != null)
            {
                CollisionHandler = new Collider.CollisionEnterHandler(OnColliderEnter);
                Parent.Collider.CollisionEvent += CollisionHandler;
            }
        }

        protected internal abstract void OnColliderEnter(Collider e);

        public override void Destroy()
        {
            if (UpdateHandle != null)
                Engine.UpdateEvent -= UpdateHandle;

            if (CollisionHandler != null)
                Parent.Collider.CollisionEvent -= CollisionHandler;

            Parent.Scripts.Remove(this);
            Parent = null;
            UpdateHandle = null;
        }
    }
}