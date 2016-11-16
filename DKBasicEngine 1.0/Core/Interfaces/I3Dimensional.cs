using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public interface I3Dimensional
    {
        double width { get; }
        double height { get; }
        double depth { get; }

        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        double ScaleX { get; set; }
        double ScaleY { get; set; }
        double ScaleZ { get; set; }
    }
}
