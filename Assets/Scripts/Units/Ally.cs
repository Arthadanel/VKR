using System;
using UnityEngine;

namespace Units
{
    public class Ally:Unit
    {
        private void OnMouseDown()
        {
            LevelMapControl.DeactivateMoveReachHighlight();
            if (LevelMapControl.IsUnitSelected)
            {
                if (LevelMapControl.GetSelectedUnit().specialAction == "heal")
                { 
                    HealthBar.ChangeHP(power);
                }
            }
            else
            {
                UnitNavigation.DisplayMovementArea(this);
            }
            LevelMapControl.ActivateUnitSelector(this);
        }
    }
}