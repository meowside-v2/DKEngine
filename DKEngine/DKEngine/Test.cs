using DKBasicEngine_1_0;
using DKBasicEngine_1_0.Core;
using DKBasicEngine_1_0.Core.Components;
using DKBasicEngine_1_0.Core.UI;
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
            Database.AddNewGameObjectMaterial("trigger", new Material(Color.AliceBlue, new Vector3(1,1,1)));

            GameObject t2 = new GameObject();
            t2.Model = new Material(Color.BurlyWood, t2);
            t2.Transform.Scale = new Vector3(5, 5, 5);
            t2.Name = "Player";
            t2.InitNewScript<PlayerControl>();
            t2.InitNewScript<TemplateScript>();
            t2.Transform.Position -= new Vector3(0, 20, 0);         

            for (int i = 0; i < 100; i++)
            {
                GameObject t1 = new GameObject();
                t1.Name = string.Format("Trigger_{0}", i);
                t1.TypeName = "trigger";
                t1.Transform.Position = new Vector3(i * 20, 0, 0);
                t1.Transform.Scale = new Vector3(10, 10, 10);
                t1.InitNewComponent<Collider>();
            }
            
            for(int i = 0; i < 100; i++)
            {
                TextBlock txt = new TextBlock();
                txt.Name = string.Format("Depth_{0}", i * 100);
                txt.Text = string.Format("{0}", i * 100);
                txt.Transform.Position = new Vector3(-150, i * 100, 1);
                txt.Foreground = Color.LightCyan;
                txt.FontSize = 2f;
                txt.Transform.Dimensions = new Vector3(300, 20, 1);
            }

            Camera c = new Camera();
            c.Position = new Vector3(-300, -100, 0);
            c.Parent = t2;
        }
    }
}
