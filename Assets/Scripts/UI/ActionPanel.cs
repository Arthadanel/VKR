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
        [SerializeField] private Color highlightColor;

        public void InitializeActionPanel(Unit unit)
        {
            _unit = unit;
            moveCost.text = (_unit.GetMovement() > 1 ? (1 + unit.MovePenalty) + "-" : "") +
                            (_unit.GetMovement() + unit.MovePenalty);
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
            List<TileNode> reachableTiles = start.GetTilesInRange(range, 
                highlightType != ActionType.MOVE, highlightType != ActionType.ATTACK);
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
            
            _unit.SelectedAction = ActionType.SPECIAL;
            
            HighlightTilesInRange(_unit.GetSpecialRange(),_unit.GetSpecialType());
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
            button.GetComponent<Image>().color =highlightColor;
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