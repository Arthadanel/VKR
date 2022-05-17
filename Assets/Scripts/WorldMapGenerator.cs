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
        int xMax = Int32.MinValue;
        int yMax = Int32.MinValue;
        int xMin = Int32.MaxValue;
        int yMin = Int32.MaxValue;
        for (int i = 0; i < pointCount; i++)
        {
            int x = Random.Range(0, 10);
            int y = Random.Range(0, 10);
            Point point = new Point(x, y);
            if (points.Contains(point))
            {
                i--;
                continue;
            }

            if (x > xMax) xMax = x;
            if (y > yMax) yMax = y;
            if (x < xMin) xMin = x;
            if (y < yMin) yMin = y;
            
            points.Add(point);
        }
        points = points.Distinct().ToList();
        
        //DEBUG
        foreach (var p in points)
        {
            Debug.Log(p);
        }
        
        //{ (0,0), (0, 2 * maxY), (2 * maxX, 0) }
        //create outer triangle and add it to the triangulation
        List<Triangle> triangles = new List<Triangle>();
        Triangle outline = new Triangle(new Point(2 * xMin, 2 * yMin), new Point(2 * xMin, 2 * yMax),
            new Point(2 * xMax, 2 * yMin));
        Triangle tmp = new Triangle(
            new Point(0, 0),
            new Point(0, GameSettings.LIMIT_VALUE),
            new Point(GameSettings.LIMIT_VALUE, GameSettings.LIMIT_VALUE));
        triangles.Add(tmp);
        
        foreach (var p in points)
        {
            DelaunayTriangulation(triangles, p);
        }

        Debug.Log(triangles.Count);
        foreach (var triangle in triangles)
        {
            Debug.Log(triangle);
        }

        triangles = triangles.Distinct().ToList();
        Debug.Log(triangles.Count);
        VoronoiDiagramConversion(triangles);

    }

    public void VoronoiDiagramConversion(List<Triangle> triangles)
    {
        List<Edge> edges = new List<Edge>();
        foreach (var t in triangles)
        {
            foreach (var edge in t.Edges)
            {
                //edge.AdjacentTriangles.Add(triangles);
            }
        }
        //todo
    }

    private void DelaunayTriangulation(List<Triangle> triangles, Point newPoint)
    {
        List<Triangle> badTriangles = new List<Triangle>();
        List<Edge> polygonHole = new List<Edge>();
        FindInvalidatedTriangles(triangles, newPoint, badTriangles, polygonHole);
        RemoveDuplicateEdgesFromPolygonHole(polygonHole);
        RemoveBadTrianglesFromTriangulation(triangles, badTriangles);
        FillInPolygonHole(triangles, newPoint, polygonHole);
        Debug.Log("======================");
        Debug.Log(newPoint);
        foreach (var triangle in triangles)
        {
            Debug.Log(triangle);
        }
    }

    private void FindInvalidatedTriangles(List<Triangle> triangles, Point newPoint, List<Triangle> badTriangles, List<Edge> polygonHole)
    {
        foreach (var triangle in triangles)
        {
            if (triangle.CircumcircleContainsPoint(newPoint))
            {
                badTriangles.Add(triangle);
                foreach (var edge in triangle.Edges)
                {
                    polygonHole.Add(edge);
                }
            }
        }
    }

    private void RemoveDuplicateEdgesFromPolygonHole(List<Edge> polygonHole)
    {
        polygonHole.Sort();
        for (int i = 1; i < polygonHole.Count; i++)
        {
            if(polygonHole[i-1]==polygonHole[i])
                polygonHole.RemoveAt(--i);
        }//NOT TESTED
    }

    private void RemoveBadTrianglesFromTriangulation(List<Triangle> triangles, List<Triangle> badTriangles)
    {
        foreach (var triangle in badTriangles)
        {
            triangles.Remove(triangle);
        }//NOT TESTED
    }

    private void FillInPolygonHole(List<Triangle> triangles, Point newPoint, List<Edge> polygonHole)
    {
        foreach (var edge in polygonHole)
        {
            Edge tmp1 = new Edge(edge.P1, newPoint);
            Edge tmp2 = new Edge(edge.P2, newPoint);
            Triangle newTriangle = new Triangle(edge,tmp1,tmp2);
            triangles.Add(newTriangle);
        }//NOT TESTED
    }

    private void LloydRelaxation()
    {
        
    }
}
