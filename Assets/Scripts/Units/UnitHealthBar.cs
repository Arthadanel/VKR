using System;
using UnityEngine;

namespace Units
{
    public class UnitHealthBar:MonoBehaviour
    {
        [SerializeField] private GameObject filler;
        private int _maxHP = 10;
        private int _currentHP = 10;

        public Action OnLethalDamage;

        public int MaxHP()
        {
            return _maxHP;
        }

        public bool ChangeHP(int value)
        {
            if (_currentHP == _maxHP && value > 0)
                return false;
            _currentHP += value;
            if (_currentHP > _maxHP)
                _currentHP = _maxHP;
            if (_currentHP <= 0)
            {
                OnLethalDamage();
                return false;
            }

            float ratio = (float)_currentHP / _maxHP;
            filler.transform.localScale = new Vector3(ratio,1,1);
            return true;
        }

        public int GetCurrentHP()
        {
            return _currentHP;
        }

        public void SetMaxHealth(int health)
        {
            _maxHP = health;
            _currentHP = _maxHP;
        }
    }
}