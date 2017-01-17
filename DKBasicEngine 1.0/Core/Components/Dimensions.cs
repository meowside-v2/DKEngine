/*
* (C) 2017 David Knieradl 
*/

namespace DKBasicEngine_1_0
{
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public struct Dimensions
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        public float Width;
        public float Height;
        public float Depth;
        
        public Dimensions(float Width, float Height, float Depth)
        {
            this.Width  = Width  < 0 ? 0 : Width;
            this.Height = Height < 0 ? 0 : Height;
            this.Depth  = Depth  < 0 ? 0 : Depth;
        }

        public static Dimensions operator -(Dimensions left, Dimensions right)
        {
            return new Dimensions(left.Width - right.Width, left.Height - right.Height, left.Depth - right.Depth);
        }

        public static Dimensions operator +(Dimensions left, Dimensions right)
        {
            return new Dimensions(left.Width + right.Width, left.Height + right.Height, left.Depth + right.Depth);
        }

        public static Dimensions operator *(Dimensions left, Dimensions right)
        {
            return new Dimensions(left.Width * right.Width, left.Height * right.Height, left.Depth * right.Depth);
        }

        public static Dimensions operator /(Dimensions left, Dimensions right)
        {
            return new Dimensions(left.Width / right.Width, left.Height / right.Height, left.Depth / right.Depth);
        }

        public static bool operator ==(Dimensions left, Dimensions right)
        {
            return left.Width == right.Width && left.Height == right.Height && left.Depth == right.Depth;
        }

        public static bool operator !=(Dimensions left, Dimensions right)
        {
            return left.Width != right.Width || left.Height != right.Height || left.Depth != right.Depth;
        }
    }
}
