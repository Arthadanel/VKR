using System.Collections.Generic;
using UnityEngine;

public class WorldMapVisualisation : MonoBehaviour
{
        public void DrawPolygon(List<Point> points)
        { 
                Vector3[] vertices3D = new Vector3[points.Count];
                Vector2[] vertices2D = new Vector2[points.Count];//uv
                int[] triangles = new int[points.Count-2];
                int Z = 0;

                SortPointsPolygon(points);
                
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

        private void SortPointsPolygon(List<Point> points)
        {
                // int[] triangles = new int[points.Count];
                // for (int i = 0; i < triangles.Length; i++)
                // {
                //         triangles[i] = i;
                // }
                //todo
                
                // int[] triangles = new int[2*3];
                // triangles[0] = 0;
                // triangles[1] = 1;
                // triangles[2] = 2;
                // triangles[3] = 2;
                // triangles[4] = 3;
                // triangles[5] = 0;
                
                
        }

        private int GetMaxY(List<Point> points)
        {
                int result = points[0].X;

                foreach (Point point in points)
                {
                        if (point.X > result)
                                result = point.X;
                }
                return result;
        } 
        
        private int GetMaxX(List<Point> points)
        {
                int result = points[0].Y;
                
                foreach (Point point in points)
                {
                        if (point.Y > result)
                                result = point.Y;
                }
                return result;
        }

        private void Start()
        {
                List<Point> points = new List<Point>();
                Random.InitState(GameSettings.SEED);
                // int pointCount = Random.Range(GameSettings.LEVEL_COUNT_MIN, GameSettings.LEVEL_COUNT_MAX + 1);
                // for (int i = 0; i < pointCount; i++)
                // {
                //         points.Add(new Point(Random.Range(0,5),Random.Range(0,5)));
                // }
                // points.Sort();
                // foreach (Point point in points)
                // {
                //         Debug.Log(point);
                // }
                points.Add(new Point(0,0));
                points.Add(new Point(0,1));
                points.Add(new Point(1,1));
                points.Add(new Point(1,0));
                DrawPolygon(points);
        }
}