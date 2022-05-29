using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
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
        [SerializeField] private GameObject messageDisplay;
        
        [SerializeField] private GameObject movementHighlighterAsset;
        [SerializeField] private GameObject attackHighlighterAsset;
        [SerializeField] private GameObject buffHighlighterAsset;

        private static Dictionary<ActionType, GameObject> _highlights;

        private static List<TileNode> _highlightedTiles;

        public bool IsMessageVisible { get; private set; }

        private void Start()
        {
            _highlights= new Dictionary<ActionType, GameObject>
            {
                {ActionType.MOVE, movementHighlighterAsset},
                {ActionType.ATTACK, attackHighlighterAsset},
                {ActionType.SPECIAL, buffHighlighterAsset}
            };
        }

        public void UpdateActionPoints(int actionPoints)
        {
            actionPointsCounter.text = actionPoints.ToString();
        }

        public void SetEnemyTurn(bool isEnemyTurn)
        {
            turnPanel.SetActive(isEnemyTurn);
            actionPanel.SetActiveState(false);
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

        private Coroutine _messageCoroutine;
        public void DisplayMessage(string message, bool isSystemMessage)
        {
            if (_messageCoroutine != null) StopCoroutine(_messageCoroutine);
            _messageCoroutine = StartCoroutine(ShowMessage(message, isSystemMessage));
        }

        IEnumerator ShowMessage(string message, bool isSystemMessage)
        {
            messageDisplay.SetActive(true);
            IsMessageVisible = true;
            messageDisplay.GetComponentInChildren<Text>().text = message;
            yield return new WaitForSeconds(isSystemMessage
                ? TurnController.SYSTEM_MESSAGE_PAUSE
                : TurnController.ENEMY_TURN_PAUSE);
            messageDisplay.SetActive(false);
            IsMessageVisible = false;
        }

        public void CallDoAfterSystemMessage(Action action,string message)
        {
            StartCoroutine(DoAfterSystemMessage(action,message));
        }
    
        private IEnumerator DoAfterSystemMessage(Action action,string message)
        {
            if (_messageCoroutine != null) StopCoroutine(_messageCoroutine);
            messageDisplay.SetActive(true);
            IsMessageVisible = true;
            messageDisplay.GetComponentInChildren<Text>().text = message;
            yield return new WaitForSeconds(TurnController.SYSTEM_MESSAGE_PAUSE);
            messageDisplay.SetActive(false);
            IsMessageVisible = false;
            action();
        }
    }
}