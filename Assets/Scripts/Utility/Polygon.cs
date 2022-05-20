using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public class Polygon
    {
        public readonly List<Edge> Edges = new List<Edge>();
        public readonly List<Point> Vertices = new List<Point>();
        public readonly Point Anchor;

        public Polygon(Point anchor)
        {
            Anchor = anchor;
        }

        public void AddEdge(Edge edge)
        {
            // Debug.Log(edge);
            if (Edges.Contains(edge)) return;
            Edges.Add(edge);
            Vertices.Add(edge.P1);
            Vertices.Add(edge.P2);
            // Debug.Log(edge);
        }

        public List<Point> GetConvexHull()
        {
            List<Point> convexHull = new List<Point>();
            List<Edge> edges = new List<Edge>();
            foreach (var e in Edges)
            {
                edges.Add(e);
            }

            int c = 0;
            Point first = edges[0].P1;
            Point last = edges[0].P2;
            edges.RemoveAt(0);
            convexHull.Add(last);
            while (edges.Count>0)
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

                c++;
                if(c>10000)
                {
                    throw new Exception("Infinite cycle");
                    break;
                }            }

            return convexHull;
        }
    }
}