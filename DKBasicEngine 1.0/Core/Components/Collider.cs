using System.Collections.Generic;
using System.Drawing;

namespace DKBasicEngine_1_0
{
    public class Collider
    {
        /// <summary>
        /// Parent of collider
        /// </summary>
        public GameObject Parent;
        
        /// <summary>
        /// Determines size and position of collider
        /// </summary>
        public Rectangle Area;

        /// <summary>
        /// If is TRUE => Triggers OnColliderEnter once another collider enter this collider
        /// </summary>
        public bool IsTrigger = false;

        private float X { get { return Parent.X + Area.X; } }
        private float Y { get { return Parent.Y + Area.Y; } }
        private float Width { get { return Parent.Width + Area.Width; } }
        private float Height { get { return Parent.Height + Area.Height; } }

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
                    return Engine.Collidable.FirstOrDefault(obj2 => FindUp(obj2)) != null;
                case Direction.Left:
                    return Engine.Collidable.FirstOrDefault(obj2 => FindLeft(obj2)) != null;
                case Direction.Down:
                    return Engine.Collidable.FirstOrDefault(obj2 => FindDown(obj2)) != null;
                case Direction.Right:
                    return Engine.Collidable.FirstOrDefault(obj2 => FindRight(obj2)) != null;
                default:
                    throw new System.Exception("WTF jak se ti to povedlo");
            }
        }

        private bool FindLeft(Collider obj)
        {
            if (Parent.Collider != null)
                if (this != obj)
                    return (this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X <= obj.X + obj.Width && this.X > obj.X);

            return false;
        }

        private bool FindRight(Collider obj)
        {
            if (Parent.Collider != null)
                if(this != obj)
                    return (this.Y < obj.Y + obj.Width && this.Y + this.Width > obj.Y && this.X + this.Width >= obj.X && this.X < X);

            return false;
        }

        private bool FindUp(Collider obj)
        {
            if (Parent.Collider != null)
                if (this != obj)
                    return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y <= obj.Y + obj.Width && this.Y > obj.Y);

            return false;
        }

        private bool FindDown(Collider obj)
        {
            if (Parent.Collider != null)
                if (this != obj)
                    return (this.X < obj.X + obj.Width && this.X + this.Width > obj.X && this.Y + this.Width >= obj.Y && this.Y < obj.Y);

            return false;
        }
    }
}
