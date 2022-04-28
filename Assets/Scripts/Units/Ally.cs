using System;
using DefaultNamespace;
using UnityEngine;

namespace Units
{
    public class Ally:Unit
    {
        private void OnMouseDown()
        {
            if (LevelMapControl.IsUnitSelected)
            {
                if (LevelMapControl.GetSelectedUnit().specialAction == "heal")
                { 
                    HealthBar.ChangeHP(_attack);
                }
            }
            LevelMapControl.ActivateUnitSelector(this);
        }
    }
}