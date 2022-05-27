using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units
{
    public class Enemy:Unit
    {
        public static readonly int AI_DETECTION_RADIUS = SaveData.IS_DIFFICULT ? 10 : 7;
        // private void OnMouseDown()
        // {
        //     if (EventSystem.current.IsPointerOverGameObject()) return;
        //     
        //     if(LevelController.IsUnitSelected)
        //     {
        //         GUIController.DeactivateHighlights();
        //
        //         // Unit selectedUnit = LevelController.GetSelectedUnit();
        //         // if (selectedUnit.Coordinates.NextTo(_coordinates))
        //         // {
        //         //     selectedUnit.Fight(this);
        //         // }
        //     }
        // }

        private List<TileNode> _tilesToClean = new List<TileNode>();
        public void Act()
        {
            bool hasTarget;
            switch (specialAction)
            {
                case "attack":
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        WarriorScenario();
                    break;
                case "shoot":
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        ArcherScenario();
                    break;
                case "heal":
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        HealerScenario();
                    break;
                case "taunt":
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        TankScenario();
                    break;
            }
            CleanUp();
        }

        private void CleanUp()
        {
            SelectedAction = ActionType.NONE;
            foreach (var tileNode in _tilesToClean)
            {
                tileNode.GetTileData().TileInteractionCost = GameSettings.MOVE_LIMIT;
            }

            _tilesToClean = new List<TileNode>();
        }

        private void WarriorScenario()
        {
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return;
            
            Tile closest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target.TileInteractionCost < closest.TileInteractionCost)
                {
                    closest = target;
                }
            }

            PursueTarget(closest.GetCurrentUnit(), ActionType.ATTACK);
        }

        private void ArcherScenario()
        {
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return;
            
            Tile weakest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target.GetCurrentUnit().HealthBar.MaxHP() < weakest.GetCurrentUnit().HealthBar.MaxHP())
                {
                    weakest = target;
                }
            }

            PursueTarget(weakest.GetCurrentUnit(),
                weakest.TileInteractionCost <= GetMovement() + 1 ? ActionType.ATTACK : ActionType.SPECIAL);
        }

        private void HealerScenario()
        {
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<Tile> targets = DetectVisibleTargets(false);
            if (targets.Count == 0) return;
            
            
            Tile weakest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target.GetCurrentUnit().HealthBar.GetCurrentHP() < weakest.GetCurrentUnit().HealthBar.GetCurrentHP())
                {
                    weakest = target;
                }
            }

            if (weakest.GetCurrentUnit().HealthBar.GetCurrentHP() == weakest.GetCurrentUnit().HealthBar.MaxHP()) return;
            
            PursueTarget(weakest.GetCurrentUnit(), ActionType.SPECIAL);
        }

        private void TankScenario()
        {
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return;
            if (targets.Count > 5)
            {
                //todo:taunt condition and trigger
                //InteractWith(start.GetTileData());
                return;
            }
            Tile closest = targets[0];
            Tile weakest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target.TileInteractionCost < closest.TileInteractionCost)
                {
                    closest = target;
                }

                if (target.GetCurrentUnit().HealthBar.MaxHP() < weakest.GetCurrentUnit().HealthBar.MaxHP())
                {
                    weakest = target;
                }
            }

            if (weakest.TileInteractionCost <= closest.TileInteractionCost)
            {
                PursueTarget(weakest.GetCurrentUnit(),ActionType.ATTACK);
                return;
            }

            if (closest.GetCurrentUnit().HealthBar.MaxHP() <= weakest.GetCurrentUnit().HealthBar.MaxHP())
            {
                PursueTarget(closest.GetCurrentUnit(),ActionType.ATTACK);
                return;
            }

            if (weakest.TileInteractionCost < GetMovement())
            {
                PursueTarget(weakest.GetCurrentUnit(),ActionType.ATTACK);
                return;
            }

            PursueTarget(closest.GetCurrentUnit(),ActionType.ATTACK);
        }

        private bool HasTargetScenario()
        {
            if(Target is null) return false;
            PursueTarget(Target, ActionType.ATTACK);
            Target = null;
            return true;
        }

        private void PursueTarget(Unit target, ActionType action)
        {
            Tile tile = LevelController.GetTileAtCoordinates(target.Coordinates).GetTileData();
            bool success = false;
            switch (action)
            {
                case ActionType.ATTACK:
                    if (tile.TileInteractionCost <= GetMovement() + 1)
                    {
                        SelectedAction = action;
                        InteractWith(tile);
                        success = true;
                    }
                    break;
                case ActionType.SPECIAL:
                    if (tile.TileInteractionCost <= GetMovement() + GetSpecialRange())
                    {
                        SelectedAction = action;
                        InteractWith(tile);
                        success = true;
                    }
                    break;
            }

            if (success) return;
            MoveTowards(tile);
        }

        private void MoveTowards(Tile tile)
        {
            CleanUp();
            int signX = (Coordinates.X - tile.Coordinates.X) > 0 ? -1 : 1;
            int signY = (Coordinates.Y - tile.Coordinates.Y) > 0 ? -1 : 1;
            Tile result;
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(movement);
            _tilesToClean.AddRange(reachableTiles);
            result = reachableTiles[0].GetTileData();
            for (var i = 1; i < reachableTiles.Count; i++)
            {
                var tileNode = reachableTiles[i];
                if (tileNode.GetTileData().Coordinates.Distance(tile.Coordinates) < result.Coordinates.Distance(tile.Coordinates))
                {
                    result = tileNode.GetTileData();
                }
            }

            if (result is null) return;
            SelectedAction = ActionType.MOVE;
            InteractWith(result);
        }

        private List<Tile> DetectVisibleTargets(bool detectPlayerUnits = true)
        {
            List<Tile> result = new List<Tile>();
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(AI_DETECTION_RADIUS);
            _tilesToClean.AddRange(reachableTiles);
            foreach (var tile in reachableTiles)
            {
                if (!tile.GetTileData().IsOccupied) continue;
                Unit unit = tile.GetTileData().GetCurrentUnit();
                if(unit is Ally != detectPlayerUnits) continue;
                result.Add(tile.GetTileData());
            }
            return result;
        }
    }
}