using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Polygon
    {
        public readonly List<Edge> Edges;
        private List<Point> _convexHull;
        public readonly Point Anchor;

        public Polygon(Point anchor)
        {
            Anchor = anchor;
            Edges = new List<Edge>();
        }

        public void AddEdge(Edge edge)
        {
            if (Edges.Contains(edge)) return;
            Edges.Add(edge);
        }

        public List<Point> GetConvexHull(bool recalculate = false)
        {
            if (!recalculate && _convexHull != null)
            {
                return _convexHull;
            }
            
            List<Point> convexHull = new List<Point>();
            List<Edge> edges = new List<Edge>();
            foreach (var e in Edges)
            {
                edges.Add(e);
            }
            
            Point first = edges[0].P1;
            Point last = edges[0].P2;
            edges.RemoveAt(0);
            convexHull.Add(last);
            while (edges.Count>1)
            {
                for (var i = 0; i < edges.Count; i++)
                {
                    var edge = edges[i];
                    if (edge.P1.Equals(last))
                    {
                        last = edge.P2;
                        convexHull.Add(last);
                        edges.RemoveAt(i);
                    }
                    else if (edge.P2.Equals(last))
                    {
                        last = edge.P1;
                        convexHull.Add(last);
                        edges.RemoveAt(i);
                    }
                }         
            }
            
            convexHull.Add(first);
            edges.RemoveAt(0);

            _convexHull = convexHull;

            return convexHull;
        }

        public override string ToString()
        {
            return Anchor.ToString() + "|" + Edges.Count;
        }
    }
}