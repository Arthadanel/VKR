using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

public class WorldMapVisualisation
{
    public static void DrawPolygon(List<Point> points, GameObject anchor)
    { 
        Vector2[] vertices2D = GetConvexHull(points);
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
        anchor.GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices3D;
        mesh.uv = vertices2D;
        mesh.triangles = triangles;
        SetMeshColor(Random.ColorHSV(),mesh);
        //Set up collider
        PolygonCollider2D polygonCollider = anchor.GetComponent<PolygonCollider2D>();
        polygonCollider.points = vertices2D;
    }

    private static void SetMeshColor(Color color, Mesh mesh)
    {
        Color32[] colors = new Color32[mesh.vertexCount];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        mesh.SetColors(colors);
    }

    private static int[] GetTriangles(int verticesCount)
    {
        int[] triangles = new int[(verticesCount - 2) * 3];
        int t = 0;
        int i = 0;
        while (t+2<verticesCount)
        {
            triangles[i] = t;
            triangles[i+1] = t+1;
            triangles[i+2] = t+2;
            t+=2;
            i += 3;
        }

        int j = 3;
        while (i < triangles.Length)
        {
            triangles[i] = 0;
            triangles[i+1] = triangles[j];
            j+=3;
            triangles[i+2] = j==i? verticesCount-1: triangles[j];
            i += 3;
        }

        return triangles;
    }

    private static Vector2[] GetConvexHull(List<Point> points)
    {
        int vertexCount = points.Count;
        Vector2[] result = new Vector2[vertexCount];
        if (points.Count == 3)
        {
            for (int i = 0; i < vertexCount; i++)
            {
                result[i] = new Vector2(points[i].X, points[i].Y);
            }
            return result;
        }
        
        //choked at least once, probably needs revision
        
        points.Sort();
        Point maxYPoint = GetMaxYPoint(points);
        Point maxXPoint = GetMaxXPoint(points);
        
        int v = 0;
        int p = 0;
        Point lastPoint=points[p];
        result[v] = new Vector2(lastPoint.X,lastPoint.Y); //min x & min y
        points.RemoveAt(p);
        
        v++;
        
        while (!lastPoint.Equals(maxYPoint))
        {
            if (points[p].X >= lastPoint.X && points[p].Y >= lastPoint.Y)
            {
                lastPoint = points[p];
                result[v] = new Vector2(lastPoint.X, lastPoint.Y);
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
            v++;
            points.RemoveAt(p);
        }

        return result;
    }

    private static Point GetMaxYPoint(List<Point> points)
    {
        Point result = points[0];

        foreach (Point point in points)
        {
            if (point.Y >= result.Y)
                result = point;
        }
        return result;
    } 
    
    private static Point GetMaxXPoint(List<Point> points)
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
        
        //DrawPolygon(points);
    }
}
