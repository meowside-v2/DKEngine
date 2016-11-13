using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Scene : ICore
    {
        public string Name { get; set; }

        public List<I3Dimensional> Model = new List<I3Dimensional>();

        public int PlayerSpawnX { get; set; }
        public int PlayerSpawnY { get; set; }
        public int PlayerSpawnZ { get; set; }

        public Scene() { }

        public enum Mode
        {
            Game,
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

                this.PlayerSpawnX = br.ReadInt32();
                this.PlayerSpawnY = br.ReadInt32();
                this.PlayerSpawnZ = br.ReadInt32();

                int temp_ModelCount = br.ReadInt32();

                this.Model.Clear();

                for (int count = 0; count < temp_ModelCount; count++)
                {
                    Model.Add(new GameObject(br.ReadString(),
                                         br.ReadInt32(),
                                         br.ReadInt32(),
                                         br.ReadInt32()));
                }

                switch (mode)
                {
                    case Mode.Game:
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

        public object DeepCopy()
        {
            Scene temp = (Scene)MemberwiseClone();
            temp.Model = Model.ToList();

            return temp;
        }

        public void Render(int x, int y, byte[] bufferData, bool[] bufferKey)
        {
            List<I3Dimensional> temp;

            lock (Model)
            {
                temp = Model.ToList().Where(item => Finder(item, x, y)).ToList();
            }

            while (temp.Count > 0)
            {
                if (BufferIsFull(bufferKey))
                    return;

                int tempHeight = temp.Max(item => (item).Z);
                List<I3Dimensional> toRender = temp.Where(item => (item).Z == tempHeight).ToList();

                foreach (ICore item in toRender)
                {
                    item.Render(x, y, bufferData, bufferKey);
                }

                temp.RemoveAll(item => toRender.FirstOrDefault(item2 => ReferenceEquals(item, item2)) != null);
            }
            //model.Render(x, y, bufferData, bufferKey);
        }

        private bool Finder(I3Dimensional obj, int x, int y)
        {
            return (obj.X + obj.width >= x && obj.X < x + Shared.RenderWidth && obj.Y + obj.height >= y && obj.Y < y + Shared.RenderHeight);
        }

        private bool FindBiggerZ(I3Dimensional item1, I3Dimensional item2)
        {
            return (item1.Z > item2.Z);
        }

        private bool BufferIsFull(bool[] key)
        {
            return !(key.Contains(false));
        }
    }
}
