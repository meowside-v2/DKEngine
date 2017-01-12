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
        public Position Position;

        internal float X { get { return RenderingGUI ? 0 : Parent != null ? Parent.X + Position.X : Position.X; } }
        internal float Y { get { return RenderingGUI ? 0 : Parent != null ? Parent.Y + Position.Y : Position.Y; } }

        public GameObject Parent = null;

        private bool RenderingGUI = false;

        public Camera()
        {
            this.Position = new Position(0, 0, 0);
            Engine._baseCam = this;
        }

        public void Destroy()
        {
            if (Engine._baseCam == this)
                Engine._baseCam = null;

            Parent = null;
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

            RenderingGUI = true;
            List<GameObject> GUI = Temp.Where(item => item.IsGUI);
            int GUICount = GUI.Count;
            for (int i = 0; i < GUICount; i++)
                Temp.Remove(GUI[i]);

            while (GUICount > 0)
            {
                float tempHeight = GUI.FindMaxZ();
                List<GameObject> toRender = GUI.Where(item => item.Z == tempHeight).ToList();

                int toRenderCount = toRender.Count;
                for (int i = 0; i < toRenderCount; i++)
                    toRender[i].Render();

                /*Parallel.For(0, toRenderCount, (i) =>
                {
                    toRender[i].Render();
                });*/

                for (int i = 0; i < toRenderCount; i++)
                {
                    GUI.Remove(toRender[i]);
                    GUICount--;
                }
            }

            RenderingGUI = false;

            int TempCount = Temp.Count;

            while(TempCount > 0)
            {
                float tempHeight = Temp.FindMaxZ();
                List<GameObject> toRender = Temp.Where(item => item.Z == tempHeight).ToList();

                int toRenderCount = toRender.Count;
                for (int i = 0; i < toRenderCount; i++)
                    toRender[i].Render();

                /*Parallel.For(0, toRenderCount, (i) =>
                {
                    toRender[i].Render();
                });*/

                for (int i = 0; i < toRenderCount; i++)
                {
                    Temp.Remove(toRender[i]);
                    TempCount--;
                }
            }
        }
    }
}
