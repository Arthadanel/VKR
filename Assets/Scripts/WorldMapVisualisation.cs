using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

public class WorldMapVisualisation : MonoBehaviour
{
    public void DrawPolygon(List<Point> points)
    { 
        Vector2[] vertices2D = SortPointsClockwise(points);
        int verticesCount = vertices2D.Length;
        int[] triangles = GetTriangles(verticesCount);
        
        Vector3[] vertices3D = new Vector3[verticesCount];
        int Z = 0;

        for (int i = 0; i < verticesCount; i++)
        {
            vertices3D[i] = new Vector3(vertices2D[i].x,vertices2D[i].y, Z);
        }
        
        //Create Mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices3D;
        mesh.uv = vertices2D;
        mesh.triangles = triangles;
        
        AddCollider(vertices2D);
    }

    private void AddCollider(Vector2[] vertices2D)
    {
        PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        polygonCollider.points = vertices2D;
    }

    private int[] GetTriangles(int verticesCount)
    {
        int[] triangles = new int[(verticesCount - 2) * 3];
        Debug.Log(verticesCount);
        int t = 0;
        int i = 0;
        while (t+2<verticesCount)
        {
            Debug.Log(t);
            triangles[i] = t;
            triangles[i+1] = t+1;
            triangles[i+2] = t+2;
            t+=2;
            Debug.Log(triangles[i] + " " + triangles[i + 1] + " " + triangles[i + 2]);
            i += 3;
        }

        int j = 3;
        while (i < triangles.Length)
        {
            triangles[i] = 0;
            triangles[i+1] = triangles[j];
            j+=3;
            triangles[i+2] = j==i? verticesCount-1: triangles[j];
            Debug.Log(triangles[i] + " " + triangles[i + 1] + " " + triangles[i + 2]);
            i += 3;
        }

        return triangles;
    }

    private Vector2[] SortPointsClockwise(List<Point> points)
    {
        Debug.Log("===================");
        points = points.Distinct().ToList();
        points.Sort();
        int size = points.Count;
        Vector2[] result = new Vector2[size];
        Point maxYPoint = GetMaxYPoint(points);
        Point maxXPoint = GetMaxXPoint(points);
        Debug.Log(maxYPoint);
        Debug.Log(maxXPoint);
        Debug.Log("===================");
        int v = 0;
        int p = 0;
        Point lastPoint=points[p];
        
        result[v] = new Vector2(lastPoint.X,lastPoint.Y); //min x & min y
        points.RemoveAt(p);

        Debug.Log(lastPoint);
        
        v++;
        
        while (!lastPoint.Equals(maxYPoint))
        {
            if (points[p].X >= lastPoint.X && points[p].Y >= lastPoint.Y)
            {
                lastPoint = points[p];
                result[v] = new Vector2(lastPoint.X, lastPoint.Y);
                Debug.Log(lastPoint);
                v++;
                points.RemoveAt(p);
                p = 0;
            }
            else
                p++;
        }
        
        p = 0;
        while (!lastPoint.Equals(maxXPoint))
        {
            if (points[p].X >= lastPoint.X && points[p].Y <= lastPoint.Y)
            {
                lastPoint = points[p];
                result[v] = new Vector2(lastPoint.X,lastPoint.Y);
                Debug.Log(lastPoint);
                v++;
                points.RemoveAt(p);
                p = 0;
            }
            else
                p++;
        }
        
        p = 0;
        points.Reverse();
        while (points.Count>0)
        {
            lastPoint = points[p];
            result[v] = new Vector2(lastPoint.X,lastPoint.Y);
            Debug.Log(lastPoint);
            v++;
            points.RemoveAt(p);
        }

        return result;
    }

    private Point GetMaxYPoint(List<Point> points)
    {
        Point result = points[0];

        foreach (Point point in points)
        {
            if (point.Y >= result.Y)
                result = point;
        }
        return result;
    } 
    
    private Point GetMaxXPoint(List<Point> points)
    {
        Point result = points[0];
        
        foreach (Point point in points)
        {
            if (point.X > result.X)
                result = point;
        }
        return result;
    }

    private void Start()
    {
        List<Point> points = new List<Point>();
        Random.InitState(GameSettings.SEED);
        int pointCount = Random.Range(GameSettings.LEVEL_COUNT_MIN, GameSettings.LEVEL_COUNT_MAX + 1);
        for (int i = 0; i < pointCount; i++)
        {
            points.Add(new Point(Random.Range(0,5),Random.Range(0,5)));
        }
        
        DrawPolygon(points);
    }
}
