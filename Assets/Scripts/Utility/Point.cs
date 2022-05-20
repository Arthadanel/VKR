using System;

namespace Utility
{
    public class Point:IComparable, IEquatable<Point>
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

        public float SqrDistance(Point other)
        {
            return (X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y);
        }

        public int CompareTo(object obj)
        {
            Point other = (Point) obj;
            return X.Equals(other.X) ? Y.CompareTo(other.Y) : X.CompareTo(other.X);
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

        public bool Equals(Point other)
        {
            return other != null && Math.Abs(X - other.X) < 0.0001 && Math.Abs(Y - other.Y) < 0.0001;
        }

        public override string ToString()
        {
            return "{X=" + X + ",Y=" + Y + "}";
        }
    }
}