/*
* (C) 2017 David Knieradl 
*/

using DKEngine.Core.Ext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public sealed class Camera : Component
    {
        public Color BackGround = Color.Black;
        public Vector3 Position;

        internal float X { get { return RenderingGUI ? 0 : Parent != null ? Parent.Transform.Position.X + Position.X : Position.X; } }
        internal float Y { get { return RenderingGUI ? 0 : Parent != null ? Parent.Transform.Position.Y + Position.Y : Position.Y; } }

        /*public float MinRenderDepth { get; set; }
        public float MaxRenderDepth { get; set; }*/

        private bool RenderingGUI = false;

        public Camera()
            :base(null)
        { }

        public Camera(GameObject Parent)
            :base(Parent)
        { }
        
        internal void BufferImage(List<GameObject> GameObjectsInView)
        {
            BackGroundInit();
            RenderingGUI = true;
            List<GameObject> GUI = GameObjectsInView.Where(item => item.IsGUI).ToList();
            int GUICount = GUI.Count;
            for (int i = 0; i < GUICount; i++)
                GameObjectsInView.Remove(GUI[i]);

            while (GUICount > 0)
            {
                float tempHeight = GUI.FindMaxZ();
                GameObject[] toRender = GUI.Where(item => item.Transform.Position.Z == tempHeight).ToArray();

                /*Parallel.ForEach(toRender, (obj) =>
                {
                    obj.Render();
                    //GUI.Remove(obj);
                    GUICount--;
                });*/

                int toRenderCount = toRender.Length;
                for (int i = toRenderCount - 1; i >= 0; i--)
                {
                    toRender[i].Render();
                    GUI.Remove(toRender[i]);
                    GUICount--;
                }
            }

            RenderingGUI = false;
            int TempCount = GameObjectsInView.Count;

            while(TempCount > 0)
            {
                float tempHeight = GameObjectsInView.FindMaxZ();
                GameObject[] toRender = GameObjectsInView.Where(item => item.Transform.Position.Z == tempHeight).ToArray();

                /*Parallel.ForEach(toRender, (obj) =>
                {
                    obj.Render();
                    //Temp.Remove(obj);
                    TempCount--;
                });*/
                
                int toRenderCount = toRender.Length;
                for (int i = toRenderCount - 1; i >= 0; i--)
                {
                    toRender[i].Render();
                    GameObjectsInView.Remove(toRender[i]);
                    TempCount--;
                }
            }
        }

        private void BackGroundInit()
        {
            byte R = BackGround.R;
            byte G = BackGround.G;
            byte B = BackGround.B;

            int imageBufferLenght = Engine.Render.imageBuffer.Length;
            for(int i = 0; i < imageBufferLenght; i += 3)
            {
                Engine.Render.imageBuffer[i + 2] = R;
                Engine.Render.imageBuffer[i + 1] = G;
                Engine.Render.imageBuffer[i] = B;
            }
            
            Array.Clear(Engine.Render.imageBufferKey, 0, Engine.Render.imageBufferKey.Length);
        }

        public sealed override void Destroy()
        {
            if (Engine.BaseCam == this)
                Engine.BaseCam = null;

            Parent = null;
        }

        internal sealed override void Init()
        {
            this.Position = new Vector3(0, 0, 0);
            Engine.BaseCam = this;

            this.Name = Parent != null ? string.Format("{0}_{1}", Parent.Name, nameof(Camera)) : string.Format("{0}_{1}", nameof(Camera), Engine.LoadingScene.AllComponents.Count);
        }
    }
}
