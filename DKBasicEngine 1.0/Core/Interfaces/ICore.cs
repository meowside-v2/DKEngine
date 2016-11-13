using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public interface ICore
    {
        void Render(int x, int y, byte[] bufferData, bool[] bufferKey);
    }
}
