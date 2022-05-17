using System;
using System.Collections.Generic;

namespace Utility
{
    public class Edge:IComparable
    {
        public readonly Point P1;
        public readonly Point P2;
        public List<Triangle> AdjacentTriangles = new List<Triangle>();
        

        public Edge(Point p1, Point p2)
        {
            if (p1.CompareTo(p2) >= 0)
            {
                P1 = p2;
                P2 = p1;
            }
            else
            {
                P1 = p1;
                P2 = p2;
            }
        }

        //check if point belongs to the edge
        public bool EdgeInterceptsPoint(Point point)
        {
            bool result = false;
            if (Math.Abs(P1.X - P2.X) < 0.001f)
            {
                result = (Math.Abs(point.X - P1.X) < 0.0001f) &&
                         ((point.Y <= P1.Y && point.Y >= P2.Y) || (point.Y <= P2.Y && point.Y >= P1.Y));
            }
            else if (Math.Abs(P1.Y - P2.Y) < 0.0001f)
            {
                result = (Math.Abs(point.Y - P1.Y) < 0.0001f) &&
                         ((point.X <= P1.X && point.X >= P2.X) || (point.X <= P2.X && point.X >= P1.X));
            }
            else
            {
                float ratio1 = 1f * (point.X - P1.X) / (P2.X - P1.X);
                float ratio2 = 1f * (point.Y - P1.Y) / (P2.Y - P1.Y);
                if (Math.Abs(ratio1 - ratio2) > 0.0001f) return false;
                result = ratio1 <= 1 && ratio1 >= 0;
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Triangle other = obj as Triangle;
            if (other == null) return false;
            return Equals(other);
        }

        protected bool Equals(Edge other)
        {
            return Equals(P1, other.P1) && Equals(P2, other.P2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((P1 != null ? P1.GetHashCode() : 0) * 397) ^ (P2 != null ? P2.GetHashCode() : 0);
            }
        }

        //for sorting the edges by first point x-coordinate
        public int CompareTo(object obj)
        {
            Edge other = (Edge) obj;
            return P1.CompareTo(other.P1) == 0 ? P1.CompareTo(other.P1) : P2.CompareTo(other.P2);
        }//NOT TESTED

        public override string ToString()
        {
            return P1.ToString() + "->" + P2.ToString();
        }
    }
}