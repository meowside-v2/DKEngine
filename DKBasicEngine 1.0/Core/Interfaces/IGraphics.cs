using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public interface IGraphics
    {
        bool HasShadow { get; set; }
        int AnimationState { get; set; }
        Material modelBase { get; set; }
        Material modelRastered { get; }
    }
}
