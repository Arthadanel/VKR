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
            LevelMapControl.DeactivateUnitSelector();
            unit.transform.position = end.GetVector3(LevelMap.UnitLayer);
        }

        public static void BuildPath(Coordinates start, Coordinates end)
        {
            
        }

        public static void DisplayMovementArea(Unit unit)
        {
            int maxCost = unit.Movement;
            TileNode start = LevelMapControl.GetTileAtCoordinates(unit.Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(maxCost);
            reachableTiles = reachableTiles.Distinct().ToList();
            //reachableTiles.RemoveAt(0);
            
            LevelMapControl.ActivateMoveReachHighlight(reachableTiles);

        }
        public static void DisplayAttackArea(Unit unit)
        {
            int maxCost = unit.Range;
            TileNode start = LevelMapControl.GetTileAtCoordinates(unit.Coordinates);

        }
    }
}