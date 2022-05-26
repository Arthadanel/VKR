using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GUIController: MonoBehaviour
    {
        [SerializeField] private ActionPanel actionPanel;
        [SerializeField] private GameObject turnPanel;
        [SerializeField] private Text actionPointsCounter;
        
        [SerializeField] private GameObject movementHighlighterAsset;
        [SerializeField] private GameObject attackHighlighterAsset;
        [SerializeField] private GameObject buffHighlighterAsset;

        private static Dictionary<ActionType, GameObject> _highlights;

        private static List<TileNode> _highlightedTiles;

        private void Start()
        {
            _highlights= new Dictionary<ActionType, GameObject>
            {
                {ActionType.MOVE, movementHighlighterAsset},
                {ActionType.ATTACK, attackHighlighterAsset},
                {ActionType.BUFF, buffHighlighterAsset}
            };
        }

        public void UpdateActionPoints(int actionPoints)
        {
            actionPointsCounter.text = actionPoints.ToString();
        }

        public void SetEnemyTurn(bool isEnemyTurn)
        {
            turnPanel.SetActive(isEnemyTurn);
        }

        public void ExitToMap()
        {
            SceneManager.LoadScene("Map");
        }

        public ActionPanel GetActionPanel()
        {
            return actionPanel;
        }

        public static void ActivateHighlights(List<TileNode> tiles, ActionType highlightType)
        {
            _highlightedTiles = tiles;

            foreach (var tile in tiles)
            {
                tile.GetTileData().AddHighlighter(_highlights[highlightType]);
            }
        }

        public static void DeactivateHighlights()
        {
            if (_highlightedTiles == null) return;
            
            foreach (var tile in _highlightedTiles)
            {
                tile.GetTileData().ClearHighlighter();
            }
            _highlightedTiles = null;
            LevelController.GetSelectedUnit().SelectedAction = ActionType.NONE;
        }
    }
}