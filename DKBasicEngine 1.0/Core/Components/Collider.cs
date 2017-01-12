using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Collider
    {
        public event CollisionEnterHandler CollisionEvent;
        public delegate void CollisionEnterHandler(Collider m);
        /// <summary>
        /// Parent of collider
        /// </summary>
        public GameObject Parent;
        
        /// <summary>
        /// Determines size and position of collider
        /// </summary>
        public Rectangle Area;

        /// <summary>
        /// If is TRUE => Triggers OnColliderEnter once another GameObject enter this collider
        /// </summary>
        public bool IsTrigger = false;

        private float X { get { return Parent.X + Area.X; } }
        private float Y { get { return Parent.Y + Area.Y; } }
        private float Width { get { return Area.Width; } }
        private float Height { get { return Area.Height; } }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="collidableReference"></param>
        /// <param name="Xoffset"></param>
        /// <param name="Yoffset"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Collider(GameObject Parent, int Xoffset, int Yoffset, int Width, int Height)
        {
            this.Parent = Parent;
            this.Area = new Rectangle(Xoffset, Yoffset, Width, Height);
            lock (Engine.Collidable)
                Engine.Collidable.Add(this);

            CollisionEvent += new CollisionEnterHandler(Parent.OnColliderEnter);
        }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider (determines size of collider)</param>
        public Collider(GameObject Parent)
        {
            this.Parent = Parent;
            this.Area = new Rectangle(0, 0, (int)Parent.Width, (int)Parent.Height);
            lock (Engine.Collidable)
                Engine.Collidable.Add(this);

            CollisionEvent += new CollisionEnterHandler(Parent.OnColliderEnter);
        }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider</param>
        /// <param name="Area">Determines size and position of collider</param>
        public Collider(GameObject Parent, Rectangle Area)
        {
            this.Parent = Parent;
            this.Area = Area;
            lock (Engine.Collidable)
                Engine.Collidable.Add(this);

            CollisionEvent += new CollisionEnterHandler(Parent.OnColliderEnter);
        }

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent">Parent of collider</param>
        /// <param name="Coordinates">Determines position of collider</param>
        /// <param name="Size">Determines size of collider</param>
        public Collider(GameObject Parent, Point Coordinates, Size Size)
        {
            this.Parent = Parent;
            this.Area = new Rectangle(Coordinates, Size);
            lock (Engine.Collidable)
                Engine.Collidable.Add(this);

            CollisionEvent += new CollisionEnterHandler(Parent.OnColliderEnter);
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

        public Rectangle GetCollider()
        {
            return Area;
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
                    return Engine.Collidable.FirstOrDefault(obj2 => Up(obj2)) != null;
                case Direction.Left:
                    return Engine.Collidable.FirstOrDefault(obj2 => Left(obj2)) != null;
                case Direction.Down:
                    return Engine.Collidable.FirstOrDefault(obj2 => Down(obj2)) != null;
                case Direction.Right:
                    return Engine.Collidable.FirstOrDefault(obj2 => Right(obj2)) != null;
                default:
                    throw new Exception("WTF jak se ti to povedlo");
            }
        }

        internal void TriggerCheck(List<GameObject> VisibleObjects)
        {
            int VisibleObjectsCount = VisibleObjects.Count;
            for (int i = 0; i < VisibleObjectsCount; i++)
            {
                if (Left(VisibleObjects[i].Collider))
                {
                    Debug.WriteLine("Left");
                    CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    continue;

                }
                else if (Right(VisibleObjects[i].Collider))
                {
                    Debug.WriteLine("Right");
                    CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    continue;
                }

                else if (Up(VisibleObjects[i].Collider))
                {
                    Debug.WriteLine("Up");
                    CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    continue;
                }

                else if (Down(VisibleObjects[i].Collider))
                {
                    Debug.WriteLine("Down");
                    CollisionEvent?.DynamicInvoke(VisibleObjects[i].Collider);
                    //this.Parent.OnColliderEnter(VisibleObjects[i].Collider);
                    continue;
                }
            }
        }

        private bool Left(Collider obj)
        {
            if (this != obj && !obj.IsTrigger)
                return (this.Y <= obj.Y + obj.Height && this.Y + this.Height >= obj.Y && this.X <= obj.X + obj.Width && this.X >= obj.X); //(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X <= obj.X + obj.Width && this.X > obj.X);

            return false;
        }

        private bool Right(Collider obj)
        {
            if (this != obj && !obj.IsTrigger)
                return (this.Y <= obj.Y + obj.Height && this.Y + this.Height >= obj.Y && this.X + this.Width >= obj.X && this.X <= obj.X);//(this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X + this.Width >= obj.X && this.X < X);

            return false;
        }

        private bool Up(Collider obj)
        {
            if (this != obj && !obj.IsTrigger)
                return (this.X <= obj.X + obj.Width && this.X + this.Width >= obj.X && this.Y <= obj.Y + obj.Height && this.Y >= obj.Y);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Width && this.Y > obj.Y);

            return false;
        }

        private bool Down(Collider obj)
        {
            if (this != obj && !obj.IsTrigger)
                return (this.X <= obj.X + obj.Width && this.X + this.Width >= obj.X && this.Y + this.Height >= obj.Y && this.Y <= obj.Y);//(this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Width >= obj.Y && this.Y < obj.Y);

            return false;
        }

        public void Destroy()
        {
            Engine.Collidable.Remove(this);
            if(Parent.Collider == this)
                Parent.Collider = null;

            Parent = null;
        }
    }
}
