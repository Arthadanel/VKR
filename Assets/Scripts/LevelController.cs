using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Level;
using Tiles;
using UI;
using Units;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

public static class LevelController
{
    private static GameObject _tileSelector;
    private static GameObject _unitSelector;
    private static GameObject _inputBlocker;
    
    private static TileNode[,] _levelLayout;
    private static GUIController _guiController;
    private static TurnController _turnController;
    
    private static Unit _selectedUnit;
    public static bool IsUnitSelected { get; private set; }
    public static int MaxActionPoints { get; private set; }

    public static Action OnTurnPass;
    public static Func<List<Enemy>> GetEnemyList;
    public static Func<List<Ally>> GetAllyList;

    public static int CURRENT_LEVEL;

    public static int ActionPoints
    {
        get => _actionPoints;
        set
        {
            _actionPoints = value;
            if (value <= 0)
            {
                _turnController.SwitchTurn();
            }
        }
    }
    public static void DisplayMessage(string message, bool isSystemMessage)
    {
        _guiController.DisplayMessage(message, isSystemMessage);
    }

    //public static List<Enemy> Enemies { get; set; }

    public static void PassTurn()
    {
        ActionPoints = MaxActionPoints;
        _guiController.UpdateActionPoints(ActionPoints);
        _guiController.SetEnemyTurn(!_turnController.IsPlayerTurn);
        AllowGameInput(_turnController.IsPlayerTurn);
        if (OnTurnPass != null) OnTurnPass();
        OnTurnPass = () => { };
        if (_turnController.IsPlayerTurn)
        {
            List<Ally> allies = GetAllyList();
            foreach (var ally in allies)
            {
                ally.RespondToTaunts();
            }
        }
    }

    private static void AllowGameInput(bool allow)
    {
        _inputBlocker.SetActive(!allow);
        _tileSelector.SetActive(allow);
    }

    private static int _actionPoints;
    private static int _tileCount;
    
    public static void Reset()
    {
        _selectedUnit = null;
        IsUnitSelected = false;
    }

    public static void SetGUIController(GUIController guiController)
    {
        _guiController = guiController;
    }
    
    public static void SetTurnController(TurnController turnController)
    {
        _turnController = turnController;
    }

    public static void SetTileSelector(GameObject selector)
    {
        _tileSelector = selector;
    }

    public static void SetInputBlocker(GameObject inputBlocker)
    {
        _inputBlocker = inputBlocker;
    }

    public static void SetUnitSelector(GameObject unitSelector)
    {
        _unitSelector = unitSelector;
    }

    public static void SetLevelLayout(TileNode[,] map)
    {
        _levelLayout = map;
    }

    public static void SetTileCount(int tileCount)
    {
        _tileCount = tileCount;
        MaxActionPoints = tileCount / (SaveData.IS_DIFFICULT ? GameSettings.TILE_AP_PERCENTAGE_HARD : GameSettings.TILE_AP_PERCENTAGE);
        ActionPoints = MaxActionPoints;
        _guiController.UpdateActionPoints(ActionPoints);
    }

    public static void ConsumeActionPoints(int ap)
    {
        ActionPoints -= ap;
        _guiController.UpdateActionPoints(ActionPoints);
    }

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
        return _levelLayout[coordinates.X, coordinates.Y];
    }

    //todo:currently ignoring AI taunting player units, fix later
    public static bool MassSetPriorityTarget(Unit taunterUnit)
    {
        bool result = false;
        TileNode start = GetTileAtCoordinates(taunterUnit.Coordinates);
        List<TileNode> reachableTiles = start.GetTilesInRange(taunterUnit.GetSpecialRange(),true);
        bool allyT = taunterUnit is Ally;
        bool allyU;
        foreach (var tile in reachableTiles)
        {
            if (!tile.GetTileData().IsOccupied) continue;
            Unit unit = tile.GetTileData().GetCurrentUnit();
            allyU = unit is Ally;
            if(allyT!=allyU) continue;
            result = true;
            unit.Target = taunterUnit;
        }

        return result;
    }

    public static void OnLevelCleared()
    {
        DisplayMessage("You have defeated enemy forces, now you can try unlocking a door to a nearby area", true);
        SaveData.LEVEL_COMPLETION[CURRENT_LEVEL]++;
    }

    public static void OnLoss()
    {
        _turnController.InterruptTurn();
        _guiController.CallDoAfterSystemMessage(ExitToMap,"Your forces can no longer keep up, retreating");
    }

    private static void ExitToMap()
    {
        SceneManager.LoadScene("Map");
    }
}