using System;
using UI;
using Units;
using UnityEngine;

public class GameController:MonoBehaviour
{
    [SerializeField] private LevelMap _levelGen;
    [SerializeField] private Camera _camera;
    [SerializeField] private GUIController _guiController;
    [SerializeField] private GameObject tileSelectorAsset;
    [SerializeField] private GameObject unitSelectorAsset;

    private void Awake()
    {
        _levelGen.OnMapGenerationFinished += SetCameraPosition;
    }

    private void Start()
    {
        GameObject selector = Instantiate(tileSelectorAsset, Vector3.zero, Quaternion.identity);
        selector.SetActive(false);
        LevelController.SetTileSelector(selector);
            
        GameObject unitSelector = Instantiate(unitSelectorAsset, Vector3.zero, Quaternion.identity);
        unitSelector.SetActive(false);
        LevelController.SetUnitSelector(unitSelector);
        
        LevelController.SetGUIController(_guiController);
    }

    private void SetCameraPosition()
    {
        _camera.transform.position = _levelGen.GetCentralPoint();
    }
}