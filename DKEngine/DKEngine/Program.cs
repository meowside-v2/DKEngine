using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DKEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Init();

            Camera c = new Camera();
            c.Init(0, 0);
            
            Test t = new Test();

            Engine.PageChange(t);
        }
    }
}
