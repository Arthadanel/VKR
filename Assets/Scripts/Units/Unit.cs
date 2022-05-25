using System;
using UnityEngine;
using Utility;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected internal string specialAction;
        [SerializeField] protected int power = 5;
        [SerializeField] protected int range = 1;
        [SerializeField] protected int movement = 3;
        public UnitHealthBar HealthBar { private set; get; }

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

        public int Movement
        {
            get => movement;
            set => movement = value;
        }

        public int Range
        {
            get => range;
            set => range = value;
        }

        private void Start()
        {
            HealthBar = GetComponentInChildren<UnitHealthBar>();
        }

        public int GetAttack()
        {
            return power;
            //todo: buffs
        }

        public void SetInitialCoordinates(int row, int column)
        {
            _coordinates = new Coordinates(row, column);
        }

        public virtual bool Fight(Unit victim)
        {
            victim.HealthBar.ChangeHP(-power);
            LevelMapControl.DeactivateUnitSelector();
            return true;
        }

        private void OnMouseEnter()
        {
            LevelMapControl.PositionSelector(_coordinates);
        }
    }
}
