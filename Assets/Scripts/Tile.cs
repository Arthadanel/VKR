using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Units;
using UnityEngine;
using Utility;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileType type = TileType.GRASS;
    [SerializeField] private bool isObstruction = false;
    [SerializeField] private int movementCost = 1;
    [SerializeField] private int attackHindrance = 0;
    
    private Coordinates _coordinates;
    private GameObject _highlighter;

    public Coordinates Coordinates
    {
        get => _coordinates;
        set => _coordinates = value;
    }
    private TileType _tileType;
    private bool _isObstruction;
    public bool IsOccupied { get; private set; }
    private int _movementCost;

    public int GetMovementCost()
    {
        return isObstruction || IsOccupied ? GameSettings.MOVE_LIMIT : _movementCost;
    }
    private int _attackHindrance;
    public int GetAttackHindrance()
    {
        return _attackHindrance;
    }

    private Unit _currentUnit;

    public void InitializeTile(Coordinates coordinates, TileType type, bool isObstruction = false)
    {
        _coordinates = coordinates;
        _tileType = type;
        //todo: set move cost based on tile type
        _movementCost = 1;
        _isObstruction = isObstruction;
        //todo: set move cost based on tile type and obstruction property
        _attackHindrance = 0;
    }
    public void InitializeTilePrefab(Coordinates coordinates)
    {
        _coordinates = coordinates;
        _tileType = type;
        _isObstruction = isObstruction;
        _movementCost = movementCost;
        _attackHindrance = attackHindrance;
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

    public void AddHighlighter(GameObject highlighter)
    {
        var position = transform.position;
        _highlighter = Instantiate(highlighter, new Vector3(position.x,position.y,-1), Quaternion.identity, transform);
    }
    
    public void ClearHighlighter()
    {
        Destroy(_highlighter);
        _highlighter = null;
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
            if (_highlighter is null) return;
            LevelMapControl.DeactivateMoveReachHighlight();
            LevelMapControl.GetSelectedUnit().Coordinates = _coordinates;
        }
    }
}
