using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Scripts references
    [Header("Script References")]
    public PathFindingAlgorithm pathFinding;
    public MapGenerator mapGenerator;

    // Path 
    private Tile startTile;
    private Tile endTile;

    // Traveler ref
    public GameObject travelerPrefab;
    private Traveler traveler;


    private void Update() { HandleInput(); }

    private void CalculatePath()
    {
        // Call the path finding algo
        Queue<Tile> path = pathFinding.FindPath(startTile, endTile);
        
        // Change the color of the tiles on the paths (excluding the start and end tile)
        if (path != null)
            foreach (Tile tile in path)
                if (tile != endTile && tile != startTile)
                    tile.SetColor(Color.white);
                
    }

    private void HandleInput()
    {
        // ===== ALGORITHM/TRAVELER MODIFICATIONS =====
        if (Input.GetKey(KeyCode.Q))
        {
            pathFinding.SetAlgorithm(Algorithm.AStar);
            RepaintMap();
        }

        if (Input.GetKey(KeyCode.D))
        {
            pathFinding.SetAlgorithm(Algorithm.Dijkstra);
            RepaintMap();
        }

        if (startTile != null && endTile != null && Input.GetKey(KeyCode.F))
        {
            // If the travaler has not been instantiated yet, instantiate it on the starting tile, else just place it on the starting tile
            if (traveler == null)
                traveler = Instantiate(travelerPrefab, startTile.transform.position, Quaternion.identity).GetComponent<Traveler>(); //TODO Fix the traveler half in the ground
            else
                traveler.transform.position = traveler.transform.position;

            // Compute the path and set it as the traveler's path
            Queue<Tile> path = pathFinding.FindPath(startTile, endTile);
            traveler.SetPath(path);
        }

        // ===== TILES MODIFICATION =====
        // Check the player tile hovering (todo redo this comment)
        Tile tileUnderMouse = GetTileUnderMouse();

        // If the player is not hovering a tile or hovering the start or end tile, make an early return
        if (tileUnderMouse == null || tileUnderMouse == endTile || tileUnderMouse == startTile)
            return;

        // START
        if (Input.GetMouseButtonDown(0) && CanPlace(tileUnderMouse))
            startTile = tileUnderMouse;

        // END
        if (Input.GetMouseButtonDown(1) && CanPlace(tileUnderMouse))
            endTile = tileUnderMouse;
        
        // PLAINS
        if (Input.GetKey(KeyCode.A))
            tileUnderMouse.SetTileType(TileType.Plains);
        
        // WOODS
        if (Input.GetKey(KeyCode.W))
            tileUnderMouse.SetTileType(TileType.Woods);

        // WALL
        if (Input.GetKey(KeyCode.E))
            tileUnderMouse.SetTileType(TileType.Wall);

        // ROAD
        if (Input.GetKey(KeyCode.R))
            tileUnderMouse.SetTileType(TileType.Road);

        // RIVER
        if (Input.GetKey(KeyCode.T))
            tileUnderMouse.SetTileType(TileType.River);

        RepaintMap();
    }

    private Tile GetTileUnderMouse()
    {
        // Raycast under the mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        bool wasHit = Physics.Raycast(ray, out rayHit, int.MaxValue, LayerMask.GetMask("Tiles"));

        // Return the hit tile or null if no hit
        if (wasHit)
            return rayHit.transform.GetComponent<Tile>();
        else
            return null;
    }

    private bool CanPlace(Tile tile)
    {
        // START and END can only be placed on plains
        return tile.GetTileType() == TileType.Plains;
    }

    public void RepaintMap()
    {
        // Reset all the tiles
        mapGenerator.ResetTiles();

        // End Tile
        if (endTile != null) {
            endTile.SetColor(Color.red);
            endTile.SetText("END");
        }

        // Start Tile
        if (startTile != null){
            startTile.SetColor(Color.green);
            startTile.SetText("START");
        }

        // If there's both a start and end tile, launch the path finding
        if (startTile != null && endTile != null)
            CalculatePath();
    }
}
