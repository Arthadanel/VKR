using System;
using System.Diagnostics;
using UnityEditor.U2D.Path;

public class Point:IComparable
{
    public int X;
    public int Y;

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
        if (obj is Point other)
        {
            return Equals(other);
        }

        return false;
    }

    protected bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X * 397) ^ Y;
        }
    }

    public override string ToString()
    {
        return "{X=" + X + ",Y=" + Y + "}";
    }
}