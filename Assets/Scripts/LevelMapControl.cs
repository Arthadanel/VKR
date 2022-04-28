using Units;
using UnityEngine;

namespace DefaultNamespace
{
    public static class LevelMapControl
    {
        private static GameObject _selector;
        private static GameObject _unitSelector;
        private static Unit _selectedUnit;
        public static bool IsUnitSelected { get; private set; }

        public static void SetSelector(GameObject selector)
        {
            _selector = selector;
        }

        public static void SetUnitSelector(GameObject unitSelector)
        {
            _unitSelector = unitSelector;
        }

        public static void PositionSelector(Coordinates coordinates)
        {
            _selector.transform.position = coordinates.GetVector3(-5);
            _selector.SetActive(true);
        }

        public static void ActivateUnitSelector(Unit unit)
        {
            if (_unitSelector.activeSelf)
            {
                DeactivateUniteSelector();
                return;
            }
            _unitSelector.transform.position = unit.Coordinates.GetVector3(-4);
            _unitSelector.SetActive(true);
            IsUnitSelected = true;
            _selectedUnit = unit;
        }

        public static Unit GetSelectedUnit()
        {
            return _selectedUnit;
        }

        public static void DeactivateUniteSelector()
        {
            _unitSelector.SetActive(false);
            IsUnitSelected = false;
        }
    }
}