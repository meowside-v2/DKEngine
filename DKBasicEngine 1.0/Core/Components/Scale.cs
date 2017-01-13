using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct Scale
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public float X;
        public float Y;
        public float Z;
        
        public Scale(float X, float Y, float Z)
        {
            this.X = X <= 0 ? 0.001f : X;
            this.Y = Y <= 0 ? 0.001f : Y;
            this.Z = Z <= 0 ? 0.001f : Z;
        }

        public static Scale operator -(Scale left, Scale right)
        {
            return new Scale(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Scale operator +(Scale left, Scale right)
        {
            return new Scale(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Scale operator *(Scale left, Scale right)
        {
            return new Scale(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }

        public static Scale operator *(Scale left, int right)
        {
            return new Scale(left.X * right, left.Y * right, left.Z * right);
        }

        public static Scale operator /(Scale left, Scale right)
        {
            return new Scale(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }

        public static bool operator ==(Scale left, Scale right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Scale left, Scale right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }
    }
}
