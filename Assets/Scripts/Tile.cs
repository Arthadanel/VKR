using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using DefaultNamespace;
using Units;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType _tileType;
    private Coordinates _coordinates;

    public Coordinates Coordinates
    {
        get => _coordinates;
        set => _coordinates = value;
    }
    private TileType _type;
    private bool _isObstruction;
    public bool IsOccupied { get; private set; }
    private Unit _currentUnit;

    public Tile(Coordinates coordinates, TileType type, bool isObstruction = false)
    {
        _coordinates = coordinates;
        _type = type;
        _isObstruction = isObstruction;
    }

    public void MoveTo(Unit unit)
    {
        if (IsOccupied)
        {
            bool fightSuccessful = unit.Fight(_currentUnit);
            if (fightSuccessful)
            {
                _currentUnit = unit;
                unit.Coordinates = _coordinates;
            }
            return;
        }
        IsOccupied = true;
        _currentUnit = unit;
        unit.Coordinates = _coordinates;
    }

    private void OnMouseEnter()
    {
        LevelMapControl.PositionSelector(_coordinates);
    }

    private void OnMouseDown()
    {
        if(_tileType!=TileType.GRASS)
            return;
        if (LevelMapControl.IsUnitSelected)
        {
            LevelMapControl.GetSelectedUnit().Coordinates = _coordinates;
        }
    }
}
