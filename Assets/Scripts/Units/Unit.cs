using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        public const int MOVE_COST = 2;
        public const int ATTACK_COST = 2;
        public const int SPECIAL_COST = 3;
        [SerializeField] protected string specialAction;
        [SerializeField] protected int power = 5;
        [SerializeField] protected int range = 1;
        [SerializeField] protected int movement = 3;
        [SerializeField] protected UnitType unitType = UnitType.DAMAGE;
        public UnitHealthBar HealthBar { private set; get; }
        private Dictionary<UnitType, float> _powerModifiers;
        protected Coordinates _coordinates;
        public ActionType SelectedAction { get; set; }
        public Unit Target { get; set; }
        public int MovePenalty { get; private set; } = 0;

        private void Start()
        {
            HealthBar = GetComponentInChildren<UnitHealthBar>();
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
            switch (specialAction)
            {
                case "heal":
                    if (!tile.IsOccupied) return false;
                    success = tile.GetCurrentUnit().HealthBar.ChangeHP(+GetSpecialAttackPower());
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
                    actionPerformed = unitType == UnitType.DAMAGE ? Fight(tile) : SpecialAction(tile);
                    actionCost = SPECIAL_COST;
                    break;
            }

            if (actionPerformed)
            {
                LevelController.ConsumeActionPoints(actionCost);
                LevelController.DeactivateUnitSelection();
            }
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
    }
}
