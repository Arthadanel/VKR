using System;
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
        public UnitHealthBar HealthBar { private set; get; }

        private float _powerModifier = 0.5f;
        protected Coordinates _coordinates;
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
            return specialAction == "attack" ? power : (int) (power * _powerModifier);
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

        private void Start()
        {
            HealthBar = GetComponentInChildren<UnitHealthBar>();
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
