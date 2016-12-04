using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Collider
    {
        List<I3Dimensional> collidableReference;
        GameObject Parent;

        Rectangle Area;

        /// <summary>
        /// Creates new Instance of Collider class
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="collidableReference"></param>
        /// <param name="Xoffset"></param>
        /// <param name="Yoffset"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public Collider(GameObject Parent, List<I3Dimensional> collidableReference, int Xoffset, int Yoffset, int Width, int Height)
        {
            this.Parent = Parent;
            this.collidableReference = collidableReference;

            this.Area = new Rectangle(Xoffset, Yoffset, Width, Height);
        }

        public Collider(GameObject Parent, List<I3Dimensional> collidableReference)
        {
            this.Parent = Parent;
            this.collidableReference = collidableReference;

            this.Area = new Rectangle(0, 0, (int)Parent.width, (int)Parent.height);
        }

        public Collider(GameObject Parent, List<I3Dimensional> collidableReference, Rectangle Area)
        {
            this.Parent = Parent;
            this.collidableReference = collidableReference;

            this.Area = Area;
        }

        public Collider(GameObject Parent, List<I3Dimensional> collidableReference, Point Coordinates, Size _Size)
        {
            this.Parent = Parent;
            this.collidableReference = collidableReference;

            this.Area = new Rectangle(Coordinates, _Size);
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
            I3Dimensional nearestObject = null;

            switch (direction)
            {
                case Direction.Up:
                    nearestObject = collidableReference.Find(obj2 => FindUp(Parent, obj2));
                    break;
                case Direction.Left:
                    nearestObject = collidableReference.Find(obj2 => FindLeft(Parent, obj2));
                    break;
                case Direction.Down:
                    nearestObject = collidableReference.Find(obj2 => FindDown(Parent, obj2));
                    break;
                case Direction.Right:
                    nearestObject = collidableReference.Find(obj2 => FindRight(Parent, obj2));
                    break;
                default:
                    break;
            }

            if (nearestObject != null)
                return true;

            return false;
        }

        private bool FindLeft(I3Dimensional obj1, I3Dimensional obj2)
        {
            if (Parent.collider != null)
                return (obj1.Y < obj2.Y + obj2.width && obj1.Y + obj1.width > obj2.Y && obj1.X <= obj2.X + obj2.width && obj1.X > obj2.X && obj1 != obj2);

            return false;
        }

        private bool FindRight(I3Dimensional obj1, I3Dimensional obj2)
        {
            if (Parent.collider != null)
                return (obj1.Y < obj2.Y + obj2.width && obj1.Y + obj1.width > obj2.Y && obj1.X + obj1.width >= obj2.X && obj1.X < obj2.X && obj1 != obj2);

            return false;
        }
        private bool FindUp(I3Dimensional obj1, I3Dimensional obj2)
        {
            if (Parent.collider != null)
                return (obj1.X < obj2.X + obj2.width && obj1.X + obj1.width > obj2.X && obj1.Y <= obj2.Y + obj2.width && obj1.Y > obj2.Y && obj1 != obj2);

            return false;
        }

        private bool FindDown(I3Dimensional obj1, I3Dimensional obj2)
        {
            if (Parent.collider != null)
                return (obj1.X < obj2.X + obj2.width && obj1.X + obj1.width > obj2.X && obj1.Y + obj1.width >= obj2.Y && obj1.Y < obj2.Y && obj1 != obj2);

            return false;

        }
    }
}
