using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public sealed class AnimationNode : Component
    {
        public Material Animation = null;
        public bool IsLoop = false;
        
        private AnimationNode()
            :base(null)
        { }

        public AnimationNode(string Name, Material Source)
            : base(null)
        {
            this.Name = Name;
            this.Animation = Source;
        }

        public override void Destroy()
        { }

        internal override void Init()
        { }
    }
}
