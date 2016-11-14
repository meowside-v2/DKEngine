using DKBasicEngine_1_0;
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
            Database.InitDatabase();

            Database.AddNewGameObject("gif", new Material(Image.FromFile(@"asd.gif")));

            Camera cam = new Camera();

            cam.sceneReference = new Scene();

            GameObject g1 = new GameObject()
            {
                ScaleX = 2,
                ScaleY = 2,
                ScaleZ = 2,
                TypeName = "gif"
            };

            GameObject g2 = new GameObject()
            {
                X = g1.width,
                ScaleX = 2,
                ScaleY = 2,
                ScaleZ = 2,
                TypeName = "gif"
            };

            GameObject g3 = new GameObject()
            {
                X = g2.width,
                ScaleX = 2,
                ScaleY = 2,
                ScaleZ = 2,
                TypeName = "gif"
            };

            Play(g1, g2, g3);

            cam.sceneReference.Model.AddAll(g1, g2, g3);

            cam.Init(0, 0);

            Console.ReadLine();
        }

        static void Play(params GameObject[] g)
        {
            foreach (GameObject x in g)
            {
                Timer t = new Timer((s) =>
                {
                    x.AnimationState++;

                    if (x.AnimationState >= x.model.Frames)
                        x.AnimationState = 0;
                }, null, 0, x.model.DurationPerFrame);
            }
        }
    }
}
