using System;
using System.Collections.Generic;
using Data;
using Tiles;
using Units;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelMap : MonoBehaviour
    {
        [SerializeField] private GameObject borderPrefab;
        [SerializeField] private GameObject transition;
        [SerializeField] List<GameObject> floors;
        [SerializeField] List<GameObject> walls;
        [SerializeField] List<GameObject> destructibles;
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private List<GameObject> allies;

        public static int BorderLayer = 0;
        public static int TileLayer = 1;
        public static int UnitLayer = -3;
        
        private TileNode[,] _map;

        public Action OnMapGenerationFinished;
        private List<GameObject> borders = new List<GameObject>();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<Ally> _allies = new List<Ally>();
    
        private int _layoutSize;

        private void Start()
        {
            //Pass actions
            LevelController.GetEnemyList = GetEnemyList;
            LevelController.GetAllyList = GetAllyList;
        
            GenerateLevel();
            SpawnUnits();
        
            BuildGraph();

            LevelController.SetLevelLayout(_map);
            if (OnMapGenerationFinished != null) OnMapGenerationFinished();
        }

        private void GenerateLevel()
        {
            LayoutTile[,] basicLayout = SetBasicLayout();
            //basicLayout = LayoutScale4(basicLayout);
            int lengthX = basicLayout.GetLength(0);
            int lengthY = basicLayout.GetLength(1);
            _map = new TileNode[lengthX,lengthY];

            _layoutSize = GenerateInnerLayout(lengthX, lengthY, basicLayout);
        
            LevelController.SetTileCount(_layoutSize);
        }

        private int GenerateInnerLayout(int lengthX, int lengthY, LayoutTile[,] basicLayout)
        {
            HashSet<int> spawnedConnections = new HashSet<int>();
            int tileCount = 0;
            int arraySize = lengthX * lengthY;

            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    var value = basicLayout[x, y];
                    if (value.TileType == 0) continue;
                    tileCount++;
                    Tile tile = null;
                
                    //spawn outer wall
                    if (value.TileType == 2 && value.TransitionEdge == null)
                        tile = SpawnTile(x, y, walls[0]);
                    else if (value.TileType == 2)
                    {
                        int level = value.TransitionEdge.GetOtherAdjPolygon(SelectedLevelData.GetLevelPolygon())
                            .LevelNumber;
                        if (spawnedConnections.Add(level))
                        {
                            tile = SpawnTransitionTile(x, y, level, transition);
                        }
                    }
                    else
                    {
                        int check = Random.Range(0, arraySize);
                        if (check < arraySize / 25)
                            tile = SpawnTile(x, y, destructibles[0]);
                    }
                
                    tile ??= SpawnTile(x, y, floors[0]);
                
                    _map[x, y] = new TileNode(tile);
                }
            }

            return tileCount;
        }

        private void SpawnUnits()
        {
            int ap = _layoutSize / (SaveData.IS_DIFFICULT
                ? GameSettings.TILE_AP_PERCENTAGE_HARD
                : GameSettings.TILE_AP_PERCENTAGE);
            int maxAllies = ap / 3;
            int maxEnemies = (int) (ap / 3 * (SaveData.IS_DIFFICULT
                ? GameSettings.ENEMY_UNIT_MULTIPLIER_HARD
                : GameSettings.ENEMY_UNIT_MULTIPLIER));
            int lengthX = _map.GetLength(0);
            int lengthY = _map.GetLength(1);

            int chanceThreshold = 3;
            int chanceA = chanceThreshold;
            int chanceE = _layoutSize + chanceThreshold;

            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    if (_map[x, y] == null) continue;
                    Tile tile = _map[x, y].GetTileData();
                    if (tile.IsObstruction() || tile.IsOccupied) continue;
                    if(_allies.Count<maxAllies)
                    {
                        int check = Random.Range(0, 1 + chanceA++);
                        if (check <= chanceThreshold)
                        {
                            _allies.Add(SpawnUnit(tile,allies[Random.Range(0,allies.Count)]) as Ally);
                        }
                    }
                    else if (_allies.Count > maxAllies / 2 && _enemies.Count < maxEnemies)
                    {
                        int check = Random.Range(0, chanceThreshold + chanceE--);
                        if (check <= chanceThreshold * GameSettings.ENEMY_UNIT_MULTIPLIER)
                        {
                            _enemies.Add(SpawnUnit(tile, enemies[Random.Range(0, enemies.Count)]) as Enemy);
                        }
                    }
                }
            }
        }

        private void BuildGraph()
        {
            int lengthX = _map.GetLength(0);
            int lengthY = _map.GetLength(1);

            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    if (_map[x, y] == null) continue;
                    TileNode left = x == 0 ? null : _map[x - 1, y];
                    TileNode top = y == 0 ? null : _map[x, y - 1];
                    TileNode right = x == lengthX - 1 ? null : _map[x + 1, y];
                    TileNode bottom = y == lengthY - 1 ? null : _map[x, y + 1];
                    _map[x, y].SetNeighbourTileNodes(left, top, right, bottom);
                }
            }
        }

        private Unit SpawnUnit(Tile tile, GameObject prefab)
        {
            Unit unit = Instantiate(prefab, new Vector3(tile.Coordinates.X, tile.Coordinates.Y, UnitLayer), Quaternion.identity, gameObject.transform)
                .GetComponent<Unit>();
            tile.PlaceUnit(unit);
            return unit;
        }
        private Tile SpawnTile(int x, int y, GameObject prefab)
        {
            Tile tile = Instantiate(prefab, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform).GetComponent<Tile>();
            tile.InitializeTilePrefab(new Coordinates(x, y));
            borders.Add(Instantiate(borderPrefab, new Vector3(x, y, BorderLayer), Quaternion.identity, gameObject.transform));
            return tile;
        }
        private Tile SpawnTransitionTile(int x, int y, int connectedLevel, GameObject prefab)
        {
            TransitionTile tile = Instantiate(prefab, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform).GetComponent<TransitionTile>();
            tile.InitializeTilePrefab(new Coordinates(x, y),connectedLevel);
            borders.Add(Instantiate(borderPrefab, new Vector3(x, y, BorderLayer), Quaternion.identity, gameObject.transform));
            return tile;
        }

        //scale layout so each single tile equals occupies 4 tiles
        private T[,] LayoutScale4<T>(T[,] original)
        {
            const int n = 2;
            int lengthX = original.GetLength(0);
            int lengthY = original.GetLength(1);
            T[,] scaled = new T[lengthX * n, lengthY * n];
            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    T value = original[x, y];
                    scaled[x * n, y * n] = value;
                    scaled[x * n, y * n + 1] = value;
                    scaled[x * n + 1, y * n] = value;
                    scaled[x * n + 1, y * n + 1] = value;
                }
            }

            return scaled;
        }

        public Vector3 GetCentralPoint()
        {
            int maxX = _map.GetLength(0);
            int maxY = _map.GetLength(1);
            Vector3 result = new Vector3(maxX/2f,maxY/2f, -10);
            return result;
        }

        private List<Enemy> GetEnemyList()
        {
            return _enemies;
        }
        private List<Ally> GetAllyList()
        {
            return _allies;
        }

        public void SwitchBorderVisibility()
        {
            foreach (GameObject border in borders)
            {
                border.SetActive(!border.activeSelf);
            }
        }

        private LayoutTile[,] SetBasicLayout()
        {
            List<Point> convexHull = SelectedLevelData.GetConvexHull();
            float minX = convexHull[0].X;   //x padding
            float maxX = convexHull[0].X;
            float minY = convexHull[0].Y;   //y padding
            float maxY = convexHull[0].Y;
            foreach (var point in convexHull)
            {
                if (point.X < minX) minX = point.X;
                if (point.X > maxX) maxX = point.X;
                if (point.Y < minY) minY = point.Y;
                if (point.Y > maxY) maxY = point.Y;
            }

            int paddingX = 0 - (int) minX;
            int paddingY = 0 - (int) minY;

            List<Point> adjustedConvexHull = new List<Point>();

            foreach (var point in convexHull)
            {
                int x = (int)point.X + paddingX + 1;
                int y = (int) point.Y + paddingY + 1;
                Point adjustedPoint = new Point(x,y);
                adjustedConvexHull.Add(adjustedPoint);
            }
        
            int lengthX = (int) maxX + paddingX + 2;
            int lengthY = (int) maxY + paddingY + 2;
        
            //============================

            LayoutTile[,] layout = new LayoutTile[lengthX, lengthY];
            LayoutTile[,] filledLayout = new LayoutTile[lengthX, lengthY];

            //generate outline
            int vertexCount = adjustedConvexHull.Count;
            for (int i = 0; i < vertexCount; i++)
            {
                int index1 = i;
                int index2 = (i + 1) % vertexCount;
                Edge edge = SelectedLevelData.GetEdge(convexHull[index1], convexHull[index2]);
                if (edge.GetAdjacentPolygonCount() < 2) edge = null;
                LineBresenham(adjustedConvexHull[index1], adjustedConvexHull[index2], layout, edge);
            }
        
            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    filledLayout[x, y] = layout[x,y];
                }
            }

            filledLayout = FillOutline(filledLayout, lengthX, lengthY);

            for (int x = 0; x < lengthX; x++)
            {
                for (int y = 0; y < lengthY; y++)
                {
                    if (layout[x, y].TileType > 0)
                        filledLayout[x, y] = layout[x, y];
                    else if (filledLayout[x, y].TileType > 0)
                    {
                        filledLayout[x, y].TileType = 1;
                    }
                }
            }

            return filledLayout;
        }

        private LayoutTile[,] FillOutline(LayoutTile[,] layout, int lengthX, int lengthY)
        {
            for (int x = 0; x < lengthX; x++)
            {
                int enteredBorderCounter = 0;
                bool inPolygon = false;
                LayoutTile previous = new LayoutTile(0);
                bool lastWasChanged = false;
                int lastBorderFlag = -1;
                for (int y = 0; y < lengthY; y++)
                {
                    LayoutTile current = layout[x, y];
                
                    if (current.TileType != previous.TileType)
                    {
                        if (current.TileType >0)
                        {
                            enteredBorderCounter++;
                            lastBorderFlag = y;
                            inPolygon = true;
                        }
                        else
                        {
                            inPolygon = enteredBorderCounter % 2 == 1;
                            if (!lastWasChanged)
                                lastBorderFlag = y-1;
                        }
                    }

                    layout[x, y] = inPolygon ? new LayoutTile(2) : new LayoutTile(0);
                    previous = inPolygon ?new LayoutTile(2) : new LayoutTile(0);

                    if (current.TileType >0 && previous.TileType >0 && lastWasChanged)
                    {
                        enteredBorderCounter++;
                        inPolygon = false;
                    }

                    lastWasChanged = current.TileType != layout[x, y].TileType;
                
                    if (y == lengthY - 1 && inPolygon && enteredBorderCounter % 2 == 1 && lastWasChanged)
                    {
                        for (int i = y; i > lastBorderFlag; i--)
                        {
                            layout[x, i].TileType = 0;
                        }
                    }
                }
            }

            return layout;
        }
    
        void LineBresenham(Point point1, Point point2, LayoutTile[,] layout, Edge edge)
        {
            int x = (int) point1.X;
            int y = (int) point1.Y;
            int sX = (point2.X - point1.X) > 0 ? 1 : -1;
            int sY = (point2.Y - point1.Y) > 0 ? 1 : -1;
            int deltaX = sX * ((int)point2.X - (int)point1.X);
            int deltaY = sY * ((int)point2.Y - (int)point1.Y);
            bool swapped = false;

            if (deltaX < deltaY) 
            {
                (deltaX, deltaY) = (deltaY, deltaX);
                swapped = true;
            }

            int e = 2 * deltaY - deltaX;
            int i = 1;

            while (i <= deltaX)
            {
                layout[x, y] = new LayoutTile(2)
                {
                    TransitionEdge = edge
                };

                while (e >= 0)
                {
                    if (swapped)
                        x += sX;
                    else
                        y += sY;
                    e -= 2 * deltaX;
                }

                if (swapped)
                    y += sY;
                else
                    x += sX;

                e += 2 * deltaY;
                i++;
            }
        }
    }
}
