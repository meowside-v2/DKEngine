using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Scene : ICore, IPage
    {
        public string Name { get; set; } = "";

        public List<I3Dimensional> Model { get; private set; } = new List<I3Dimensional>();

        public int FocusSelection { get; set; } = 0;
        public List<IControl> PageControls { get; } = new List<IControl>();

        public Scene() { this.Start(); }

        public enum Mode
        {
            View,
            Edit
        }

        public void New(string Name)
        {
            this.Name = Name;
        }

        public void Init(string path, Mode mode)
        {
            BinaryReader br;

            try
            {
                br = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            catch (IOException e)
            {
                throw new SceneInitFailedException(path + "\nWorld wasn't found", e);
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
                throw new SceneInitFailedException("World loading failed", e);
            }

            br.Close();
        }

        public void Start()
        {
            lock (Engine.ToUpdate)
            {
                Engine.ToUpdate.Add(this);
            }
        }

        public void Update()
        {
            if(PageControls.Count > 1)
            {
                if (Engine.Input.IsKeyPressed(ConsoleKey.UpArrow))
                {
                    if (FocusSelection > 0)
                        FocusSelection--;
                }

                if (Engine.Input.IsKeyPressed(ConsoleKey.DownArrow))
                {
                    if (FocusSelection < PageControls.Count - 1)
                        FocusSelection++;
                }
            }
        }

        public object DeepCopy()
        {
            Scene temp = (Scene)MemberwiseClone();
            temp.Model = Model.ToList();

            return temp;
        }

        public void Render()
        {
            List<I3Dimensional> temp = Model.GetGameObjectsInView();
            
            while (temp.Count > 0)
            {
                if (Engine.Render.imageBufferKey.BufferIsFull(255, 1))
                    return;

                double tempHeight = temp.FindMaxZ();
                List<I3Dimensional> toRender = temp.Where(item => (item).Z == tempHeight).ToList();

                foreach (ICore item in toRender)
                {
                    item.Render();
                }

                temp.RemoveAll(item => toRender.FirstOrDefault(item2 => ReferenceEquals(item, item2)) != null);
            }
        }
    }
}
