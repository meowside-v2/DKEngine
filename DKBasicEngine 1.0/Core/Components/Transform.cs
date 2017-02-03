/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Transform : I3Dimensional
    {
        GameObject Parent;

        private Vector3 _Dimensions;
        private Vector3 _Position;
        private Vector3 _Scale;

        public Vector3 Dimensions
        {
            get { return _Dimensions; }
            set
            {
                Vector3 tmp = value - _Dimensions;
                _Dimensions = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Dimensions += tmp;
            }
        }
        public Vector3 Position
        {
            get { return _Position; }
            set
            {
                Vector3 tmp = value - _Position;
                _Position = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Position += tmp;
            }
        }
        public Vector3 Scale
        {
            get { return _Scale; }
            set
            {
                Vector3 tmp = value / _Scale;
                _Scale = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Scale *= tmp;
            }
        }
        
        public Transform(GameObject Parent)
        {
            this.Parent = Parent;

            _Position = new Vector3();
            _Dimensions = new Vector3();
            _Scale = new Vector3();
        }
    }
}
