using System;
using System.Collections;
using Level;
using UI;
using Units;
using UnityEngine;

public class GameController:MonoBehaviour
{
    [SerializeField] private LevelMap _levelGen;
    [SerializeField] private Camera _camera;
    [SerializeField] private GUIController _guiController;
    [SerializeField] private TurnController _turnController;
    [SerializeField] private GameObject tileSelectorAsset;
    [SerializeField] private GameObject unitSelectorAsset;
    [SerializeField] private GameObject inputBlocker;

    private void Awake()
    {
        _levelGen.OnMapGenerationFinished += SetCameraPosition;
    }

    private void Start()
    {
        LevelController.Reset();
        GameObject selector = Instantiate(tileSelectorAsset, Vector3.zero, Quaternion.identity);
        selector.SetActive(false);
        LevelController.SetTileSelector(selector);
            
        GameObject unitSelector = Instantiate(unitSelectorAsset, Vector3.zero, Quaternion.identity);
        unitSelector.SetActive(false);
        LevelController.SetUnitSelector(unitSelector);
        
        LevelController.SetGUIController(_guiController);
        LevelController.SetTurnController(_turnController);
        LevelController.SetInputBlocker(inputBlocker);
    }

    private void SetCameraPosition()
    {
        _camera.transform.position = _levelGen.GetCentralPoint();
        _camera.GetComponent<CameraController>().SetCameraStartCoordinates();
    }
}