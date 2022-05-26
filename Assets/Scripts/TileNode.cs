using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

public class TileNode
{
    private readonly Tile _tile;
    private TileNode _left;
    private TileNode _top;
    private TileNode _right;
    private TileNode _bottom;

    public TileNode(Tile tile)
    {
        _tile = tile;
    }
    public void SetNeighbourTileNodes(TileNode left, TileNode top, TileNode right, TileNode bottom)
    {
        _left = left;
        _top = top;
        _right = right;
        _bottom = bottom;
    }

    public List<TileNode> GetTilesInRange(int maxCost, bool ignoreMovementCost = false,
        bool ignoreAttackHindrance = true)
    {
        List<TileNode> result = new List<TileNode>();

        if (_left != null)
        {
            List<TileNode> left = _left.GetTilesInRangeRecursive(maxCost, 0, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(left);
        }
        if (_top != null)
        {
            List<TileNode> top = _top.GetTilesInRangeRecursive(maxCost, 0, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(top);
        }
        if (_right != null)
        {
            List<TileNode> right = _right.GetTilesInRangeRecursive(maxCost, 0, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(right);
        }
        if (_bottom != null)
        {
            List<TileNode> bottom = _bottom.GetTilesInRangeRecursive(maxCost, 0, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(bottom);
        }

        result = result.Distinct().ToList();

        return result;
    }

    private List<TileNode> GetTilesInRangeRecursive(int maxCost, int previousCost, bool ignoreMovementCost = false,
        bool ignoreAttackHindrance = true)
    {
        List<TileNode> result = new List<TileNode>();

        int actionCost = (ignoreMovementCost ? 1 : _tile.GetMovementCost());
        int attackHindrance = ignoreAttackHindrance ? 0 : +_tile.GetAttackHindrance();
        int cost = previousCost + actionCost;

        if (cost > maxCost)
            return result;

        //assign tile interaction costs
        if(!ignoreMovementCost)
        {
            if (_tile.TileInteractionCost == 0 || _tile.TileInteractionCost > cost)
                _tile.TileInteractionCost = cost;
        }
        else
        {
            _tile.TileInteractionCost = 1;
        }

        result.Add(this);
        
        if (_left != null)
        {
            List<TileNode> left =
                _left.GetTilesInRangeRecursive(maxCost, cost+attackHindrance, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(left);
        }
        if (_top != null)
        {
            List<TileNode> top =
                _top.GetTilesInRangeRecursive(maxCost, cost+attackHindrance, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(top);
        }
        if (_right != null)
        {
            List<TileNode> right =
                _right.GetTilesInRangeRecursive(maxCost, cost+attackHindrance, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(right);
        }
        if (_bottom != null)
        {
            List<TileNode> bottom =
                _bottom.GetTilesInRangeRecursive(maxCost, cost+attackHindrance, ignoreMovementCost, ignoreAttackHindrance);
            result.AddRange(bottom);
        }

        return result;
    }

    public Tile GetTileData()
    {
        return _tile;
    }
}