using System;
using System.Collections.Generic;
using Data;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public const int MOVE_COST = 1;
        public const int ATTACK_COST = 2;
        public const int SPECIAL_COST = 3;
        [SerializeField] protected string specialAction;
        [SerializeField] protected int power = 5;
        [SerializeField] protected int range = 1;
        [SerializeField] protected int movement = 3;
        [SerializeField] protected int health = 48;
        [SerializeField] protected UnitType unitType = UnitType.DAMAGE;
        public UnitHealthBar HealthBar { private set; get; }
        private Dictionary<UnitType, float> _powerModifiers;
        protected Coordinates _coordinates;
        public ActionType SelectedAction { get; set; }
        public Unit Target { get; set; }
        public int MovePenalty { get; private set; } = 0;

        protected List<TileNode> _tilesToClean = new List<TileNode>();

        private void Start()
        {
            HealthBar = GetComponentInChildren<UnitHealthBar>();
            HealthBar.OnLethalDamage = OnDeath;
            HealthBar.SetMaxHealth(health);
            _powerModifiers = new Dictionary<UnitType, float>
            {
                {UnitType.DAMAGE, 1f},
                {UnitType.SUPPORT, 0.6f}
            };
        }
        
        public Coordinates Coordinates
        {
            get => _coordinates;
            set
            {
                UnitNavigation.Move(this,_coordinates,value);//initiate movement
                _coordinates = value;
            }
        }

        public string GetSpecialName()
        {
            return specialAction;
        }

        public int GetAttackPower()
        {
            return (int) (power * _powerModifiers[unitType]);
        }

        public int GetMovement()
        {
            return movement;
        }

        public int GetSpecialRange()
        {
            return range;
        }

        public int GetSpecialAttackPower()
        {
            return power;
        }

        // public Func<Tile, bool> GetSpecialAction()
        // {
        //     return SpecialAction;
        // }

        public bool SpecialAction(Tile tile)
        {
            bool success = false;
            switch (GetSpecialName())
            {
                case "heal":
                    if (!tile.IsOccupied) return false;
                    Unit unit = tile.GetCurrentUnit();
                    success = unit.HealthBar.ChangeHP(+GetSpecialAttackPower());
                    break;
                case "taunt":
                    success = LevelController.MassSetPriorityTarget(this);
                    break;
            }
            return success;
        }

        public void SetInitialCoordinates(int row, int column)
        {
            _coordinates = new Coordinates(row, column);
        }

        public void RespondToTaunts()
        {
            if (Target != null)
            {
                PursueTarget(Target, ActionType.ATTACK);
                Target = null;
            }
        }

        public bool Fight(Tile tile)
        {
            if (!tile.IsOccupied) return false;
            
            Unit victim = tile.GetCurrentUnit();
            victim.HealthBar.ChangeHP(-GetAttackPower());

            return true;
        }

        public void InteractWith(Tile tile)
        {
            bool actionPerformed = false;
            int actionCost = 0;
            switch (SelectedAction)
            {
                case ActionType.MOVE:
                    tile.MoveTo(this);
                    actionCost = tile.TileInteractionCost + MovePenalty;
                    if (MovePenalty == 0)
                    {
                        MovePenalty = 1;
                        LevelController.OnTurnPass += () =>
                        {
                            MovePenalty = 0;
                        };
                    }
                    actionPerformed = true;
                    break;
                case ActionType.ATTACK:
                    actionPerformed = Fight(tile);
                    actionCost = ATTACK_COST;
                    break;
                case ActionType.SPECIAL:
                    actionPerformed = (unitType == UnitType.DAMAGE) ? Fight(tile) : SpecialAction(tile);
                    actionCost = SPECIAL_COST;
                    break;
            }

            if (actionPerformed)
            {
                LevelController.ConsumeActionPoints(actionCost);
                LevelController.DeactivateUnitSelection();
            }
        }

        protected bool PursueTarget(Unit target, ActionType action)
        {
            CleanUp();
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            int targetRange = action == ActionType.ATTACK ? 1 : GetSpecialRange();
            List<TileNode> reachableTiles = start.GetTilesInRange(targetRange,true);
            _tilesToClean.AddRange(reachableTiles);
            
            Tile tile = LevelController.GetTileAtCoordinates(target.Coordinates).GetTileData();
            
            bool success = false;
            switch (action)
            {
                case ActionType.ATTACK:
                    if (tile.TileInteractionCost <= 1)
                    {
                        SelectedAction = action;
                        InteractWith(tile);
                        success = true;
                    }
                    break;
                case ActionType.SPECIAL:
                    if (tile.TileInteractionCost <= GetSpecialRange())
                    {
                        SelectedAction = action;
                        InteractWith(tile);
                        success = true;
                    }
                    break;
            }

            if (success) return true;
            MoveTowards(tile);
            return false;
        }

        private void MoveTowards(Tile tile)
        {
            CleanUp();
            TileNode start = LevelController.GetTileAtCoordinates(Coordinates);
            List<TileNode> reachableTiles = start.GetTilesInRange(movement);
            _tilesToClean.AddRange(reachableTiles);
            
            var result = reachableTiles[0].GetTileData();
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

        protected void CleanUp()
        {
            SelectedAction = ActionType.NONE;
            foreach (var tileNode in _tilesToClean)
            {
                tileNode.GetTileData().TileInteractionCost = GameSettings.MOVE_LIMIT;
            }

            _tilesToClean = new List<TileNode>();
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            LevelController.PositionSelector(_coordinates);
        }

        public ActionType GetSpecialType()
        {
            return unitType == UnitType.DAMAGE ? ActionType.ATTACK : ActionType.SPECIAL;
        }

        private void OnDeath()
        {
            TileNode tile = LevelController.GetTileAtCoordinates(Coordinates);
            tile.GetTileData().Vacate();
            if (this is Enemy)
            {
                List<Enemy> listE = LevelController.GetEnemyList();
                listE.Remove(this as Enemy);
                if (listE.Count==0)
                {
                    LevelController.OnLevelCleared();
                }
            }
            if (this is Ally)
            {
                List<Ally> listA = LevelController.GetAllyList();
                listA.Remove(this as Ally);
                if (listA.Count==0)
                {
                    LevelController.OnLoss();
                }
            }
            Destroy(gameObject);
        }
    }
}
