using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Edge:IComparable, IEquatable<Edge>
    {
        public readonly Point P1;
        public readonly Point P2;
        private List<Triangle> _adjacentTriangles = new List<Triangle>();
        private List<Polygon> _adjacentPolygons = new List<Polygon>();
        private Edge _edgeBetweenAdjTrianglesCenters;
        

        public Edge(Point p1, Point p2)
        {
            if (p1.CompareTo(p2) > 0)
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

        public Edge(Edge edge)
        {
            P1 = edge.P1;
            P2 = edge.P2;
        }

        public void AddAdjacentPolygon(Polygon polygon)
        {
            if(!_adjacentPolygons.Contains(polygon))
            {
                _adjacentPolygons.Add(polygon);
            }
        }
        public int GetAdjacentPolygonCount()
        {
            return _adjacentPolygons.Count;
        }

        public Polygon GetOtherAdjPolygon(Polygon polygon)
        {
            Polygon other = null;
            if (_adjacentPolygons.Count > 0 && _adjacentPolygons.Contains(polygon))
            {
                other = _adjacentPolygons[(_adjacentPolygons.IndexOf(polygon) + 1) % 2];
            }
            
            return other;
        }

        public void AddAdjacentTriangle(Triangle triangle)
        {
            if(!_adjacentTriangles.Contains(triangle))
            {
                _adjacentTriangles.Add(triangle);
            }
        }

        public int GetAdjacentTriangleCount()
        {
            return _adjacentTriangles.Count;
        }

        public Edge GetEdgeBetweenAdjTrianglesCenters()
        {
            _edgeBetweenAdjTrianglesCenters ??= new Edge(_adjacentTriangles[0].Circumcenter, _adjacentTriangles[1].Circumcenter);
            return _edgeBetweenAdjTrianglesCenters;
        }

        public bool BelongsToPolygon(Polygon polygon)
        {
            Point anchor = polygon.Anchor;
            if (P1.Equals(anchor)) return true;
            if (P2.Equals(anchor)) return true;

            return false;
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
            Edge other = obj as Edge;
            if (other == null) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((P1 != null ? P1.GetHashCode() : 0) * 397) ^ (P2 != null ? P2.GetHashCode() : 0);
            }
        }

        public bool Equals(Edge other)
        {
            return other != null && P1.Equals(other.P1) && P2.Equals(other.P2);
        }

        //for sorting the edges by first point x-coordinate
        public int CompareTo(object obj)
        {
            Edge other = (Edge) obj;
            return P1.Equals(other.P1) ? P2.CompareTo(other.P2) : P1.CompareTo(other.P1);
        }//NOT TESTED

        public override string ToString()
        {
            return P1 + "->" + P2;
        }
    }
}