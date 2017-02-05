using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.Components
{
    public sealed class AnimationNode
    {
        public string Name = "";
        public Material Animation = null;
        public bool IsLoop = false;
        
        private AnimationNode()
        { }

        public AnimationNode(string Name, Material Source)
        {
            this.Name = Name;
            this.Animation = Source;
        }
    }
}
