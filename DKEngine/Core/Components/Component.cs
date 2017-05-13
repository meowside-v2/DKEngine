using DKEngine.Core.Ext;
using DKEngine.Core.UI;
using System;
using System.Diagnostics;

namespace DKEngine.Core.Components
{
    /// <summary>
    /// Base class for all objects using DKEngine library
    /// </summary>
    public abstract class Component
    {
        private TimeSpan _lastUpdated;

        internal TimeSpan LastUpdated
        {
            get
            {
                TimeSpan tmp = _lastUpdated;
                _lastUpdated = Engine.LastUpdated;
                return tmp;
            }
        }

        /// <summary>
        /// The parent object of this instance
        /// </summary>
        public GameObject Parent = null;

        /// <summary>
        /// The name of this instance
        /// </summary>
        public string Name = "";

        internal Component(GameObject Parent)
        {
            this.Parent = Parent;
            _lastUpdated = Engine.LastUpdated;

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
                if (this.GetType() != typeof(Letter))
                {
                    Engine.LoadingScene.AllComponents.AddSafe(this);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }

        internal virtual void Init()
        { }

        public abstract void Destroy();

        /// <summary>
        /// Finds the specified component of specified name.
        /// </summary>
        /// <typeparam name="T">Determines type of desired component</typeparam>
        /// <param name="Name">The name of desired component.</param>
        /// <returns></returns>
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