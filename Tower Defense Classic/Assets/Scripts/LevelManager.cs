using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private GameObject[] tiles;
    [SerializeField]
    private CameraMovement cm;
    private Point portal1Spawn, portal2Spawn;
    [SerializeField]
    private GameObject portal1, portal2;
    public Dictionary<Point,Tile> Tiles { get; set; }
    public float TileSizeX
    {
        get
        {
            return tiles[3].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }
    public float TileSizeY
    {
        get
        {
            return tiles[3].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        }
    }
    void Start()
    {
        CreateLevel();
        portal1.tag = "SpawnPortal";
    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, Tile>();
        string[] mapData = ReadLevelText(SceneManager.GetActiveScene().name);
        
        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;
        Vector3 maxTile= new Vector3(0,0,0);
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        for(int i=0;i<mapYSize;i++)
        {
            char[] newTiles = mapData[i].ToCharArray();
            for(int j=0;j<mapXSize;j++)
            {
                PlaceTile(newTiles[j].ToString(), i, j, worldStart);
            }
        }
        maxTile = Tiles[new Point(mapYSize -1, mapXSize -1)].transform.position;
        cm.SetLimits(new Vector3(maxTile.x+TileSizeX,maxTile.y-TileSizeY));
        SpawnPortal();
    }
    private void PlaceTile(string  tileType ,int i,int j,Vector3 WorldStart)
    {
        int tileIndex = int.Parse(tileType);
        Tile newTile = Instantiate(tiles[tileIndex],transform.position,Quaternion.identity,GameObject.FindGameObjectWithTag("Map").transform).GetComponent<Tile>();
        newTile.Setup(new Point(i, j), new Vector3(WorldStart.x + (TileSizeX * j), WorldStart.y - (TileSizeY * i), 0));
    }
    private string[] ReadLevelText(string LevelName)
    {
        if(LevelName=="Level1")
        {
        string[] arr = { "44441111111111111111", "44441111111111111111", "44441111111111111111", "44441114444411111111", "44441114111411111111", "14111444111411111111", "14111411144444111111", "14444411144444111111", "11111111144444111111", "11111111111111111111" };
        return arr;
        }
        else
        {
            string[] arr = { "14444411111111111111", "14444411111111111111", "14444411111111111111", "11141114444111111111", "11141114114111111111", "11141114114444411111", "11141114111144444111", "11144444111144444111", "11111111111144444111", "11111111111144444111" };
            return arr;
        }
        //TextAsset bindingData = Resources.Load(LevelName) as TextAsset;
        //string data = bindingData.text.Replace(Environment.NewLine, string.Empty);
    }
    private void SpawnPortal()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            portal1Spawn = new Point(1, 1);
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            portal1Spawn = new Point(1, 3);
        }
        Instantiate(portal1, Tiles[portal1Spawn].GetComponent<Tile>().WorldPosition, Quaternion.AngleAxis(180,new Vector3(0,1,0)));
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            portal2Spawn = new Point(6, 11);
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
         portal2Spawn = new Point(6 ,13);
        }
        Instantiate(portal2, Tiles[portal2Spawn].GetComponent<Tile>().WorldPosition, Quaternion.identity);
    }
}
