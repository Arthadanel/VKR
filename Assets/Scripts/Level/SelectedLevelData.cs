using System.Collections.Generic;
using Utility;

namespace Level
{
    public static class SelectedLevelData
    {
        private static Polygon _polygon;

        public static void SelectLevel(Polygon polygon)
        {
            _polygon = polygon;
            LevelController.CURRENT_LEVEL = polygon.LevelNumber;
        }

        public static Polygon GetLevelPolygon()
        {
            return _polygon;
        }
    
        public static Edge GetEdge(Point a, Point b)
        {
            foreach (var edge in _polygon.Edges)
            {
                if (edge.Equals(new Edge(a, b)))
                    return edge;
            }
            return null;
        }

        public static void ClearSelection()
        {
            _polygon = null;
        }

        public static List<Point> GetConvexHull()
        {
            return _polygon.GetConvexHull();
        }
    }
}