using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public class MapSegment : MonoBehaviour
{
    private Polygon _polygon;
    private bool _levelReached;

    public void SetPolygon(Polygon polygon)
    {
        _polygon = polygon;
    }

    private void Start()
    {
        _levelReached = SaveData.OPEN_LEVELS.Contains(_polygon.LevelNumber);
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (!_levelReached)
        {
            WorldMapVisualisation.SetMeshColor(Color.gray, mesh);
        }
    }

    private void OnMouseDown()
    {
        if (!_levelReached) return;
        SelectedLevelData.SelectLevel(_polygon);
        SceneManager.LoadScene("Level");
    }
}