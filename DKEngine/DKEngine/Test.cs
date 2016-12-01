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
                Foreground = Color.FromArgb(0xCD, 0xFF, 0xFF, 0xFF),
                Background = Color.FromArgb(0x80, 0xFF, 0x00, 0XFF)
            };

            tx = new TextBox(this)
            {
                Y = 20,
                width = 150,
                height = 100,
                FontSize = 2,
                Background = Color.FromArgb(0x70, 0x50, 0x00, 0x20),
                Foreground = Color.FromArgb(0x90, 0xff, 0xfd, 0x48),
                TextHAlignment = TextBlock.HorizontalAlignment.Center,
                TextVAlignment = TextBlock.VerticalAlignment.Center
            };

            new TextBlock(this)
            {
                X = 120,
                Y = 40,
                width = 300,
                height = 100,
                Text = "Ahoj ;)",
                Background = Color.FromArgb(0xAD, 128, 100, 10),
                Foreground = Color.FromArgb(0xCC, 80, 75, 190),
                FontSize = 2f,
                TextHAlignment = TextBlock.HorizontalAlignment.Center,
                TextVAlignment = TextBlock.VerticalAlignment.Center
            };

            new TextBlock(this)
            {
                X = 120,
                Y = 45,
                width = 300,
                height = 100,
                Text = "Ahoj ;)",
                Background = Color.FromArgb(0xAD, 128, 100, 10),
                Foreground = Color.FromArgb(0xCC, 80, 75, 190),
                FontSize = 2f,
                TextHAlignment = TextBlock.HorizontalAlignment.Center,
                TextVAlignment = TextBlock.VerticalAlignment.Center
            };

            new TextBlock(this)
            {
                X = 120,
                Y = 50,
                width = 300,
                height = 100,
                Text = "Ahoj ;)",
                Background = Color.FromArgb(0xAD, 128, 100, 10),
                Foreground = Color.FromArgb(0xCC, 80, 75, 190),
                FontSize = 2f,
                TextHAlignment = TextBlock.HorizontalAlignment.Center,
                TextVAlignment = TextBlock.VerticalAlignment.Center
            };
        }
    }
}
