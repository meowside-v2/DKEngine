/*
* (C) 2017 David Knieradl 
*/

using DKEngine.Core.Ext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public sealed class Camera
    {
        public Vector3 Position;

        internal float X { get { return RenderingGUI ? 0 : Parent != null ? Parent.Transform.Position.X + Position.X : Position.X; } }
        internal float Y { get { return RenderingGUI ? 0 : Parent != null ? Parent.Transform.Position.Y + Position.Y : Position.Y; } }

        public float MinRenderDepth { get; set; }
        public float MaxRenderDepth { get; set; }

        public GameObject Parent = null;

        private bool RenderingGUI = false;

        public Camera()
        {
            this.Position = new Vector3(0, 0, 0);
            Engine.BaseCam = this;
        }

        public void Destroy()
        {
            if (Engine.BaseCam == this)
                Engine.BaseCam = null;

            Parent = null;
        }
        
        internal void BufferImage(List<GameObject> GameObjectsInView)
        {
            Array.Clear(Engine.Render.imageBuffer, 0, Engine.Render.imageBuffer.Length);
            Array.Clear(Engine.Render.imageBufferKey, 0, Engine.Render.imageBufferKey.Length);

            
            List<GameObject> Temp = null;


            if (GameObjectsInView != null)
                Temp = GameObjectsInView;

            else
                Temp = Engine.RenderGameObjects.Where(obj => obj.IsInView && obj.Transform.Position.Z > MinRenderDepth && obj.Transform.Position.Z < MaxRenderDepth).ToList(); 

            RenderingGUI = true;
            List<GameObject> GUI = Temp.Where(item => item.IsGUI).ToList();
            int GUICount = GUI.Count;
            for (int i = 0; i < GUICount; i++)
                Temp.Remove(GUI[i]);

            while (GUICount > 0)
            {
                float tempHeight = GUI.FindMaxZ();
                GameObject[] toRender = GUI.Where(item => item.Transform.Position.Z == tempHeight).ToArray();

                int toRenderCount = toRender.Length;
                for (int i = toRenderCount - 1; i >= 0; i--)
                {
                    toRender[i].Render();
                    GUI.Remove(toRender[i]);
                    GUICount--;
                }
            }

            RenderingGUI = false;
            int TempCount = Temp.Count;

            while(TempCount > 0)
            {
                float tempHeight = Temp.FindMaxZ();
                GameObject[] toRender = Temp.Where(item => item.Transform.Position.Z == tempHeight).ToArray();

                int toRenderCount = toRender.Length;
                for (int i = toRenderCount - 1; i >= 0; i--)
                {
                    toRender[i].Render();
                    Temp.Remove(toRender[i]);
                    TempCount--;
                }
            }
        }
    }
}
