using System;
using UnityEngine;
using Utility;

namespace Units
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] protected internal string specialAction;
        protected int _attack = 5;
        protected int _range = 1;
        protected int _movement = 3;
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
            get => _movement;
            set => _movement = value;
        }

        public int Range
        {
            get => _range;
            set => _range = value;
        }

        private void Start()
        {
            HealthBar = GetComponentInChildren<UnitHealthBar>();
            if (specialAction == "attack")
            {
                HealthBar.ChangeHP(-7);
            }
        }

        public int GetAttack()
        {
            return _attack;
            //todo: buffs
        }

        public void SetInitialCoordinates(int row, int column)
        {
            _coordinates = new Coordinates(row, column);
        }

        public virtual bool Fight(Unit victim)
        {
            victim.HealthBar.ChangeHP(-_attack);
            LevelMapControl.DeactivateUnitSelector();
            return true;
        }

        private void OnMouseEnter()
        {
            LevelMapControl.PositionSelector(_coordinates);
        }
    }
}
