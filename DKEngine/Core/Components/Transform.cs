/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public sealed class Transform : Component
    {
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
                _ScaledDimensions = _Dimensions * _Scale;

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
                _ScaledDimensions = _Dimensions * _Scale;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Scale *= tmp;
            }
        }

        internal Vector3 _ScaledDimensions;
        
        public Transform(GameObject Parent)
            :base(Parent)
        {
            _Position         = new Vector3();
            _Dimensions       = new Vector3();
            _Scale            = new Vector3();
            _ScaledDimensions = new Vector3();
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
 /*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0.Core.Components
{
    public sealed class Transform : Component, I3Dimensional
    {
        private Vector3 _Dimensions;
        private Vector3 _Position;
        private Vector3 _Scale;

        public Vector3 Dimensions
        {
            get { return _Dimensions; }
            set
            {
                Vector3 tmp = value.Decrease(_Dimensions);
                _Dimensions = value;
                _ScaledDimensions = _Dimensions.Multiply(_Scale);

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Dimensions.Add(tmp);
            }
        }
        public Vector3 Position
        {
            get { return _Position; }
            set
            {
                Vector3 tmp = value.Decrease(_Position);
                _Position = value;

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Position.Add(tmp);
            }
        }
        public Vector3 Scale
        {
            get { return _Scale; }
            set
            {
                Vector3 tmp = value.Divide(_Scale);
                _Scale = value;
                _ScaledDimensions = _Dimensions.Multiply(_Scale);

                int childCount = Parent.Child.Count;
                for (int i = 0; i < childCount; i++)
                    Parent.Child[i].Transform.Scale.Multiply(tmp);
            }
        }

        internal Vector3 _ScaledDimensions;

        public Transform(GameObject Parent)
            : base(Parent)
        {
            _Position = new Vector3();
            _Dimensions = new Vector3();
            _Scale = new Vector3();
            _ScaledDimensions = new Vector3();
        }

        protected internal override void Destroy()
        { }
    }
}
*/