using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public class Triangle : IEquatable<Triangle>
    {
        public readonly List<Edge> Edges;
        public readonly List<Point> Points;
        public readonly Point Circumcenter;
        public readonly float SqrRadius;

        public Triangle(Edge edge1, Edge edge2, Edge edge3)
        {
            Edges = new List<Edge>
            {
                edge1,
                edge2,
                edge3
            };
            Edges.Sort();
            Points = GetPoints();
            Circumcenter = GetCircumcenter();
            SqrRadius = Circumcenter.SqrDistance(Points[0]);
        }

        public Triangle(Point a, Point b, Point c)
        {
            Edge edge1 = new Edge(a,b);
            Edge edge2 = new Edge(b,c);
            Edge edge3 = new Edge(c,a);
            Edges = new List<Edge>
            {
                edge1,
                edge2,
                edge3
            };
            Edges.Sort();
            Points = GetPoints();
            //Circumcenter = GetCircumcenter();
            //SqrRadius = Circumcenter.SqrDistance(Points[0]);
        }

        private Point GetCircumcenter()
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
        
        private float sign (Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        public bool PointInsideTriangle (Point point)
        {
            var d1 = sign(point, Points[0], Points[1]);
            var d2 = sign(point, Points[1], Points[2]);
            var d3 = sign(point, Points[2], Points[0]);

            var hasNegative = (d1 < 0) || (d2 < 0) || (d3 < 0);
            var hasPositive = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(hasNegative && hasPositive);
        }

        public Edge ClosestEdge(Point point)
        {
            Edge closest = Edges[0];
            foreach (var edge in Edges)
            {
                if (edge.SumSqrDistanceToVertices(point) < closest.SumSqrDistanceToVertices(point))
                    closest = edge;
            }

            return closest;
        }

        public bool PointWithinCircumcircle(Point point)
        {
            return CircumcircleContainsPoint(point); //Circumcenter.SqrDistance(point) <= SqrRadius;
        }

        private bool CircumcircleContainsPoint(Point newPoint)
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

            // if (ToString() == "|{X=0,Y=25}|{X=10,Y=4}|{X=7,Y=11}|" ||
            //     ToString() == "|{X=7,Y=11}|{X=9,Y=11}|{X=25,Y=25}|")
            // {
            //     Debug.Log("Triangle DET: " + det);
            //     Debug.Log(this);
            // }

            return det > 0; //inside circumcircle
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
            if (obj is null) return false;
            Triangle other = obj as Triangle;
            if (other is null) return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return (Points != null ? Points.GetHashCode() : 0);
        }

        public bool Equals(Triangle other)
        {
            foreach (var point in Points)
            {
                if (other != null && other.Points.Contains(point))
                    continue;
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string result = "|";
            // foreach (var edge in Edges)
            // {
            //     result += edge.ToString() + '|';
            // }
            foreach (var point in Points)
            {
                result += point.ToString() + '|';
            }
            return result;
        }
    }
}