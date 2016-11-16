﻿using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

            Scene x = new Scene();

            for (int i = 0; i < 100; i++)
                x.Model.Add(new Test() { TypeName = "border" , Y = i * 8 });

            c.sceneReference = x;

            Console.ReadLine();
        }
    }
}
