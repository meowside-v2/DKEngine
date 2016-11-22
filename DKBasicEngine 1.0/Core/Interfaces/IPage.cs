using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public interface IPage
    {
        int FocusSelection { get; set; }
        List<IControl> PageControls { get; }
        List<I3Dimensional> Model { get; }
    }
}
