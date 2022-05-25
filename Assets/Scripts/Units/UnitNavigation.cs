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
            LevelController.DeactivateUnitSelection();
            unit.transform.position = end.GetVector3(LevelMap.UnitLayer);
        }

        public static void BuildPath(Coordinates start, Coordinates end)
        {
            
        }

        public static void DisplayMovementArea(Unit unit)
        {
            int maxCost = unit.GetMovement();
            TileNode start = LevelController.GetTileAtCoordinates(unit.Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(maxCost+1);
            reachableTiles = reachableTiles.Distinct().ToList();
            //reachableTiles.RemoveAt(0);
            
            LevelController.ActivateMoveReachHighlight(reachableTiles);

        }
        public static void DisplayAttackArea(Unit unit)
        {
            int maxCost = unit.GetSpecialRange();
            TileNode start = LevelController.GetTileAtCoordinates(unit.Coordinates);

        }
    }
}