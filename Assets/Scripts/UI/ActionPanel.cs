using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ActionPanel: MonoBehaviour
    {
        [SerializeField] private Text moveCost;
        [SerializeField] private Text attackName;
        [SerializeField] private Text specialActionName;
        [SerializeField] private Button moveBtn;
        [SerializeField] private Button attackBtn;
        [SerializeField] private Button specialBtn;
        [SerializeField] private Button cancelBtn;

        private Unit _unit;
        [SerializeField] private Color _highlightColor;

        public void InitializeActionPanel(Unit unit)
        {
            _unit = unit;
            moveCost.text = _unit.GetMovement().ToString();
            attackName.text = "Attack (" + _unit.GetAttackPower() + ")";
            specialActionName.text = _unit.GetSpecialName()[0].ToString().ToUpper() +
                                     _unit.GetSpecialName().Substring(1) + " (" + _unit.GetSpecialAttackPower() + ")";
            specialBtn.gameObject.SetActive(_unit.GetSpecialName() != "attack");
        }

        public void SetActiveState(bool isActive)
        {
            gameObject.SetActive(isActive);
            moveBtn.interactable = true;
            attackBtn.interactable = true;
            specialBtn.interactable = true;
            cancelBtn.interactable = false;
            ResetButtonColor();
            GUIController.DeactivateHighlights();
        }

        private void HighlightTilesInRange(int range, ActionType highlightType)
        {
            //TileNode.ResetMaxCost();
            TileNode start = LevelController.GetTileAtCoordinates(_unit.Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(range);
            reachableTiles = reachableTiles.Distinct().ToList();
            start.GetTileData().ClearHighlighter();
            
            GUIController.ActivateHighlights(reachableTiles,highlightType);
        }
        
        public void OnMove()
        {
            if (cancelBtn.interactable) return;
            
            _unit.SelectedAction = ActionType.MOVE;
            HighlightTilesInRange(_unit.GetMovement(),_unit.SelectedAction);
            HighlightButton(moveBtn);
        }
        
        public void OnAttack()
        {
            if (cancelBtn.interactable) return;
            
            _unit.SelectedAction = ActionType.ATTACK;
            HighlightTilesInRange(1,_unit.SelectedAction);
            HighlightButton(attackBtn);
        }
        
        public void OnSpecialAction()
        {
            if (cancelBtn.interactable) return;

            _unit.SelectedAction = _unit.GetUnitType() == UnitType.DAMAGE ? ActionType.ATTACK : ActionType.BUFF;
            HighlightTilesInRange(_unit.GetSpecialRange(),_unit.SelectedAction);
            HighlightButton(specialBtn);
        }

        public void OnCancel()
        {
            moveBtn.interactable = true;
            attackBtn.interactable = true;
            specialBtn.interactable = true;
            cancelBtn.interactable = false;
            ResetButtonColor();
            GUIController.DeactivateHighlights();
        }

        private void ChangeMoveDisplay(int movesLeft)
        {
            int subStringStart = moveCost.text.IndexOf('/');
            moveCost.text = movesLeft + moveCost.text.Substring(subStringStart);
        }

        private void HighlightButton(Button button)
        {
            SetButtonsState(true);
            //button.interactable = true;
            button.GetComponent<Image>().color =_highlightColor;
        }

        private void ResetButtonColor()
        {
            moveBtn.GetComponent<Image>().color = Color.white;
            attackBtn.GetComponent<Image>().color = Color.white;
            specialBtn.GetComponent<Image>().color = Color.white;
        }

        private void SetButtonsState(bool isCancelActive)
        {
            moveBtn.interactable = !isCancelActive;
            attackBtn.interactable = !isCancelActive;
            specialBtn.interactable = !isCancelActive;
            cancelBtn.interactable = isCancelActive;
        }
    }
}