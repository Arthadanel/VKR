using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class MapSegment:MonoBehaviour
{
    private Polygon _polygon;

    public void SetPolygon(Polygon polygon)
    {
        _polygon = polygon;
    }

    private void OnMouseEnter()
    {
    }

    private void OnMouseDown()
    {
        SelectedLevelData.SelectLevel(_polygon);
        SceneManager.LoadScene("Level");
    }
}