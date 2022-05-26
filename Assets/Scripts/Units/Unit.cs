using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected string specialAction;
        [SerializeField] protected int power = 5;
        [SerializeField] protected int range = 1;
        [SerializeField] protected int movement = 3;
        [SerializeField] protected UnitType unitType = UnitType.DAMAGE;
        public UnitHealthBar HealthBar { private set; get; }
        private Dictionary<UnitType, float> _powerModifiers;
        protected Coordinates _coordinates;
        public ActionType SelectedAction { get; set; }

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

        public UnitType GetUnitType()
        {
            return unitType;
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

        public Action GetSpecialAction()
        {
            return SpecialAction;
        }

        protected virtual void SpecialAction()
        {
            //todo
        }

        public void SetInitialCoordinates(int row, int column)
        {
            _coordinates = new Coordinates(row, column);
        }

        public virtual bool Fight(Unit victim)
        {
            victim.HealthBar.ChangeHP(-power);
            LevelController.DeactivateUnitSelection();
            return true;
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            LevelController.PositionSelector(_coordinates);
        }
    }
}
