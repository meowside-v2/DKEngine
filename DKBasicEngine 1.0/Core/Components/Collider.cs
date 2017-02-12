/*
* (C) 2017 David Knieradl 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace DKBasicEngine_1_0.Core.Components
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

        public bool IsCollidable = false;

        private float X { get { return Parent.Transform.Position.X + Area.X; } }
        private float Y { get { return Parent.Transform.Position.Y + Area.Y; } }
        private float Width { get { return Parent.Transform.Scale.X * Area.Width; } }
        private float Height { get { return Parent.Transform.Scale.Y * Area.Height; } }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider (determines size of collider)</param>
        internal Collider(GameObject Parent)
            : base(Parent)
        {
            this.Area = new RectangleF(0, 0, Parent.Transform.Dimensions.X, Parent.Transform.Dimensions.Y);
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
            switch (direction)
            {
                case Direction.Up:
                    return Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Up(obj2)) != null;
                case Direction.Left:
                    return Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Left(obj2)) != null;
                case Direction.Down:
                    return Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Down(obj2)) != null;
                case Direction.Right:
                    return Engine.CurrentScene.AllGameObjectsColliders.FirstOrDefault(obj2 => Right(obj2)) != null;
                default:
                    throw new Exception("WTF jak se ti to povedlo");
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
                    Debug.WriteLine(Parent.Name);
                    //CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    CollisionEvent?.Invoke(VisibleObjects[i].Collider);
                    continue;
                }
            }
        }

        private bool Left(Collider obj)
        {
            if (this != obj && obj.IsCollidable)
                return (this.Y <= obj.Y + obj.Height && this.Y + this.Height >= obj.Y && this.X <= obj.X + obj.Width && this.X >= obj.X); //(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X <= obj.X + obj.Width && this.X > obj.X);

            return false;
        }

        private bool Right(Collider obj)
        {
            if (this != obj && obj.IsCollidable)
                return (this.Y <= obj.Y + obj.Height && this.Y + this.Height >= obj.Y && this.X + this.Width >= obj.X && this.X <= obj.X);//(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X + this.Width >= obj.X && this.X < X);

            return false;
        }

        private bool Up(Collider obj)
        {
            if (this != obj && obj.IsCollidable)
                return (this.X <= obj.X + obj.Width && this.X + this.Width >= obj.X && this.Y <= obj.Y + obj.Height && this.Y >= obj.Y);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Width && this.Y > obj.Y);

            return false;
        }

        private bool Down(Collider obj)
        {
            if (this != obj && obj.IsCollidable)
                return (this.X <= obj.X + obj.Width && this.X + this.Width >= obj.X && this.Y + this.Height >= obj.Y && this.Y <= obj.Y);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Width >= obj.Y && this.Y < obj.Y);

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

        protected internal override void Destroy()
        {
            Engine.CurrentScene.AllGameObjectsColliders.Remove(this);
            if (Parent.Collider == this)
                Parent.Collider = null;
        }
    }
}
