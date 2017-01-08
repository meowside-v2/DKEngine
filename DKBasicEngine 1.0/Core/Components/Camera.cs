using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Camera
    {
        public float Xoffset = 0;
        public float Yoffset = 0;

        public Camera()
        {
            Engine._baseCam = this;
        }

        public void Init(int Xoffset, int Yoffset)
        {
            this.Xoffset = Xoffset;
            this.Yoffset = Yoffset;
        }

        public void Destroy()
        {
            if (Engine._baseCam == this)
                Engine._baseCam = null;
        }
        
        internal void BufferImage()
        {
            Array.Clear(Engine.Render.imageBuffer, 0, Engine.Render.imageBuffer.Length);
            Array.Clear(Engine.Render.imageBufferKey, 0, Engine.Render.imageBufferKey.Length);

            List<GameObject> Temp = null;

            lock (Engine.ToRender)
            {
                Temp = Engine.ToRender.Where(item => item.IsInView()).ToList(); 
            }

            List<GameObject> GUI = Temp.Where(item => item.IsGUI);

            int GUICount = GUI.Count;

            for (int i = 0; i < GUICount; i++)
                Temp.Remove(GUI[i]);

            while (GUICount > 0)
            {
                float tempHeight = GUI.FindMaxZ();
                List<GameObject> toRender = GUI.Where(item => item.Z == tempHeight).ToList();

                int toRenderCount = toRender.Count;

                Parallel.For(0, toRenderCount, (i) =>
                {
                    toRender[i].Render();
                });

                for (int i = 0; i < toRenderCount; i++)
                {
                    GUI.Remove(toRender[i]);
                    GUICount--;
                }
            }

            int TempCount = Temp.Count;

            while(TempCount > 0)
            {
                float tempHeight = Temp.FindMaxZ();
                List<GameObject> toRender = Temp.Where(item => item.Z == tempHeight).ToList();

                int toRenderCount = toRender.Count;

                Parallel.For(0, toRenderCount, (i) =>
                {
                    toRender[i].Render();
                });

                for (int i = 0; i < toRenderCount; i++)
                {
                    Temp.Remove(toRender[i]);
                    TempCount--;
                }
            }
        }
    }
}
