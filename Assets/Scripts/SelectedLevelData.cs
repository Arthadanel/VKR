using System.Collections.Generic;
using Utility;

public static class SelectedLevelData
{
    private static Polygon _polygon;

    public static void SelectLevel(Polygon polygon)
    {
        _polygon = polygon;
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