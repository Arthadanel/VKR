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
    
    public void SetBasicLayout(List<Point> anchorPoints)
    {
        for (int i = 0; i < anchorPoints.Count; i++)
        {
            
        }
    }

    public void GenerateInnerLayout()
    {
        int width = 10;
        int height = 8;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int x = i - width / 2;
                int y = j - height / 2;
                if (i % 3 == 0 && j % 4 == 0)
                {
                    GameObject tmp = Instantiate(tileCrate, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform);
                    tmp.GetComponent<Tile>().Coordinates = new Coordinates(x, y);
                }
                else if (i == 0 || j == 0)
                {
                    GameObject tmp =Instantiate(tileWall, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform);
                    tmp.GetComponent<Tile>().Coordinates = new Coordinates(x, y);
                }
                else
                {
                    GameObject tmp = Instantiate(tileGrass, new Vector3(x, y, TileLayer), Quaternion.identity, gameObject.transform);
                    tmp.GetComponent<Tile>().Coordinates = new Coordinates(x, y);
                }
                borders.Add(Instantiate(tileBorder, new Vector3(x, y, BorderLayer), Quaternion.identity, gameObject.transform));
            }
        }
        PlaceUnits();
    }

    public void PlaceUnits()
    {
        GameObject tmp =Instantiate(enemy, new Vector3(2, 0, UnitLayer), Quaternion.identity, gameObject.transform);
        tmp.GetComponent<Unit>().SetInitialCoordinates(2,0);
        tmp = Instantiate(ally, new Vector3(-3, -1, UnitLayer), Quaternion.identity, gameObject.transform);
        tmp.GetComponent<Unit>().SetInitialCoordinates(-3,-1);
        tmp = Instantiate(healer, new Vector3(-4, -3, UnitLayer), Quaternion.identity, gameObject.transform);
        tmp.GetComponent<Unit>().SetInitialCoordinates(-4,-3);
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
        GenerateInnerLayout();
    }
}
