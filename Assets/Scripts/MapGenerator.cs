using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;

    [Header("Grid Values")]
    public int sizeX = 20;
    public int sizeY = 20;
    public Tile[,] grid;

    // Data struct for the Graph reprensation of the Grid
    private Dictionary<Tile, Tile[]> neighborDictionary;

    void Awake()
    {
        grid = new Tile[sizeX, sizeY];
        neighborDictionary = new Dictionary<Tile, Tile[]>();
        GenerateMap(sizeX, sizeY);
    }


    void GenerateMap(int sizeX, int sizeY)
    {
        // Generate Map
        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {
                grid[x, y] = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity).GetComponent<Tile>();
                grid[x, y].Init(x, y);
            }
        }

        // Build the Graph from the map
        for (int y = 0; y < sizeY; y++) {
            for (int x = 0; x < sizeX; x++) {

                // For the current tile create a nex list of it's neighbors and fill it with the 4 neighbors adjacents tiles when possible
                List<Tile> neighbors = new List<Tile>();
                if (y < sizeY - 1)
                    neighbors.Add(grid[x, y + 1]);
                if (x < sizeX - 1)
                    neighbors.Add(grid[x + 1, y]);
                if (y > 0)
                    neighbors.Add(grid[x, y - 1]);
                if (x > 0)
                    neighbors.Add(grid[x - 1, y]);

                neighborDictionary.Add(grid[x, y], neighbors.ToArray());
            }
        }
    }
    public Tile[] GetNeighbors(Tile tile) { return neighborDictionary[tile]; }
    public void ResetTiles()
    {
        // Set the tiles back to their default value
        foreach (Tile tile in grid){
            tile.SetColor(Color.white);
            tile.SetText("");
        }
    }
}