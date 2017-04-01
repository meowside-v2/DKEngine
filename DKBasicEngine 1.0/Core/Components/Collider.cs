/*
* (C) 2017 David Knieradl 
*/

using DKEngine.Core.Ext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace DKEngine.Core.Components
{
    public class Collider : Component
    {
        internal event CollisionEnterHandler CollisionEvent;
        internal delegate void CollisionEnterHandler(Collider m);
        
        
        /// <summary>
        /// Determines size and position of collider
        /// </summary>
        public RectangleF Area;

        /// <summary>
        /// If is TRUE => Triggers OnColliderEnter once another GameObject enter this collider
        /// </summary>
        public bool IsTrigger = false;

        private float X { get { return Parent.Transform.Position.X + Area.X; } }
        private float Y { get { return Parent.Transform.Position.Y + Area.Y; } }
        private float Width { get { return Parent.Transform.Scale.X * Area.Width; } }
        private float Height { get { return Parent.Transform.Scale.Y * Area.Height; } }

        private bool _Right;
        private bool _Left;
        private bool _Top;
        private bool _Bottom;

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider (determines size of collider)</param>
        internal Collider(GameObject Parent)
            : base(Parent)
        {
            this.Area = new RectangleF(0, 0, Parent.Transform.Dimensions.X, Parent.Transform.Dimensions.Y);
            Engine.LoadingScene?.AllGameObjectsColliders.Add(this);

            this.Name = string.Format("{0}_Collider", Parent.Name);

            if (Engine.LoadingScene != null)
            {
                if (IsPartOfScene)
                    Engine.LoadingScene.AllComponents.AddSafe(this);
            }
        }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="collidableReference"></param>
        /// <param name="Xoffset">Horizontal offset</param>
        /// <param name="Yoffset">Vertical offset</param>
        /// <param name="Width">Width of collider</param>
        /// <param name="Height">Height of collider</param>
        internal Collider(GameObject Parent, float Xoffset, float Yoffset, float Width, float Height)
            :base(Parent)
        {
            this.Area = new RectangleF(Xoffset, Yoffset, Width, Height);
            Engine.LoadingScene?.AllGameObjectsColliders.Add(this);

            this.Name = string.Format("{0}_Collider", Parent.Name);

            if (Engine.LoadingScene != null)
            {
                if (IsPartOfScene)
                    Engine.LoadingScene.AllComponents.AddSafe(this);
            }
        }
        
        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider</param>
        /// <param name="Area">Determines size and position of collider</param>
        internal Collider(GameObject Parent, RectangleF Area)
            : base(Parent)
        {
            this.Area = Area;
            Engine.LoadingScene?.AllGameObjectsColliders.Add(this);

            this.Name = string.Format("{0}_Collider", Parent.Name);

            if (Engine.LoadingScene != null)
            {
                if (IsPartOfScene)
                    Engine.LoadingScene.AllComponents.AddSafe(this);
            }
        }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider</param>
        /// <param name="Coordinates">Determines position of collider</param>
        /// <param name="Size">Determines size of collider</param>
        internal Collider(GameObject Parent, PointF Coordinates, SizeF Size)
            : base(Parent)
        {
            this.Area = new RectangleF(Coordinates, Size);
            Engine.LoadingScene?.AllGameObjectsColliders.Add(this);

            this.Name = string.Format("{0}_Collider", Parent.Name);

            if (Engine.LoadingScene != null)
            {
                if (IsPartOfScene)
                    Engine.LoadingScene.AllComponents.AddSafe(this);
            }
        }

#if DEBUG

        /// <summary>
        /// Returns string containing <b>bool</b> value for each of the directions of this object.
        /// </summary>
        /// <returns></returns>
        public string DebugTestCollision()
        {
            return string.Format("Left {0}\nRight {1}\nTop {2}\nDown {3}", Collision(Direction.Left), Collision(Direction.Right), Collision(Direction.Up), Collision(Direction.Down));
        }
#endif
        /// <summary>
        /// Direction of the collision detection: Up, Down, Left, Right
        /// </summary>
        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        /// <summary>
        /// Collision check in specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool Collision(Direction direction)
        {
            if (this.IsTrigger)
                return false;

            if(LastUpdated != Engine.LastUpdated)
            {
                LastUpdated = Engine.LastUpdated;

                _Right = false;
                _Left = false;
                _Bottom = false;
                _Top = false;

                foreach(Collider tmp in Engine.CurrentScene.AllGameObjectsColliders)
                {
                    bool _L = false;
                    bool _R = false;
                    bool _T = false;
                    bool _B = false;

                    float _LeftSpan = float.MaxValue;
                    float _RightSpan = float.MaxValue;
                    float _BottomSpan = float.MaxValue;
                    float _TopSpan = float.MaxValue;

                    if (Left(tmp))
                    {
                        _L = true;
                        _LeftSpan = LeftSpan(tmp);
                        //this.Parent.Transform.Position += new Vector3(tmp.X + tmp.Width - this.X, 0, 0);
                        //_Left = true;
                    }

                    if (Right(tmp))
                    {
                        _R = true;
                        _RightSpan = RightSpan(tmp);
                        //this.Parent.Transform.Position -= new Vector3(this.X + this.Width - tmp.X, 0, 0);
                        //_Right = true;
                    }

                    if (Up(tmp))
                    {
                        _T = true;
                        _TopSpan = TopSpan(tmp);
                        //this.Parent.Transform.Position += new Vector3(0, tmp.Y + tmp.Height - this.Y, 0);
                        //_Top = true;
                    }

                    if (Down(tmp))
                    {
                        _B = true;
                        _BottomSpan = BottomSpan(tmp);
                        //this.Parent.Transform.Position -= new Vector3(0, this.Y + this.Height - tmp.Y, 0);
                        //_Bottom = true;
                    }

                    if (_T)
                    {
                        if (_TopSpan <= _LeftSpan && _TopSpan <= _RightSpan && _TopSpan <= _BottomSpan)
                        {
                            _Top = true;
                            this.Parent.Transform.Position += new Vector3(0, _TopSpan, 0);
                            continue;
                        }
                    }

                    if (_B)
                    {
                        if (_BottomSpan <= _LeftSpan && _BottomSpan <= _RightSpan && _BottomSpan <= _TopSpan)
                        {
                            _Bottom = true;
                            this.Parent.Transform.Position += new Vector3(0, -_BottomSpan, 0);
                            continue;
                        }
                    }

                    if (_L)
                    {
                        if (_LeftSpan <= _BottomSpan && _LeftSpan <= _TopSpan && _LeftSpan <= _RightSpan)
                        {
                            _Left = true;
                            this.Parent.Transform.Position += new Vector3(_LeftSpan, 0, 0);
                            continue;
                        }
                    }

                    if (_R)
                    {
                        if (_RightSpan <= _BottomSpan && _RightSpan <= _TopSpan && _RightSpan <= _LeftSpan)
                        {
                            _Right = true;
                            this.Parent.Transform.Position += new Vector3(-_RightSpan, 0, 0);
                            continue;
                        }
                    }
                }

                /*foreach (Collider tmp in Engine.CurrentScene.AllGameObjectsColliders)
                {
                    bool _L = false;
                    bool _R = false;
                    bool _T = false;
                    bool _B = false;

                    float _LeftSpan = 0f;
                    float _RightSpan = 0f;
                    float _BottomSpan = 0f;
                    float _TopSpan = 0f;

                    if (Left(tmp))
                    {
                        _L = true;
                        _LeftSpan = LeftSpan(tmp);
                        //this.Parent.Transform.Position += new Vector3(tmp.X + tmp.Width - this.X, 0, 0);
                        //_Left = true;
                    }

                    if (Right(tmp))
                    {
                        _R = true;
                        _RightSpan = RightSpan(tmp);
                        //this.Parent.Transform.Position -= new Vector3(this.X + this.Width - tmp.X, 0, 0);
                        //_Right = true;
                    }

                    if (Up(tmp))
                    {
                        _T = true;
                        _TopSpan = TopSpan(tmp);
                        //this.Parent.Transform.Position += new Vector3(0, tmp.Y + tmp.Height - this.Y, 0);
                        //_Top = true;
                    }

                    if (Down(tmp))
                    {
                        _B = true;
                        _BottomSpan = BottomSpan(tmp);
                        //this.Parent.Transform.Position -= new Vector3(0, this.Y + this.Height - tmp.Y, 0);
                        //_Bottom = true;
                    }

                    if (_T)
                    {
                        if(_TopSpan < _LeftSpan && _TopSpan < _RightSpan)
                        {
                            _Top = true;
                            this.Parent.Transform.Position += new Vector3(0, _TopSpan, 0);
                            continue;
                        }
                    }

                    if (_B)
                    {
                        if(_BottomSpan < _LeftSpan && _BottomSpan < _RightSpan)
                        {
                            _Bottom = true;
                            this.Parent.Transform.Position += new Vector3(0, -_BottomSpan, 0);
                            continue;
                        }
                    }

                    if (_L)
                    {
                        if(_LeftSpan < _BottomSpan && _LeftSpan < _TopSpan)
                        {
                            _Left = true;
                            this.Parent.Transform.Position += new Vector3(_LeftSpan, 0, 0);
                            continue;
                        }
                    }

                    if (_R)
                    {
                        if(_RightSpan < _BottomSpan && _RightSpan < _TopSpan)
                        {
                            _Right = true;
                            this.Parent.Transform.Position += new Vector3(-_RightSpan, 0, 0);
                            continue;
                        }
                    }
                }*/

                /*switch (direction)
                {
                    case Direction.Up:
                        if ((tmp = Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Up(obj2))) != null)
                        {
                            this.Parent.Transform.Position += new Vector3(0, tmp.Y + tmp.Height - this.Y, 0);
                            return true;
                        }
                        return false;
                    case Direction.Left:
                        if ((tmp = Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Left(obj2))) != null)
                        {
                            this.Parent.Transform.Position += new Vector3(tmp.X + tmp.Width - this.X, 0, 0);
                            return true;
                        }
                        return false;
                    case Direction.Down:
                        if ((tmp = Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Down(obj2))) != null)
                        {
                            this.Parent.Transform.Position += new Vector3(0, this.Y + this.Height - tmp.Y, 0);
                            return true;
                        }
                        return false;
                    case Direction.Right:
                        if ((tmp = Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Right(obj2))) != null)
                        {
                            this.Parent.Transform.Position -= new Vector3(this.X + this.Width - tmp.X, 0, 0);
                            return true;
                        }
                        return false;
                    default:
                        throw new Exception("WTF jak se ti to povedlo");
                }*/
            }


            switch (direction)
            {
                case Direction.Up:
                    return _Top;
                case Direction.Left:
                    return _Left;
                case Direction.Down:
                    return _Bottom;
                case Direction.Right:
                    return _Right;
                default:
                    return false;
            }
        }

        internal void TriggerCheck(List<GameObject> VisibleObjects)
        {
            int VisibleObjectsCount = VisibleObjects.Count;
            for (int i = 0; i < VisibleObjectsCount; i++)
            {
                Collider tmp = VisibleObjects[i].Collider;

                if (Left(tmp) || Right(tmp) || Up(tmp) || Down(tmp))
                {
                    //Debug.WriteLine(Parent.Name);
                    //CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    CollisionEvent?.Invoke(VisibleObjects[i].Collider);
                    continue;
                }
            }
        }

        /*private Collider.Direction Collision(Collider obj)
        {
            //Right collision
            if (this.X >= obj.X + obj.Width / 2 && this.X < obj.X + obj.Width)
            {
                //Bottom collision
                if(this.Y >= obj.Y + obj.Height / 2 && this.Y < obj.Y + obj.Height)
                {
                    return Direction.Down;
                }
                //Top collision
                else if (this.Y + this.Height >= obj.Y && this.Y + this.Height < obj.Y + obj.Height / 2)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Right;
                }
            }
            //Left collision
            else if (this.X + this.Width >= obj.X && this.X + this.Width < obj.X + obj.Width / 2)
            {
                //Bottom collision
                if (this.Y >= obj.Y + obj.Height / 2 && this.Y < obj.Y + obj.Height)
                {
                    return Direction.Down;
                }
                //Top collision
                else if (this.Y + this.Height >= obj.Y && this.Y + this.Height < obj.Y + obj.Height / 2)
                {
                    return Direction.Up;
                }
                else
                {
                    return Direction.Left;
                }
            }

            return Direction.None;
        }*/

        private float LeftSpan(Collider obj)
        {
            return obj.X + obj.Width - this.X;
        }

        private float TopSpan(Collider obj)
        {
            return obj.Y + obj.Height - this.Y;
        }

        private float RightSpan(Collider obj)
        {
            
            return this.X + this.Width - obj.X;
        }

        private float BottomSpan(Collider obj)
        {
            
            return this.Y + this.Height - obj.Y;
        }

        private bool Left(Collider obj)
        {
            if (!this.Equals(obj) && !obj.IsTrigger)
                return (this.Y < obj.Y + obj.Height && this.Y + this.Height > obj.Y && this.X >= obj.X + obj.Width / 2 && this.X <= obj.X + obj.Width); //(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X <= obj.X + obj.Width && this.X > obj.X);

            return false;
        }

        private bool Right(Collider obj)
        {
            if (!this.Equals(obj) && !obj.IsTrigger)
                return (this.Y < obj.Y + obj.Height && this.Y + this.Height > obj.Y && this.X + this.Width >= obj.X && this.X + this.Width <= obj.X + obj.Width / 2);//(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X + this.Width >= obj.X && this.X < X);

            return false;
        }

        private bool Up(Collider obj)
        {
            if (!this.Equals(obj) && !obj.IsTrigger)
                return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Height && this.Y >= obj.Y + obj.Height / 2);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Width && this.Y > obj.Y);

            return false;
        }

        private bool Down(Collider obj)
        {
            if (!this.Equals(obj) && !obj.IsTrigger)
                return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Height >= obj.Y && this.Y + this.Height <= obj.Y + obj.Height / 2);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Width >= obj.Y && this.Y < obj.Y);

            return false;
        }
        
        public static void SetNewCollider(Collider destination, RectangleF Area)
        {
            destination.Area = Area;
        }

        public static void SetNewCollider(Collider destination, PointF Point, SizeF Size)
        {
            destination.Area = new RectangleF(Point, Size);
        }

        public static void SetNewCollider(Collider destination, float X, float Y, float Width, float Height)
        {
            destination.Area = new RectangleF(X, Y, Width, Height);
        }

        public override void Destroy()
        {
            Engine.CurrentScene.AllGameObjectsColliders.Remove(this);
            if (Parent.Collider == this)
                Parent.Collider = null;
        }

        public void SetCollisionManually(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    _Top = true;
                    break;
                case Direction.Left:
                    _Left = true;
                    break;
                case Direction.Down:
                    _Bottom = true;
                    break;
                case Direction.Right:
                    _Right = true;
                    break;
                default:
                    break;
            }
        }
    }
}
