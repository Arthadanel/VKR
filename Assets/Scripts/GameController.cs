using System;
using Units;
using UnityEngine;

public class GameController:MonoBehaviour
{
    [SerializeField] private LevelMap levelGen;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject tileSelectorAsset;
    [SerializeField] private GameObject unitSelectorAsset;
    [SerializeField] private GameObject movementHighlighterAsset;
    [SerializeField] private GameObject attackHighlighterAsset;
    [SerializeField] private GameObject pathHighlighterAsset;

    private void Awake()
    {
        levelGen.OnMapGenerationFinished += SetCameraPosition;
    }

    private void Start()
    {
        GameObject selector = Instantiate(tileSelectorAsset, Vector3.zero, Quaternion.identity);
        selector.SetActive(false);
        LevelMapControl.SetTileSelector(selector);
            
        GameObject unitSelector = Instantiate(unitSelectorAsset, Vector3.zero, Quaternion.identity);
        unitSelector.SetActive(false);
        LevelMapControl.SetUnitSelector(unitSelector);
        
        LevelMapControl.SetHighlighters(movementHighlighterAsset,attackHighlighterAsset,pathHighlighterAsset);
    }

    private void SetCameraPosition()
    {
        _camera.transform.position = levelGen.GetCentralPoint();
    }
}