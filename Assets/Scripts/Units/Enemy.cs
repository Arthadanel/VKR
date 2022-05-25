using System;
using UnityEngine;

namespace Units
{
    public class Enemy:Unit
    {
        private void OnMouseDown()
        {
            if(LevelMapControl.IsUnitSelected)
            {
                LevelMapControl.DeactivateMoveReachHighlight();

                Unit selectedUnit = LevelMapControl.GetSelectedUnit();
                if (selectedUnit.Coordinates.NextTo(_coordinates))
                {
                    selectedUnit.Fight(this);
                }
            }
        }
    }
}