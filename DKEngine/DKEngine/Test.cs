using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine
{
    class Test : Scene
    {
        
        public Test()
        { }

        public void LoadStuff()
        {
            Database.AddNewGameObjectMaterial("animatedDemo", new Material(Image.FromFile(@"giphy-downsized-large.gif")));

            GameObject t = new GameObject()
            {
                TypeName = "animatedDemo",
                Transform = new Transform(200, 0, 0)
            };

            t.Animator.Settings = AnimationLoop.Endless;

            for (int i = 0; i < 100; i++)
            {
                string tx = i.ToString();

                new Button()
                {
                    Text = tx + " Button",
                    OnClick = () => Debug.WriteLine(tx + " Button clicked"),
                    Transform = new Transform(0, i * 34, -20),
                    Dimensions = new Dimensions(100, 8, 1),
                    FontSize = 1,
                    Foreground = Color.FromArgb(0xFF, 0xAF, 0xAF, 0xAF),
                    Background = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF)
                };
            }

            new Button()
            {
                Text = "GG",
                Transform = new Transform(50, 0, 2),
                Dimensions = new Dimensions(100, 50, 1),
                OnClick = () => Debug.WriteLine("ButtonJede"),
                FontSize = 4,
                Foreground = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF),
                Background = Color.FromArgb(0x80, 0xFF, 0x00, 0XFF)
            };

            for (int a = 1; a < 2; a++)
                for (int i = 0; i < 16; i++)
                    for (int j = 0; j < 16; j++)
                        new GameObject()
                        {
                            Transform = new Transform(i * 16, j * 16, -a),
                            Scale = new Scale(1, 1, 1),
                            TypeName = "border"
                        };

            for (int a = 1; a < 2; a++)
                for (int i = 1; i < 32; i++)
                    for (int j = 1; j < 64; j++)
                        new GameObject()
                        {
                            Transform = new Transform(i * 1024, j * 16, -a),
                            Scale = new Scale(1, 1, 1),
                            TypeName = "border"
                        };
        }
    }
}
