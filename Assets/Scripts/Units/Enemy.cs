using System;
using DefaultNamespace;
using UnityEngine;

namespace Units
{
    public class Enemy:Unit
    {
        private void OnMouseDown()
        {
            if(LevelMapControl.IsUnitSelected)
            {
                Unit selectedUnit = LevelMapControl.GetSelectedUnit();
                if (selectedUnit.Coordinates.NextTo(_coordinates))
                {
                    selectedUnit.Fight(this);
                }
            }
        }
    }
}