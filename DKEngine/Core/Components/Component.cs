using DKEngine.Core.Ext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        internal Component(GameObject Parent)
        {
            this.Parent = Parent;
            LastUpdated = Engine.LastUpdated;

            try
            {
                Engine.LoadingScene.NewlyGeneratedComponents.Push(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        internal void InitInternal()
        {
            Init();

            try
            {
                Engine.LoadingScene.AllComponents.AddSafe(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        internal virtual void Init()
        { }

        public abstract void Destroy();
        
        public static T Find<T>(string Name) where T : Component
        {
            T retValue = null;

            try
            {
                retValue = (T)Engine.LoadingScene.AllComponents[Name];
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Object not found\n" + ex);
            }

            return retValue;
        }
    }
}
