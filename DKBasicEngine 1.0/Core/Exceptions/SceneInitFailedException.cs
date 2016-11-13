using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class SceneInitFailedException : Exception
    {
        public SceneInitFailedException()
            : base()
        { }

        public SceneInitFailedException(string msg)
            : base(msg)
        { }

        public SceneInitFailedException(string msg, Exception ex)
            : base(msg, ex)
        { }
    }
}
