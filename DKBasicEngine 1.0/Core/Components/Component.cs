using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.Components
{
    public abstract class Component
    {
        protected GameObject Parent;

        internal Component(GameObject Parent)
        {
            this.Parent = Parent;
        }

        protected internal abstract void Destroy();
    }
}
