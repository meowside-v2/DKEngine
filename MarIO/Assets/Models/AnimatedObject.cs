using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    abstract class AnimatedObject : GameObject
    {
        public bool IsDead = false;

        public AnimatedObject()
            :base()
        { }

        public AnimatedObject(GameObject Parent)
            :base(Parent)
        { }
    }
}
