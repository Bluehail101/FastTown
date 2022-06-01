using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceTiles : MonoBehaviour
{
    public Tilemap level0;
    public Tilemap level1;
    public Tilemap trees;
    public Tilemap upperTrees;
    public Tilemap details;

    public Tile tileWater;

    public RuleTile groundTileLower;
    public RuleTile groundTileUpper;
    private RuleTile groundTile;

    public List<Tile> tileSprites;
    public List<Tile> forestTiles;
    public List<Tile> detailTiles;
    public List<Tile> rockTiles;

    public Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();

    public int size;
    public float waterLevel;
    public float level1Level;

    public float treeDensity;
    public float treeScale;

    public float grassDensity;
    public float grassScale;

    public int goldSparsity;

    public float scale;

    Cell[,] grid0;
    Cell[,] grid1;
    DetailCell[,] treeGrid;
    DetailCell[,] detailGrid;

    public void Start()
    {
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


        groundTile = groundTileLower;
        drawTiles(grid0, level0, true);
        groundTile = groundTileUpper;
        drawTiles(grid1, level1, false);
        //cornerPass(grid1, level1);
        //cleanUpPass(grid1, level1);
        //cleanUpPass2(grid1, level1);
        //treePass(falloffMap);
        //detailPass(details, falloffMap);
    }
    public void drawTiles(Cell[,] grid, Tilemap currentMap, bool spawnWater)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (grid[x,y].isWater == true)
                {
                    if(spawnWater == false) { continue; }
                    currentMap.SetTile(new Vector3Int(x, y, 1), tileWater);
                }
                else
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), groundTile);
                }               
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
    public void cleanUpPass2(Cell[,] grid, Tilemap currentMap)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if(currentMap.GetTile(new Vector3Int (x,y,1)) == tileSprites[6])
                {
                    if(checkNextToWater(grid1, x, y) == "1111")
                    {
                        currentMap.SetTile(new Vector3Int(x,y,1), null);
                        grid1[x, y].isWater = true;
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

    public void treePass(float[,] falloff)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * treeScale + xOffset, y * treeScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        treeGrid = new DetailCell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                DetailCell cell = new DetailCell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloff[x, y];
                float v = Random.Range(treeDensity / 2, treeDensity);
                if(noiseMap[x,y] < v)
                {
                    cell.isTree = true;
                }
                else
                {
                    cell.isTree = false;
                }
                if(grid0[x,y].isWater == true || grid0[x,y].isBottomEdge == true || grid1[x,y].isBottomEdge == true)
                {
                    cell.isTree = false;
                }
                if(grid1[x,y].isWater == false)
                {
                    cell.level = 1;
                }
                else
                {
                    cell.level = 0;
                }
                treeGrid[x, y] = cell;
            }
        }
        placeTrees(0, trees);
        placeTrees(1, upperTrees);
        
    }
    public void placeTrees(int level, Tilemap currentMap)
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (treeGrid[x, y].isTree == false) { continue; }
                if (treeGrid[x, y].level != level) { continue; }
                if ((treeGrid[x, y - 1].isTree == false || treeGrid[x, y - 1].level != level) && (treeGrid[x, y + 1].isTree == false || treeGrid[x, y + 1].level != level))
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), forestTiles[0]);
                    currentMap.SetTile(new Vector3Int(x, y + 1, 1), forestTiles[1]);
                    continue;
                }
                if (treeGrid[x, y - 1].isTree == false || treeGrid[x, y - 1].level != level)
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), forestTiles[0]);
                    continue;
                }
                if (treeGrid[x, y + 1].isTree == false || treeGrid[x, y + 1].level != level)
                {
                    currentMap.SetTile(new Vector3Int(x, y + 1, 1), forestTiles[1]);
                }
                int rnd = Random.Range(1, 4);
                if (rnd == 1)
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), forestTiles[2]);
                }
                else
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), forestTiles[3]);
                }
            }
        }
    }
    public void detailPass(Tilemap currentMap, float[,] falloff)
    {
        float[,] noiseMap = new float[size, size];
        (float xOffset, float yOffset) = (Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * grassScale + xOffset, y * grassScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }
        detailGrid = new DetailCell[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                DetailCell cell = new DetailCell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= falloff[x, y];
                float v = Random.Range(grassDensity / 2, grassDensity);
                if (noiseMap[x, y] < v)
                {
                    cell.isGrass = true;
                }
                else
                {
                    cell.isGrass = false;
                }
                if (grid0[x, y].isWater == true || grid0[x, y].isBottomEdge == true || grid1[x, y].isBottomEdge == true)
                {
                    cell.isGrass = false;
                }
                
                detailGrid[x, y] = cell;
            }
        }
        int rockcounter = 0;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (detailGrid[x, y].isGrass == false) { continue; }
                int rnd = Random.Range(1, 8);
                if (rnd == 1)
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), detailTiles[1]);
                }
                else if (rnd == 2 && treeGrid[x,y].isTree == false)
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), rockTiles[Random.Range(0, rockTiles.Count - 1 )]);
                    detailGrid[x, y].isRock = true;
                    rockcounter += 1;
                    if (rockcounter != goldSparsity) { continue; }
                    rockcounter = 0;
                    currentMap.SetTile(new Vector3Int(x, y, 1), rockTiles[4]);
                    detailGrid[x, y].isGold = true;
                }
                else
                {
                    currentMap.SetTile(new Vector3Int(x, y, 1), detailTiles[0]);
                }
            }
        }
    }
}
