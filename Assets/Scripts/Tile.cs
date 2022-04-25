using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Coordinates _coordinates;
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

}
