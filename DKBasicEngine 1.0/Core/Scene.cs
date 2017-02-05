using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DKBasicEngine_1_0.Core
{
    public class Scene : IPage
    {
        public string Name = "";

        internal readonly List<GameObject> Model          = new List<GameObject>();
        internal readonly List<GameObject> NewlyGenerated = new List<GameObject>();
        //internal readonly List<GameObject> AllGameObjects = new List<GameObject>();

        public Scene()
        { }

        public enum Mode
        {
            View,
            Edit
        }

        public void New(string Name)
        {
            this.Name = Name;
        }

        internal void Init(string path, Mode mode)
        {
            BinaryReader br;

            try
            {
                br = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            catch (IOException e)
            {
                throw new Exception(path + "\nWorld wasn't found", e);
            }

            try
            {
                this.Name = br.ReadString();
                int temp_ModelCount = br.ReadInt32();
                this.Model.Clear();

                /*for (int count = 0; count < temp_ModelCount; count++)
                {
                    Model.Add(new GameObject()
                                {
                                    TypeName = br.ReadString(),
                                    X = br.ReadInt32(),
                                    Y = br.ReadInt32(),
                                    Z = br.ReadInt32()
                                });
                }*/

                switch (mode)
                {
                    case Mode.View:
                        break;

                    case Mode.Edit:
                        break;

                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception("World loading failed", e);
            }

            br.Close();
        }

        protected internal virtual void Init()
        { }

        /*public virtual void Start() { }

        public virtual void Update()
        {
            if (PageControls.Count > 1)
            {
                if(TimeOutControls.Elapsed > TimeOut)
                    TimeOutControls.Reset();

                if(TimeOutControls.ElapsedMilliseconds == 0)
                {
                    if (Engine.Input.IsKeyPressed(ConsoleKey.UpArrow))
                    {
                        TimeOutControls.Start();

                        if (FocusSelection > 0)
                            FocusSelection--;
                    }

                    if (Engine.Input.IsKeyPressed(ConsoleKey.DownArrow))
                    {
                        TimeOutControls.Start();

                        if (FocusSelection < PageControls.Count - 1)
                            FocusSelection++;
                    }
                }
            }
        }

        public void Destroy()
        {

        }

        public void Render()
        {
            
        }*/
    }
}
