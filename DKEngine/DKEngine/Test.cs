using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
                X = 50,
                width = 100,
                height = 50,
                FontSize = 4,
                Foreground = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF),
                Background = Color.FromArgb(0x50, 0x50, 150, 200)
            };

            tx = new TextBox(this)
            {
                Y = 20,
                width = 150,
                height = 100,
                FontSize = 2,
                Background = Color.ForestGreen,
                Foreground = Color.FromArgb(0x80, 0x15, 0xfd, 0x50),
                TextHAlignment = TextBlock.HorizontalAlignment.Center,
                TextVAlignment = TextBlock.VerticalAlignment.Center
            };
        }
    }
}
