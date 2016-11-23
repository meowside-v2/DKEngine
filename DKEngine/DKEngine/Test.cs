﻿using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine
{
    class Test : Scene
    {
        public Button b { get; set; }
        public TextBox tx { get; set; }

        public Test()
            :base()
        {
            b = new Button(this)
            {
                Text = "GG",
                OnClick = () => Debug.WriteLine("ButtonJede"),
                width = 100,
                height = 20,
                ScaleX = 4f
            };

            tx = new TextBox(this)
            {
                Y = 20,
                width = 50,
                height = 100,
                ScaleX = 2f,
                Foreground = System.Drawing.Color.FromArgb(0xff, 0x15, 0xfd, 0x50)
            };
        }
    }
}
