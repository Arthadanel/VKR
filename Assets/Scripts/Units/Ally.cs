using System;
using Tiles;
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
                Unit unit = LevelController.GetSelectedUnit();
                if (unit == this)
                    LevelController.DeactivateUnitSelection();
                else
                {
                    if (unit.GetSpecialName() == "heal")
                    {
                        HealthBar.ChangeHP(unit.GetSpecialAttackPower());
                        LevelController.ConsumeActionPoints(Unit.SPECIAL_COST);
                        LevelController.DeactivateUnitSelection();
                    }
                }
            }
            else
            {
                LevelController.ActivateUnitSelection(this);
            }
        }
    }
}