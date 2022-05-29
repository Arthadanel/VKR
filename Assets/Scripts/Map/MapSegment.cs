using Data;
using Level;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utility;

namespace Map
{
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
            _levelReached = SaveDataStorage.OPEN_LEVELS.Contains(_polygon.LevelNumber);
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            TextMeshPro textMesh = GetComponentInChildren<TextMeshPro>();
            textMesh.transform.position = textMesh.transform.TransformPoint(mesh.bounds.center);
            
            if (!_levelReached)
            {
                WorldMapVisualisation.SetMeshColor(Color.gray, mesh);
            }
            else
            {
                textMesh.text = "Times Completed: " + SaveDataStorage.LEVEL_COMPLETION[_polygon.LevelNumber];
            }
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!_levelReached) return;
            SelectedLevelData.SelectLevel(_polygon);
            SceneManager.LoadScene("Level");
        }
    }
}