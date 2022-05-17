using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public class Triangle
    {
        public readonly List<Edge> Edges;
        public readonly List<Point> Points;

        public Triangle(Edge edge1, Edge edge2, Edge edge3)
        {
            Edges = new List<Edge>
            {
                edge1,
                edge2,
                edge3
            };
            foreach (var edge in Edges)
            {
                if(!edge.AdjacentTriangles.Contains(this))
                    edge.AdjacentTriangles.Add(this);
            }

            Points = GetPoints();
        }

        public Triangle(Point a, Point b, Point c)
        {
            Edge edge1 = new Edge(a,b);
            Edge edge2 = new Edge(b,c);
            Edge edge3 = new Edge(b,a);
            Edges = new List<Edge>
            {
                edge1,
                edge2,
                edge3
            };
            foreach (var edge in Edges)
            {
                if(!edge.AdjacentTriangles.Contains(this))
                    edge.AdjacentTriangles.Add(this);
            }
            Points = GetPoints();
        }

        public Point GetCircumcenter()
        {
            float x1 = Points[0].X;
            float y1 = Points[0].Y;
            float x2 = Points[1].X;
            float y2 = Points[1].Y;
            float x3 = Points[2].X;
            float y3 = Points[2].Y;
            float d = 2 * (x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            float xPart = (x1 * x1 + y1 * y1) * (y2 - y3) + (x2 * x2 + y2 * y2) * (y3 - y1) +
                           (x3 * x3 + y3 * y3) * (y1 - y2);
            float yPart = (x1 * x1 + y1 * y1) * (x3 - x2) + (x2 * x2 + y2 * y2) * (x1 - x3) +
                           (x3 * x3 + y3 * y3) * (x2 - x1);
            
            float x = xPart/d;
            float y = xPart/d;
            
            Point circumcenter = new Point(x,y);
            return circumcenter;
        }

        public bool CircumcircleContainsPoint(Point newPoint)
        {
            
            /*
             * a, b, c - three triangle points points
             * C - circumcircle of (a, b, c)
             * d - other point.
             * 
             *         | ax-dx, ay-dy, (ax-dx)² + (ay-dy)² |
             *   det = | bx-dx, by-dy, (bx-dx)² + (by-dy)² |
             *         | cx-dx, cy-dy, (cx-dx)² + (cy-dy)² |
             */

            float x1 = Points[0].X-newPoint.X;
            float y1 = Points[0].Y - newPoint.Y;
            float x2 = Points[1].X-newPoint.X;
            float y2 = Points[1].Y - newPoint.Y;
            float x3 = Points[2].X-newPoint.X;
            float y3 = Points[2].Y - newPoint.Y;
            
            //(ax_*ax_ + ay_*ay_) * (bx_*cy_-cx_*by_) - (bx_*bx_ + by_*by_) * (ax_*cy_-cx_*ay_) +
            //(cx_*cx_ + cy_*cy_) * (ax_*by_-bx_*ay_)
            
            float det = (x1 * x1 + y1 * y1) * (x2 * y3 - x3 * y2) - (x2 * x2 + y2 * y2) * (x1 * y3 - x3 * y1) +
                        (x3 * x3 + y3 * y3) * (x1 * y2 - x2 * y1);

            return det >= -0.0001; //inside circumcircle
        }

        private List<Point> GetPoints()
        {
            List<Point> result = new List<Point>();
            foreach (var edge in Edges)
            {
                result.Add(edge.P1);
                result.Add(edge.P2);
            }
            result.Sort();
            result = result.Distinct().ToList();
            //DEBUG
            //Debug.Log(result.Count+ " points in triangle");
            SortPointsCounterClockwise(result);
            return result;
        }
        
        private void SortPointsCounterClockwise (List<Point> points)
        {
            //check if already sorted
            bool check = (points[1].X - points[0].X) * (points[2].Y - points[0].Y) -
                (points[2].X - points[0].X) * (points[1].Y - points[0].Y) > 0;
            if (check) return;
            //change points' order if necessary
            (points[1], points[2]) = (points[2], points[1]);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Triangle other = obj as Triangle;
            if (other == null) return false;
            return Equals(other);
        }

        protected bool Equals(Triangle other)
        {
            // int similarities = 0;
            // foreach (var edge in Edges)
            // {
            //     if (other.Edges.Contains(edge))
            //         similarities++;
            // }
            //
            // Debug.Log("similarities: " + similarities);
            // return similarities == 3;
            return this.ToString() == other.ToString();
        }

        public override int GetHashCode()
        {
            return (Edges != null ? Edges.GetHashCode() : 0);
        }

        public override string ToString()
        {
            string result = "|";
            // foreach (var edge in Edges)
            // {
            //     result += edge.ToString() + '|';
            // }
            List<Point> points = GetPoints();
            foreach (var point in points)
            {
                result += point.ToString() + '|';
            }
            return result;
        }

        // public int CompareTo(object obj)
        // {
        //     if (obj == null) throw new NullReferenceException();
        //     Triangle other = obj as Triangle;
        //     if (other == null) throw new InvalidCastException();
        //     Edges.Sort();
        //     int result = 0;
        //     for (int i = 0; i < Edges.Count; i++)
        //     {
        //         result = Edges[i].CompareTo(Edges[i]);
        //         if ())
        //             return 1;
        //     }
        // }
    }
}