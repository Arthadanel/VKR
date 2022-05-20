using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class WorldMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject mapAreaTemplate;
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
            int x = Random.Range(GameSettings.PADDING, GameSettings.LIMIT_VALUE - GameSettings.PADDING);
            int y = Random.Range(GameSettings.PADDING, GameSettings.LIMIT_VALUE - GameSettings.PADDING);
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

        Triangle outline = new Triangle(
            new Point(0, 0),
            new Point(0, GameSettings.LIMIT_VALUE),
            new Point(GameSettings.LIMIT_VALUE, GameSettings.LIMIT_VALUE));

        Triangle outline2 = new Triangle(
            new Point(0, 0),
            new Point(GameSettings.LIMIT_VALUE, 0),
            new Point(GameSettings.LIMIT_VALUE, GameSettings.LIMIT_VALUE));

        List<Triangle> triangles = new List<Triangle> {outline, outline2};

        foreach (var p in points)
        {
            DelaunayTriangulation(ref triangles, p);
        }

        foreach (var triangle in triangles)
        {
            DrawTriangle(triangle);
        }

        List<Polygon> voronoiPolygons = VoronoiDiagramConversion(triangles, points);
        foreach (var polygon in voronoiPolygons)
        {
            var triangleMesh = Instantiate(mapAreaTemplate, transform);
            WorldMapVisualisation.DrawPolygon(polygon, triangleMesh);
        }
    }

    private void DrawTriangle(Triangle triangle)
    {
        GameObject triangleMesh = Instantiate(mapAreaTemplate, transform);
        WorldMapVisualisation.DrawPolygon(triangle.Points, triangleMesh);
    }

    public List<Polygon> VoronoiDiagramConversion(List<Triangle> triangles, List<Point> points)
    {
        List<Edge> edges = new List<Edge>();
        List<Polygon> polygons = new List<Polygon>();
        
        foreach (var triangle in triangles)
        {
            foreach (var edge in triangle.Edges)
            {
                if(!edges.Contains(edge))
                {
                    edges.Add(edge);
                    edge.AddAdjacentTriangle(triangle);
                }
                else
                {
                    edges[edges.IndexOf(edge)].AddAdjacentTriangle(triangle);
                }
            }
        }

        for (var i = 0; i < points.Count; i++)
        {
            var point = points[i];
            Polygon polygon = new Polygon(point);
            polygons.Add(polygon);
            foreach (var edge in edges)
            {
                if (edge.GetAdjacentTriangleCount() < 2) continue;
                if (edge.BelongsToPolygon(polygon))
                {
                    polygon.AddEdge(edge.GetEdgeBetweenAdjTrianglesCenters());
                }
            }
        }

        return polygons;
    }

    private void DelaunayTriangulation(ref List<Triangle> triangles, Point newPoint)
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
        foreach (var t in triangles)
        {
            if (!t.PointWithinCircumcircle(newPoint))
                continue;
            
            badTriangles.Add(t);
            foreach (var edge in t.Edges)
            {
                polygonHole.Add(new Edge(edge));
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
                i--;
            }
        }
    }

    private void RemoveBadTrianglesFromTriangulation(ref List<Triangle> triangles, ref List<Triangle> badTriangles)
    {
        foreach (var badTriangle in badTriangles)
        {
            while (triangles.Contains(badTriangle))
            {
                bool tmp = triangles.Remove(badTriangle);
            }
        }
    }

    private void FillInPolygonHole(ref List<Triangle> triangles, Point newPoint, ref List<Edge> polygonHole)
    {
        foreach (var edge in polygonHole)
        {
            Triangle newTriangle = new Triangle(edge.P1, edge.P2, newPoint);
            triangles.Add(newTriangle);
        }
    }

    private void LloydRelaxation()
    {
        //todo
    }
}
