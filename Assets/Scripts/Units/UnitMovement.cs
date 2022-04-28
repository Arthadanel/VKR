using System;
using DefaultNamespace;

namespace Units
{
    public static class UnitMovement
    {
        public static void Move(Unit unit, Coordinates start, Coordinates end)
        {
            LevelMapControl.DeactivateUniteSelector();
            unit.transform.position = end.GetVector3(LevelMap.UnitLayer);
        }

        public static void BuildPath(Coordinates start, Coordinates end)
        {
            
        }
    }
}