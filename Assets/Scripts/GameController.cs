using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameController:MonoBehaviour
    {
        [SerializeField] private GameObject selectorAsset;
        [SerializeField] private GameObject unitSelectorAsset;

        private void Start()
        {
            GameObject selector = Instantiate(selectorAsset, Vector3.zero, Quaternion.identity);
            selector.SetActive(false);
            LevelMapControl.SetSelector(selector);
            
            GameObject unitSelector = Instantiate(unitSelectorAsset, Vector3.zero, Quaternion.identity);
            unitSelector.SetActive(false);
            LevelMapControl.SetUnitSelector(unitSelector);
        }
    }
}