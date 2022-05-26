using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace Units
{
    public static class UnitNavigation
    {
        public static void Move(Unit unit, Coordinates start, Coordinates end)
        {
            unit.transform.position = end.GetVector3(LevelMap.UnitLayer);
        }
    }
}