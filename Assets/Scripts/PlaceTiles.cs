using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTiles : MonoBehaviour
{
    public Tilemap map;
    public Tile tileWater;
    public Tile tileGrass;
    public Tile tileEdge;
    public List<Tile> tileSprites;
    public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    public int size;
    public float waterLevel;
    public float scale;

    Cell[,] grid;

    public void Start()
    {
        tiles.Add("1001", tileSprites[0]);
        tiles.Add("1000", tileSprites[1]);
        tiles.Add("1100", tileSprites[2]);
        tiles.Add("0001", tileSprites[3]);
        tiles.Add("0100", tileSprites[4]);
        tiles.Add("0101", tileSprites[5]);
        tiles.Add("0000", tileSprites[6]);
        tiles.Add("1111", tileSprites[7]);
        tiles.Add("0010", tileSprites[8]);
        tiles.Add("0011", tileSprites[9]);
        tiles.Add("0110", tileSprites[10]);

        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] falloffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                falloffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                cell.isWater = noiseValue < waterLevel;
                grid[x, y] = cell;
            }
        }
        drawTiles();
        cornerPass();
    }
    public void drawTiles()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile currentTile;
                if (grid[x,y].isWater == true)
                {
                    currentTile = tileWater;
                }
                else
                {
                   
                    try
                    {
                        currentTile = tiles[checkNextToWater(x, y)];
                        if(currentTile.name[currentTile.name.Length - 1].ToString() == "1")
                        {
                            grid[x, y].isBottomCorner = true;
                        }
                        if(checkNextToWater(x,y)[2].ToString() == "1")
                        {
                            grid[x, y].isBottomEdge = true;
                        }
                    }
                    catch
                    {
                        currentTile = tiles["1111"];
                    }

                }
                
                map.SetTile(new Vector3Int(x, y, 1), currentTile);
               

            }
        }
        
    }

    public void cornerPass()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (grid[x, y].isBottomCorner == false) { continue; }
                if (grid[x - 1, y].isWater == true)
                {
                    if(grid[x-1, y+1].isWater == true) { continue; }
                    map.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[12]);
                }
                if (grid[x + 1, y].isWater == true)
                {
                    if (grid[x + 1, y + 1].isWater == true) { continue; }
                    map.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[11]);
                }

            }
        }
    }
    public string checkNextToWater(int x, int y)
    {
        string edges = "";
        int xMod = 0;
        int yMod = 0;
        for (int i = 0; i < 4; i++)
        {
            if(i == 0) { xMod = 0; yMod = 1;  }
            else if (i == 1) { xMod = 1; yMod = 0; }
            else if (i == 2) { xMod = 0; yMod = -1; }
            else if (i == 3) { xMod = -1; yMod = 0; }
            if(grid[x + xMod, y + yMod].isWater == true)
            {
                edges += "1";
            }
            else
            {
                edges += "0";
            }
        }
        return edges;
    }
}
