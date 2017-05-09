/*
* (C) 2017 David Knieradl
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static DKEngine.Core.Components.Transform;

namespace DKEngine.Core.Components
{
    public class Collider : Component
    {
        internal event CollisionEnterHandler CollisionEvent;

        internal delegate void CollisionEnterHandler(Collider m);

        /// <summary>
        /// Determines size and position of collider
        /// </summary>
        public RectangleF Area = new RectangleF();

        /// <summary>
        /// If is TRUE => Triggers OnColliderEnter once another GameObject enter this collider
        /// </summary>
        public bool IsTrigger = false;
        public bool Enabled = true;

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

            this.Name = string.Format("{0}_{1}", Parent.Name, nameof(Collider));
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
        /// Collision check in specified direction.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool Collision(Direction direction)
        {
            if (this.IsTrigger || !this.Enabled)
                return false;

            if (LastUpdated != Engine.LastUpdated)
            {
                _Right = false;
                _Left = false;
                _Bottom = false;
                _Top = false;

                int count = Engine.CurrentScene.AllGameObjectsColliders.Count;
                for (int i = 0; i < count; i++)
                {
                    Collider tmp = Engine.CurrentScene.AllGameObjectsColliders[i];

                    bool _L = false;
                    bool _R = false;
                    bool _T = false;
                    bool _B = false;

                    float _LeftSpan = float.MaxValue;
                    float _RightSpan = float.MaxValue;
                    float _BottomSpan = float.MaxValue;
                    float _TopSpan = float.MaxValue;

                    if (_L = Left(tmp))
                    {
                        _LeftSpan = LeftSpan(tmp);
                    }

                    if (_R = Right(tmp))
                    {
                        _RightSpan = RightSpan(tmp);
                    }

                    if (_T = Up(tmp))
                    {
                        _TopSpan = TopSpan(tmp);
                    }

                    if (_B = Down(tmp))
                    {
                        _BottomSpan = BottomSpan(tmp);
                    }

                    if (_T && _TopSpan <= _LeftSpan && _TopSpan <= _RightSpan && _TopSpan <= _BottomSpan)
                    {
                        _Top = true;
                        this.Parent.Transform.Position += new Vector3(0, _TopSpan, 0);
                        continue;
                    }

                    if (_B && _BottomSpan <= _LeftSpan && _BottomSpan <= _RightSpan && _BottomSpan <= _TopSpan)
                    {
                        _Bottom = true;
                        this.Parent.Transform.Position += new Vector3(0, -_BottomSpan, 0);
                        continue;
                    }

                    if (_L && _LeftSpan <= _BottomSpan && _LeftSpan <= _TopSpan && _LeftSpan <= _RightSpan)
                    {
                        _Left = true;
                        this.Parent.Transform.Position += new Vector3(_LeftSpan, 0, 0);
                        continue;
                    }

                    if (_R && _RightSpan <= _BottomSpan && _RightSpan <= _TopSpan && _RightSpan <= _LeftSpan)
                    {
                        _Right = true;
                        this.Parent.Transform.Position += new Vector3(-_RightSpan, 0, 0);
                        continue;
                    }
                }
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
            if (!this.Enabled)
                return;

            int VisibleObjectsCount = VisibleObjects.Count;
            for (int i = 0; i < VisibleObjectsCount; i++)
            {
                Collider tmp = VisibleObjects[i].Collider;

                if (!tmp.Enabled)
                    continue;

                if (Collided(tmp))
                {
                    CollisionEvent?.Invoke(VisibleObjects[i].Collider);
                    continue;
                }
            }
        }
        
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
            try
            {
                if (!this.Equals(obj) && !obj.IsTrigger && obj.Enabled)
                    return (this.Y < obj.Y + obj.Height && this.Y + this.Height > obj.Y && this.X >= obj.X + obj.Width / 2 && this.X <= obj.X + obj.Width); //(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X <= obj.X + obj.Width && this.X > obj.X);
            }
            catch { }

            return false;
        }

        private bool Right(Collider obj)
        {
            try
            {
                if (!this.Equals(obj) && !obj.IsTrigger && obj.Enabled)
                    return (this.Y < obj.Y + obj.Height && this.Y + this.Height > obj.Y && this.X + this.Width >= obj.X && this.X + this.Width <= obj.X + obj.Width / 2);//(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X + this.Width >= obj.X && this.X < X);
            }
            catch { }

            return false;
        }

        private bool Up(Collider obj)
        {
            try
            {
                if (!this.Equals(obj) && !obj.IsTrigger && obj.Enabled)
                    return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Height && this.Y >= obj.Y + obj.Height / 2);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Width && this.Y > obj.Y);
            }
            catch { }

            return false;
        }

        private bool Down(Collider obj)
        {
            try
            {
                if (!this.Equals(obj) && !obj.IsTrigger && obj.Enabled)
                    return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Height >= obj.Y && this.Y + this.Height <= obj.Y + obj.Height / 2);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Width >= obj.Y && this.Y < obj.Y);
            }
            catch { }

            return false;
        }

        private bool Collided(Collider obj)
        {
            try
            {
                if (!this.Equals(obj) && !obj.IsTrigger && obj.Enabled)
                    return (this.X <= obj.X + obj.Width && this.X + this.Width >= obj.X && this.Y <= obj.Y + obj.Height && this.Y + this.Height >= obj.Y);
            }
            catch { }

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
            try
            {
                Engine.CurrentScene.AllGameObjectsColliders.Remove(this);
            }
            catch { }

            try
            {
                Engine.CurrentScene.AllComponents.Remove(this.Name);
            }
            catch
            { }

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

        internal sealed override void Init()
        {
            try
            {
                Engine.LoadingScene.AllGameObjectsColliders.Add(this);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Loading scene is NULL\n\n{0}", e);
            }
        }
    }
}