using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utility;

public class LevelMap : MonoBehaviour
{
    [SerializeField] GameObject tileBorder;
    [SerializeField] GameObject tileGrass;
    [SerializeField] GameObject tileWall;
    [SerializeField] GameObject tileCrate;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject ally;
    [SerializeField] private GameObject healer;

    public static int BorderLayer = 0;
    public static int TileLayer = 1;
    public static int UnitLayer = -3;

    private List<GameObject> borders = new List<GameObject>();

    public static int CURRENT_LEVEL = 1;
    private List<Tile> _map;
    
    public void SetBasicLayout()
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

        bool[,] layout = new bool[lengthX, lengthY];
        bool[,] filledLayout = new bool[lengthX, lengthY];

        //generate outline
        int vertexCount = adjustedConvexHull.Count;
        for (int i = 0; i < vertexCount; i++)
        {
            LineBresenham(adjustedConvexHull[i], adjustedConvexHull[(i + 1)%vertexCount], layout);
        }
        
        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 0; y < lengthY; y++)
            {
                filledLayout[x,y] = layout[x,y];
            }
        }

        FillOutline(filledLayout, lengthX, lengthY);
        
        //todo: fix it properly
        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 0; y < lengthY; y++)
            {
                if (layout[x, y])
                    filledLayout[x, y] = true;
            }
        }

        for (int x = 0; x < lengthX; x++)
        {
            for (int y = 1; y < lengthY; y++)
            {
                if(!filledLayout[x,y])continue;
                GameObject tmp = Instantiate(tileGrass, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform);
                tmp.GetComponent<Tile>().Coordinates = new Coordinates(x, y);
            }
        }
    }

    private bool[,] FillOutline(bool[,] layout, int lengthX, int lengthY)
    {
        for (int x = 0; x < lengthX; x++)
        {
            int enteredBorderCounter = 0;
            bool inPolygon = false;
            bool previous = false;
            bool lastWasChanged = false;
            int lastBorderFlag = -1;
            for (int y = 0; y < lengthY; y++)
            {
                bool current = layout[x, y];
                
                if (current != previous)
                {
                    if (current)
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

                layout[x, y] = inPolygon;
                previous = inPolygon;

                if (current && previous && lastWasChanged)
                {
                    enteredBorderCounter++;
                    inPolygon = false;
                }

                lastWasChanged = current != layout[x, y];
                
                if (y == lengthY - 1 && inPolygon && enteredBorderCounter % 2 == 1 && lastWasChanged)
                {
                    for (int i = y; i > lastBorderFlag; i--)
                    {
                        layout[x, i] = false;
                    }
                }
            }
        }

        return layout;
    }
    
    void LineBresenham(Point point1, Point point2, bool[,] layout)
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
            layout[x, y] = true;

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

    public void GenerateInnerLayout()
    {
        
    }

    public void PlaceUnits()
    {
        
    }

    public void SwitchBorderVisibility()
    {
        foreach (GameObject border in borders)
        {
            border.SetActive(!border.activeSelf);
        }
    }

    private void Start()
    {
        SetBasicLayout();
    }
}
