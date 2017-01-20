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

        protected sealed override void Init()
        {
            /*Database.AddNewGameObjectMaterial("animatedDemo", new Material(Image.FromFile(@"giphy-downsized-large.gif")));

            GameObject t = new GameObject();
            t.TypeName = "animatedDemo";
            t.Transform.Position = new Position(200, 0, 0);

            t.TypeName = "animatedDemo";
            t.Transform.Position = new Position(200, 0, 0);
            t.Animator.Settings = AnimationLoop.Endless;

            for (int i = 0; i < 100; i++)
            {
                string tx = i.ToString();

                Button b1 = new Button();
                //{
                b1.Text = tx + " Button";
                b1.OnClick = () => Debug.WriteLine(tx + " Button clicked");
                b1.Transform.Position = new Position(0, i * 34, -20);
                b1.Transform.Dimensions = new Dimensions(100, 8, 1);
                b1.FontSize = 1;
                b1.Foreground = Color.FromArgb(0xFF, 0xAF, 0xAF, 0xAF);
                b1.Background = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                //};
            }*/

            /*Button b = new Button();
            //{
            b.Text = "GG";
                b.Transform.Position = new Vector3(50, 0, 2);
                b.Transform.Dimensions = new Vector3(100, 50, 1);
                b.OnClick = () => Debug.WriteLine("ButtonJede");
                b.FontSize = 4;
                b.Foreground = Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF);
                b.Background = Color.FromArgb(0x80, 0xFF, 0x00, 0XFF);
            //};*/

            /*for (int a = 1; a < 2; a++)
                for (int i = 0; i < 16; i++)
                    for (int j = 0; j < 16; j++)
                    {
                        GameObject g = new GameObject();
                        //{
                        g.Transform.Position = new Position(i * 16, j * 16, -a);
                        g.TypeName = "border";
                        //};
                    }


            for (int a = 1; a < 2; a++)
                for (int i = 1; i < 32; i++)
                    for (int j = 1; j < 64; j++)
                    {
                        GameObject g = new GameObject();
                        g.Transform.Position = new Position(i * 1024, j * 16, -a);
                        g.TypeName = "border";
                    }*/
                        
                        
            for (int i = 0; i < 10; i++)
            {
                GameObject t1 = new GameObject();
                t1.Model = new Material(Color.AliceBlue, t1);
                t1.Transform.Position = new Vector3(0, i * 10, 0);
                t1.Collider = new Collider(t1);
                t1.Transform.Scale = new Vector3(5, 5, 5);
            }
            
            GameObject t2 = new GameObject();
            t2.Scripts.Add(new TemplateScript(t2));
            t2.Transform.Position = new Vector3(0, -10, 0);
            
            Camera c = new Camera();
            c.Position = new Vector3(-300, -100, 0);
            c.Parent = t2;
        }
    }
}
