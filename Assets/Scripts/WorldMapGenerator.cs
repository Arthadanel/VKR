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
        for (int i = 0; i < pointCount; i++)
        {
            int x = Random.Range(0, GameSettings.MAX_XY);
            int y = Random.Range(0, GameSettings.MAX_XY);
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
        
        //{ (0,0), (0, 2 * maxY), (2 * maxX, 0) }
        //create outer triangle and add it to the triangulation
        List<Triangle> triangles = new List<Triangle>();
        // Triangle outline = new Triangle(new Point(2 * xMin, 2 * yMin), new Point(2 * xMin, 2 * yMax),
        //     new Point(2 * xMax, 2 * yMin));
        Triangle outline = new Triangle(
            new Point(0, 0),
            new Point(0, GameSettings.LIMIT_VALUE),
            new Point(GameSettings.LIMIT_VALUE, GameSettings.LIMIT_VALUE));
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
        FindInvalidatedTriangles(ref triangles, newPoint, ref badTriangles, ref polygonHole);
        Debug.Log("======================");
        Debug.Log("_T: "+triangles.Count);
        Debug.Log("BT: "+badTriangles.Count);
        Debug.Log("_P: "+polygonHole.Count);
        RemoveBadTrianglesFromTriangulation(ref triangles,ref badTriangles);
        Debug.Log("GT: "+triangles.Count);
        FillInPolygonHole(ref triangles, newPoint, ref polygonHole);
        Debug.Log("_T: "+triangles.Count);
        Debug.Log("======================");
    }

    private void FindInvalidatedTriangles(ref List<Triangle> triangles, Point newPoint, ref List<Triangle> badTriangles,
        ref List<Edge> polygonHole)
    {
        foreach (var triangle in triangles)
        {
            if (!triangle.CircumcircleContainsPoint(newPoint))
                continue;

            if(triangle.PointInsideTriangle(newPoint))
            {
                badTriangles.Add(triangle);
                foreach (var edge in triangle.Edges)
                {
                    if (!polygonHole.Contains(edge))
                        polygonHole.Add(edge);
                }
                continue;
            }
            
            polygonHole.Add(triangle.ClosestEdge(newPoint));
        }
    }

    private void RemoveBadTrianglesFromTriangulation(ref List<Triangle> triangles, ref List<Triangle> badTriangles)
    {
        foreach (var badTriangle in badTriangles)
        {
            triangles.Remove(badTriangle);
        }//NOT TESTED
    }

    private void FillInPolygonHole(ref List<Triangle> triangles, Point newPoint, ref List<Edge> polygonHole)
    {
        foreach (var edge in polygonHole)
        {
            Debug.Log(edge);
            Edge tmp1 = new Edge(edge.P1, newPoint);
            Edge tmp2 = new Edge(edge.P2, newPoint);
            Triangle newTriangle = new Triangle(edge,tmp1,tmp2);
            if (!triangles.Contains(newTriangle))
                triangles.Add(newTriangle);
        }//NOT TESTED
    }

    private void LloydRelaxation()
    {
        
    }
}
