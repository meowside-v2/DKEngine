using DKBasicEngine_1_0;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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

            GameObject g = new GameObject()
            {
                TypeName = "gif"
            };

            g.Play();

            cam.sceneReference.Model.Add(g);

            cam.Init(0, 0);

            Console.ReadLine();
        }
    }
}
