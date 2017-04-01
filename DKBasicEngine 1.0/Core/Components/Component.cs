using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DKEngine.Core.Components
{
    public abstract class Component
    {
        internal long LastUpdated;

        public GameObject Parent { get; set; } = null;
        public  string Name { get; set; } = "";
        internal bool IsPartOfScene { get; set; } = true;

        internal Component(GameObject Parent)
        {
            this.Parent = Parent;
            LastUpdated = Engine.LastUpdated;
        }

        public abstract void Destroy();

        public static T Find<T>(string Name) where T : Component
        {
            T retValue = null;

            try
            {
                retValue = (T)Engine.CurrentScene.AllComponents[Name];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Object not found\n" + ex);
            }

            return retValue;
        }
    }
}
