namespace DKEngine.Core.Components
{
    /// <summary>
    /// Node used in Animator Component
    /// </summary>
    /// <seealso cref="DKEngine.Core.Components.Component" />
    public sealed class AnimationNode : Component
    {
        public Material Animation = null;
        public bool IsLoop = false;

        private AnimationNode()
            : base(null)
        { }

        public AnimationNode(string Name, Material Source)
            : base(null)
        {
            this.Name = Name;
            this.Animation = Source;
        }

        public override void Destroy()
        { }
    }
}