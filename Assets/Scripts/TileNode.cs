using System.Collections.Generic;
using UnityEngine;

public class TileNode
{
    private Tile _tile;
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

    public List<TileNode> GetTilesInRange(int maxCost, bool ignoreMovementCost = false, bool ignoreAttackHindrance = true)
    {
        List<TileNode> result = new List<TileNode>();

        int rangeCost = (ignoreMovementCost ? 1 : _tile.GetMovementCost()) + (ignoreAttackHindrance ? 0 : _tile.GetAttackHindrance());
        int leftoverCost = maxCost - rangeCost;
        //Debug.Log(rangeCost);
        if (leftoverCost > 0)
        {
            result.Add(this);
            if (_left != null)
                result.AddRange(_left.GetTilesInRange(leftoverCost, ignoreMovementCost, ignoreAttackHindrance));
            if (_top != null)
                result.AddRange(_top.GetTilesInRange(leftoverCost, ignoreMovementCost, ignoreAttackHindrance));
            if (_right != null)
                result.AddRange(_right.GetTilesInRange(leftoverCost, ignoreMovementCost, ignoreAttackHindrance));
            if (_bottom != null)
                result.AddRange(_bottom.GetTilesInRange(leftoverCost, ignoreMovementCost, ignoreAttackHindrance));
        }
        if(leftoverCost == 0)
            result.Add(this);

        return result;
    }


    public Tile GetTileData()
    {
        return _tile;
    }
}