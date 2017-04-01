using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DKEngine.Core
{
    public class Scene : IPage
    {
        public string Name = "";

        internal readonly Dictionary<string, Component> AllComponents;
        internal readonly Dictionary<string, GameObject> AllGameObjects;

        internal readonly List<GameObject> Model;
        internal readonly List<GameObject> NewlyGeneratedGameObjects;
        internal readonly List<Behavior>   AllBehaviors;
        internal readonly List<Behavior>   NewlyGeneratedComponents;
        internal readonly List<Collider>   AllGameObjectsColliders;

        public Scene()
        {
            AllComponents = new Dictionary<string, Component>(0xFFFF);
            AllGameObjects = new Dictionary<string, GameObject>(0xFFFF);

            AllBehaviors             = new List<Behavior>(0xFFFF);
            NewlyGeneratedGameObjects = new List<GameObject>(0xFFFF);
            Model                     = new List<GameObject>(0xFFFF);
            NewlyGeneratedComponents  = new List<Behavior>(0xFFFF);
            AllGameObjectsColliders   = new List<Collider>(0xFFFF);
        }

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
