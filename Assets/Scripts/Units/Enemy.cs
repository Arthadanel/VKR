using System;
using System.Collections.Generic;
using Data;
using Tiles;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Units
{
    public class Enemy:Unit
    {
        public static readonly int AI_DETECTION_RADIUS = SaveData.IS_DIFFICULT ? 10 : 7;
        public string Act()
        {
            bool hasTarget;
            string actionResult = "";
            string scenarioResult = "";
            switch (specialAction)
            {
                case "attack":
                    actionResult += "Warrior at " + Coordinates + " ";
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        scenarioResult = WarriorScenario();
                    else
                    {
                        actionResult += "blindly charged at his target";
                    }
                    break;
                case "shoot":
                    actionResult += "Archer at " + Coordinates + " ";
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        scenarioResult =ArcherScenario();
                    else
                    {
                        actionResult += "blindly charged at his target";
                    }
                    break;
                case "heal":
                    actionResult += "Healer at " + Coordinates + " ";
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        scenarioResult =HealerScenario();
                    else
                    {
                        actionResult += "blindly charged at his target";
                    }
                    break;
                case "taunt":
                    actionResult += "Tank at " + Coordinates + " ";
                    hasTarget = HasTargetScenario();
                    if (!hasTarget)
                        scenarioResult =TankScenario();
                    else
                    {
                        actionResult += "blindly charged at his target";
                    }
                    break;
            }
            CleanUp();
            if (scenarioResult == "")
            {
                scenarioResult = "waited";
            }
            actionResult += scenarioResult;
            return actionResult;
        }

        private string WarriorScenario()
        {
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return "";
            
            Tile closest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target.TileInteractionCost < closest.TileInteractionCost)
                {
                    closest = target;
                }
            }

            Unit finalTarget = closest.GetCurrentUnit();
            Coordinates targetCoordinates = finalTarget.Coordinates;
            bool targetInRange = PursueTarget(finalTarget, ActionType.ATTACK);
            return (targetInRange ? "attacked " : "moved towards ") + "unit at " + targetCoordinates;
        }

        private string ArcherScenario()
        {
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return "";
            
            Tile weakest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target.GetCurrentUnit().HealthBar.MaxHP() < weakest.GetCurrentUnit().HealthBar.MaxHP())
                {
                    weakest = target;
                }
            }
            Unit finalTarget = weakest.GetCurrentUnit();
            Coordinates targetCoordinates = finalTarget.Coordinates;
            bool targetInRange = PursueTarget(finalTarget, ActionType.SPECIAL);
            return (targetInRange ? "attacked " : "moved towards ") + "unit at " + targetCoordinates;
        }

        private string HealerScenario()
        {
            List<Tile> targets = DetectVisibleTargets(false);
            if (targets.Count == 0) return "";
            
            
            Tile weakest = targets[0];
            for (var i = 1; i < targets.Count; i++)
            {
                var target = targets[i];

                if (target.GetCurrentUnit().HealthBar.GetCurrentHP() < weakest.GetCurrentUnit().HealthBar.GetCurrentHP())
                {
                    weakest = target;
                }
            }

            if (weakest.GetCurrentUnit().HealthBar.GetCurrentHP() == weakest.GetCurrentUnit().HealthBar.MaxHP()) return "";
            
            Unit finalTarget = weakest.GetCurrentUnit();
            Coordinates targetCoordinates = finalTarget.Coordinates;
            bool targetInRange = PursueTarget(finalTarget, ActionType.SPECIAL);
            return (targetInRange ? "healed " : "moved towards ") + "unit at " + targetCoordinates;
        }

        private string TankScenario()
        {
            List<Tile> targets = DetectVisibleTargets();
            if (targets.Count == 0) return "";
            if (targets.Count > 5)
            {
                SelectedAction = ActionType.SPECIAL;
                InteractWith(null);
                return "taunted";
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

            Unit finalTarget = closest.GetCurrentUnit();

            if (weakest.TileInteractionCost <= closest.TileInteractionCost)
            {
                finalTarget = weakest.GetCurrentUnit();
            }
            else if (closest.GetCurrentUnit().HealthBar.MaxHP() <= weakest.GetCurrentUnit().HealthBar.MaxHP())
            {
                finalTarget = closest.GetCurrentUnit();
            }
            else if (weakest.TileInteractionCost < GetMovement())
            {
                finalTarget = weakest.GetCurrentUnit();
            }

            bool targetInRange = PursueTarget(finalTarget,ActionType.ATTACK);
            return (targetInRange ? "attacked " : "moved towards ") + "unit at " + closest.GetCurrentUnit().Coordinates;
        }

        private bool HasTargetScenario()
        {
            if(Target is null) return false;
            PursueTarget(Target, ActionType.ATTACK);
            Target = null;
            return true;
        }

        private List<Tile> DetectVisibleTargets(bool detectPlayerUnits = true)
        {
            List<Tile> result = new List<Tile>();
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(AI_DETECTION_RADIUS, true);
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