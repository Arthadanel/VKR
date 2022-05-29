using Utility;

namespace Level
{
    public struct LayoutTile
    {
        public int TileType;
        public Edge TransitionEdge;

        public LayoutTile(int type)
        {
            TileType = type;
            TransitionEdge = null;
        }
    }
}