using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public abstract class Component
    {
        protected GameObject Parent;
        protected long LastUpdated;

        internal Component(GameObject Parent)
        {
            this.Parent = Parent;
            LastUpdated = Engine.LastUpdated;
        }

        protected internal abstract void Destroy();
    }
}
