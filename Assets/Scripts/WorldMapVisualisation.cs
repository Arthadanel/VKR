using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldMapVisualisation : MonoBehaviour
{
    public void DrawPolygon(List<Point> points)
    { 
        Vector2[] vertices2D = SortPointsPolygon(points);
        Debug.Log(vertices2D.Length);
        Vector3[] vertices3D = new Vector3[points.Count];
        int[] triangles = new int[points.Count-2];
        int Z = 0;

        
        for (int i = 0; i < points.Count; i++)
        {
            int x = points[i].X;
            int y = points[i].Y;
            int z = Z;
            vertices3D[i] = new Vector3(x,y,z);
            vertices2D[i] = new Vector2(x,y);
        }

        int t = 0;
        for (int i = 0; i < triangles.Length; i+=2)
        {
            triangles[i] = t;
            triangles[i+1] = t+1;
            triangles[i+2] = t+2;
            t+=2;
        }
        
        //Create Mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices3D;
        mesh.uv = vertices2D;
        mesh.triangles = triangles;
    }

    private Vector2[] SortPointsPolygon(List<Point> points)
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
            // if (points[p].X <= result[v].x && points[p].Y <= result[v].y)
            // {
            //     Debug.Log(points.Count);
            //     lastPoint = points[p];
            //     result[v] = new Vector2(lastPoint.X,lastPoint.Y);
            //     Debug.Log(lastPoint);
            //     v++;
            //     points.RemoveAt(p);
            //     p = 0;
            // }
            // else
            //     p++;
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
        points = points.Distinct().ToList();
        points.Sort();
        foreach (Point point in points)
        {
            Debug.Log(point);
        }
        // points.Add(new Point(0,0));
        // points.Add(new Point(0,1));
        // points.Add(new Point(1,1));
        // points.Add(new Point(1,0));
        
        DrawPolygon(points);
    }
}
