using UnityEngine;

namespace Units
{
    public class UnitHealthBar:MonoBehaviour
    {
        [SerializeField] private GameObject filler;
        private int _maxHP = 10;
        private int _currentHP = 10;

        public int MaxHP()
        {
            return _maxHP;
        }

        public void ChangeHP(int value)
        {
            _currentHP += value;
            if (_currentHP > _maxHP)
                _currentHP = _maxHP;
            if (_currentHP <= 0)
            {
                Destroy(transform.parent.gameObject);
            }

            float ratio = (float)_currentHP / _maxHP;
            filler.transform.localScale = new Vector3(ratio,1,1);
        }
    }
}