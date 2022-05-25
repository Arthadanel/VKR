using System.Collections.Generic;
using UI;
using Units;
using UnityEngine;
using Utility;

public static class LevelController
{
    private static GameObject _tileSelector;
    private static GameObject _unitSelector;
    private static GameObject _moveHighlighter;
    private static GameObject _attackHighlighter;
    private static GameObject _pathHighlighter;
    
    private static TileNode[,] _levelLayout;
    private static GUIController _guiController;
    
    private static Unit _selectedUnit;
    private static List<TileNode> lastAttackTiles;
    private static List<TileNode> lastMoveTiles;
    public static bool IsUnitSelected { get; private set; }

    #region PassGameControllerValues
    public static void SetLevelLayout(TileNode[,] map)
    {
        _levelLayout = map;
    }

    public static void SetGUIController(GUIController guiController)
    {
        _guiController = guiController;
    }

    public static void SetTileSelector(GameObject selector)
    {
        _tileSelector = selector;
    }

    public static void SetUnitSelector(GameObject unitSelector)
    {
        _unitSelector = unitSelector;
    }

    public static void SetHighlighters(GameObject move, GameObject attack, GameObject path)
    {
        _moveHighlighter = move;
        _attackHighlighter = attack;
        _pathHighlighter = path;
    }
    #endregion

    public static void PositionSelector(Coordinates coordinates)
    {
        _tileSelector.transform.position = coordinates.GetVector3(-5);
        _tileSelector.SetActive(true);
    }

    public static void ActivateUnitSelection(Unit unit)
    {
        _unitSelector.transform.position = unit.Coordinates.GetVector3(-4);
        _unitSelector.SetActive(true);
        IsUnitSelected = true;
        _selectedUnit = unit;
        //GUI
        _guiController.GetActionPanel().InitializeActionPanel(unit);
        _guiController.GetActionPanel().SetActiveState(true);
    }

    public static void DeactivateUnitSelection()
    {
        _unitSelector.SetActive(false);
        IsUnitSelected = false;
        //GUI
        _guiController.GetActionPanel().SetActiveState(false);
    }

    public static Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public static TileNode[,] GetCurrentLevelLayout()
    {
        return _levelLayout;
    }

    public static TileNode GetTileAtCoordinates(Coordinates coordinates)
    {
        return _levelLayout[coordinates.Row, coordinates.Column];
    }

    public static void ActivateMoveReachHighlight(List<TileNode> tiles)
    {
        lastMoveTiles = tiles;

        foreach (var tile in tiles)
        {
            tile.GetTileData().AddHighlighter(_moveHighlighter);
        }
    }

    public static void DeactivateMoveReachHighlight()
    {
        if (lastMoveTiles != null)
            foreach (var tile in lastMoveTiles)
            {
                tile.GetTileData().ClearHighlighter();
            }

        lastMoveTiles = null;
    }

    public static void ActivateAttackReachHighlight(List<TileNode> tiles)
    {
        foreach (var tile in tiles)
        {
            
        }
    }
}