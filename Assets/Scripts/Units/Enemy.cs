using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units
{
    public class Enemy:Unit
    {
        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if(LevelController.IsUnitSelected)
            {
                GUIController.DeactivateHighlights();

                Unit selectedUnit = LevelController.GetSelectedUnit();
                if (selectedUnit.Coordinates.NextTo(_coordinates))
                {
                    selectedUnit.Fight(this);
                }
            }
        }
    }
}