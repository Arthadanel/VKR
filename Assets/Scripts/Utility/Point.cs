using System;

namespace Utility
{
    public class Point:IComparable
    {
        public readonly float X;
        public readonly float Y;
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int CompareTo(object obj)
        {
            Point other = (Point) obj;
            int result = 1;
        
            if (other.X > this.X) result = -1;
            else if (other.X > this.X) result = -1;
        
            if (Equals(this, other)) result = 0;
        
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Point other = obj as Point;
            if (other == null) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        protected bool Equals(Point other)
        {
            return Math.Abs(X - other.X) < 0.0001 && Math.Abs(Y - other.Y) < 0.0001;
        }

        public override string ToString()
        {
            return "{X=" + X + ",Y=" + Y + "}";
        }
    }
}