using DKEngine.Core.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DKEngine.Core
{
    public abstract class Scene : IPage
    {
        public string Name = "";

        internal readonly Dictionary<string, Component> AllComponents;
        //internal readonly Dictionary<string, GameObject> AllGameObjects;

        internal readonly List<GameObject> Model;
        internal readonly List<Behavior>   AllBehaviors;
        internal readonly List<Collider>   AllGameObjectsColliders;

        internal readonly Stack<Component> NewlyGeneratedComponents;
        //internal readonly Stack<Behavior> NewlyGeneratedBehaviors;

        internal readonly Stack<GameObject> GameObjectsToAddToRender;
        internal readonly Stack<GameObject> GameObjectsAddedToRender;

        public Scene()
        {
            AllComponents = new Dictionary<string, Component>(0xFFFF);
            //AllGameObjects = new Dictionary<string, GameObject>(0xFFFF);

            AllBehaviors              = new List<Behavior>(0xFFFF);
            Model                     = new List<GameObject>(0xFFFF);
            AllGameObjectsColliders   = new List<Collider>(0xFFFF);

            NewlyGeneratedComponents = new Stack<Component>(0xFFFF);
            //NewlyGeneratedBehaviors   = new Stack<Behavior>(0xFFFF);

            GameObjectsToAddToRender  = new Stack<GameObject>(0xFFFF);
            GameObjectsAddedToRender  = new Stack<GameObject>(0xFFFF);
        }

        
        public abstract void Init();
        public abstract void Set(params string[] Args);
        public abstract void Unload();

        #region Nechapu_K_Cemu_To_Tady_Jeste_Je
        /*public enum Mode
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

        /*switch (mode)
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
}*/

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
        #endregion
    }
}
