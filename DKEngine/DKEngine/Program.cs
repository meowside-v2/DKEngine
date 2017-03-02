using DKBasicEngine_1_0;
using DKBasicEngine_1_0.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            
            Engine.ChangeScene<Test>();
        }
    }
}
