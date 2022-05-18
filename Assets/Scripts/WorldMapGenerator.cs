using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class WorldMapGenerator : MonoBehaviour
{
    [SerializeField] private int PointCount = 1;
    private void Awake()
    {
        Random.InitState(GameSettings.SEED);
    }

    private void Start()
    {
        //Generate anchor points
        List<Point> points = new List<Point>();
        int pointCount = PointCount; //Random.Range(GameSettings.LEVEL_COUNT_MIN, GameSettings.LEVEL_COUNT_MAX + 1);
        int maxX = 0;
        int maxY = 0;
        for (int i = 0; i < pointCount; i++)
        {
            int x = Random.Range(0, GameSettings.MAX_XY);
            int y = Random.Range(0, GameSettings.MAX_XY);
            if (x > maxX) maxX = x;
            if (y > maxY) maxY = y;
            Point point = new Point(x, y);
            if (points.Contains(point))
            {
                i--;
                continue;
            }
            
            points.Add(point);
        }
        points = points.Distinct().ToList();
        
        //DEBUG
        foreach (var p in points)
        {
            Debug.Log(p);
        }
        
        // Triangle outline = new Triangle(
        //     new Point(0, 0),
        //     new Point(0, 2 * maxY),
        //     new Point(2 * maxX, 0));

        Triangle outline = new Triangle(
            new Point(0, 0),
            new Point(0, GameSettings.LIMIT_VALUE),
            new Point(GameSettings.LIMIT_VALUE * 2, GameSettings.LIMIT_VALUE));
        
        List<Triangle> triangles = new List<Triangle>();
        triangles.Add(outline);
        
        foreach (var p in points)
        {
            DelaunayTriangulation(triangles, p);
        }

        Debug.Log(triangles.Count);
        foreach (var triangle in triangles)
        {
            Debug.Log(triangle);
        }

        //VoronoiDiagramConversion(triangles);

    }

    public List<Edge> VoronoiDiagramConversion(List<Triangle> triangles)
    {
        List<Edge> edges = new List<Edge>();
        List<Edge> voronoiEdges = new List<Edge>();
        foreach (var triangle in triangles)
        {
            foreach (var edge in triangle.Edges)
            {
                edge.AddAdjacentTriangle(triangle);
                edges.Add(edge);
            }
        }

        foreach (var edge in edges)
        {
            //edge.AdjacentTriangles = edge.AdjacentTriangles.Distinct().ToList();
            Debug.Log("GetAdjacentTriangleCount: "+edge.GetAdjacentTriangleCount());
            if(edge.GetAdjacentTriangleCount()>2)
            {
                Debug.Log("Edge: " + edge);
                List<Triangle> tmp = edge.GetAdjacentTriangles();
                foreach (var triangle in tmp)
                {
                    Debug.Log(triangle);
                }
            }
            if(edge.GetAdjacentTriangleCount()<2)continue;
            List<Triangle> adjTriangles = edge.GetAdjacentTriangles();
            Point cc1 = adjTriangles[0].Circumcenter;
            Point cc2 = adjTriangles[1].Circumcenter;
            Edge newEdge = new Edge(cc1, cc2);
            voronoiEdges.Add(newEdge);
        }

        return voronoiEdges;
    }

    private void DelaunayTriangulation(List<Triangle> triangles, Point newPoint)
    {
        List<Triangle> badTriangles = new List<Triangle>();
        List<Edge> polygonHole = new List<Edge>();
        FindInvalidatedTriangles(ref triangles, newPoint, ref badTriangles, ref polygonHole);
        RemoveDuplicateEdgesFromPolygonHole(ref polygonHole);
        RemoveBadTrianglesFromTriangulation(ref triangles,ref badTriangles);
        FillInPolygonHole(ref triangles, newPoint, ref polygonHole);
    }

    private void FindInvalidatedTriangles(ref List<Triangle> triangles, Point newPoint, ref List<Triangle> badTriangles,
        ref List<Edge> polygonHole)
    {
        foreach (var triangle in triangles)
        {
            if (!triangle.PointWithinCircumcircle(newPoint))
                continue;

            // if (!triangle.PointInsideTriangle(newPoint))
            // {
            //     polygonHole.Add(triangle.ClosestEdge(newPoint));
            //     continue;
            // }
            
            badTriangles.Add(triangle);
            foreach (var edge in triangle.Edges)
            {
                polygonHole.Add(edge);
                // if (polygonHole.Contains(edge))
                // {
                //     polygonHole.Remove(edge);
                // }
                // else
                // {
                //     polygonHole.Add(edge);
                // }
            }
        }
    }

    private void RemoveDuplicateEdgesFromPolygonHole(ref List<Edge> polygonHole)
    {
        polygonHole.Sort();
        
        for (int i = 1; i < polygonHole.Count; i++)
        {
            if (polygonHole[i].Equals(polygonHole[i-1]))
            {
                Edge innerEdge = polygonHole[i];
                while (polygonHole.Contains(innerEdge))
                {
                    polygonHole.Remove(innerEdge);
                }
            }
        }
    }

    private void RemoveBadTrianglesFromTriangulation(ref List<Triangle> triangles, ref List<Triangle> badTriangles)
    {
        foreach (var badTriangle in badTriangles)
        {
            foreach (var edge in badTriangle.Edges)
            {
                edge.RemoveAdjacentTriangle(badTriangle);
            }
            triangles.Remove(badTriangle);
        }
    }

    private void FillInPolygonHole(ref List<Triangle> triangles, Point newPoint, ref List<Edge> polygonHole)
    {
        foreach (var edge in polygonHole)
        {
            Edge tmp1 = new Edge(edge.P1, newPoint);
            Edge tmp2 = new Edge(edge.P2, newPoint);
            Triangle newTriangle = new Triangle(edge,tmp1,tmp2);
            if (!triangles.Contains(newTriangle))
                triangles.Add(newTriangle);
        }
    }

    private void LloydRelaxation()
    {
        
    }
}
