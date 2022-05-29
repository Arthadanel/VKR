using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units
{
    public class Ally:Unit
    {
        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if (LevelController.IsUnitSelected)
            {
                LevelController.DeactivateUnitSelection();
            }
            else
            {
                LevelController.ActivateUnitSelection(this);
            }
            //=============
            
            // LevelMapControl.DeactivateMoveReachHighlight();
            // if (LevelMapControl.IsUnitSelected)
            // {
            //     if (LevelMapControl.GetSelectedUnit().GetSpecialName() == "heal")
            //     { 
            //         HealthBar.ChangeHP(power);
            //     }
            // }
            // else
            // {
            //     UnitNavigation.DisplayMovementArea(this);
            // }
        }
    }
}