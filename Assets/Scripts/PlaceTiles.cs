using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTiles : MonoBehaviour
{
    public Tilemap level0;
    public Tilemap level1;
    public Tilemap level2;
    public Tile tileGrass;
    public Tile tileEdge;
    public List<Tile> tileSprites;
    public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    public int size;
    public float waterLevel;
    public float level1Level;
    public float scale;

    Cell[,] grid0;
    Cell[,] grid1;

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
        tiles.Add("1010", tileSprites[10]);
        tiles.Add("1101", tileSprites[14]);
        tiles.Add("1110", tileSprites[15]);
        tiles.Add("1011", tileSprites[16]);
        tiles.Add("0111", tileSprites[17]);

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

        grid0 = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                cell.isWater = noiseValue < waterLevel;
                grid0[x, y] = cell;
            }
        }

        grid1 = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloffMap[x, y];
                cell.isWater = noiseValue < level1Level;
                grid1[x, y] = cell;
            }
        }



        drawTiles(grid0, level0, true);
        cornerPass(grid0, level0);
        cleanUpPass(grid0, level0);
        tiles["0010"] = tileSprites[22];
        tiles["0011"] = tileSprites[21];
        tiles["0110"] = tileSprites[24];
        tiles["0111"] = tileSprites[23];
        tiles["0111"] = tileSprites[23];
        tiles["1111"] = tileSprites[6];
        drawTiles(grid1, level1, false);
        cornerPass(grid1, level1);
        cleanUpPass(grid1, level1);
    }
    public void drawTiles(Cell[,] grid, Tilemap currentMap, bool spawnWater)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Tile currentTile;
                if (grid[x,y].isWater == true)
                {
                    if(spawnWater == true)
                    {
                        currentTile = tiles["1111"];
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                   
                    try
                    {
                        string tileString = checkNextToWater(grid, x, y);
                        currentTile = tiles[tileString];
                        if(currentTile.name[currentTile.name.Length - 1].ToString() == "1")
                        {
                            grid[x, y].isBottomCorner = true;
                        }
                        if(tileString[2].ToString() == "1")
                        {
                            grid[x, y].isBottomEdge = true;
                        }
                        if(tileString == "1011" || tileString == "1110")
                        {
                            grid[x, y].isEndPiece = true;
                        }
                        if (tileString == "0111")
                        {
                            grid[x, y].isBottomEndPiece = true;
                        }
                    }
                    catch
                    {
                        currentTile = tiles["1111"];
                    }

                }
                currentMap.SetTile(new Vector3Int(x, y, 1), currentTile);
               

            }
        }
        
    }

    public void cornerPass(Cell[,] grid, Tilemap currentMap)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if(grid[x, y].isEndPiece == true) { continue; }
                if(grid[x, y].isBottomCorner == false && grid[x, y].isBottomEdge == true)
                {
                    if (grid[x - 1, y].isWater == false)
                    {
                        if (grid[x - 1, y - 1].isWater == false) { currentMap.SetTile(new Vector3Int(x - 1, y, 1), tileSprites[11]); }
                    }
                    if (grid[x + 1, y].isWater == false)
                    {
                        if (grid[x + 1, y - 1].isWater == false) { currentMap.SetTile(new Vector3Int(x + 1, y, 1), tileSprites[12]); }
                    }
                }


                if (grid[x, y].isBottomCorner == false) { continue; }

                if (grid[x - 1, y].isWater == true)
                {
                    if(grid[x - 1, y + 1].isWater == false) { currentMap.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[12]); }                  
                }
                else
                {
                    if (grid[x - 1, y - 1].isWater == false) { currentMap.SetTile(new Vector3Int(x - 1, y, 1), tileSprites[11]); }
                }
                if (grid[x + 1, y].isWater == true)
                {
                    if (grid[x + 1, y + 1].isWater == false) { currentMap.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[11]); }
                }
                else
                {
                    if (grid[x + 1, y - 1].isWater == false) { currentMap.SetTile(new Vector3Int(x + 1, y, 1), tileSprites[12]); }
                }

            }
        }
    }

    public void cleanUpPass(Cell[,] grid, Tilemap currentMap)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if(grid[x, y].isBottomEndPiece == true)
                {
                    if (grid[x - 1, y + 1].isWater == false && grid[x + 1, y + 1].isWater == false)
                    {
                        currentMap.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[18]);
                        continue;
                    }
                    if (grid[x - 1, y + 1].isWater == true && grid[x + 1, y + 1].isWater == false)
                    {
                        currentMap.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[19]);
                        continue;
                    }
                    if (grid[x - 1, y + 1].isWater == false && grid[x + 1, y + 1].isWater == true)
                    {
                        currentMap.SetTile(new Vector3Int(x, y + 1, 1), tileSprites[20]);
                        continue;
                    }
                }
                if(grid[x,y].isEndPiece == false) { continue; }
                if(grid[x - 1,y].isWater == true)
                {
                    if(tiles["1111"] == tileSprites[6])
                    {
                        currentMap.SetTile(new Vector3Int(x, y, 1), null);
                    }
                    else
                    {
                        currentMap.SetTile(new Vector3Int(x, y, 1), tiles["1111"]);
                    }
                    grid[x, y].isWater = true;
                    if(grid[x + 1, y + 1].isWater == true)
                    {
                        currentMap.SetTile(new Vector3Int(x + 1, y, 1), tileSprites[0]);
                    }
                    else
                    {
                        currentMap.SetTile(new Vector3Int(x + 1, y, 1), tileSprites[3]);
                    }
                }
                else
                {
                    if (tiles["1111"] == tileSprites[6])
                    {
                        currentMap.SetTile(new Vector3Int(x, y, 1), null);
                    }
                    else
                    {
                        currentMap.SetTile(new Vector3Int(x, y, 1), tiles["1111"]);
                    }
                    grid[x, y].isWater = true;
                    if (grid[x - 1, y + 1].isWater == true)
                    {
                        currentMap.SetTile(new Vector3Int(x - 1, y, 1), tileSprites[2]);
                    }
                    else
                    {
                        currentMap.SetTile(new Vector3Int(x - 1, y, 1), tileSprites[4]);
                    }
                }
            }
        }
    }
    public string checkNextToWater(Cell[,] grid, int x, int y)
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
