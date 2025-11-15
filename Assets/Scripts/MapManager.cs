using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // Scripts references
    public PathFindingAlgorithm pathFinding;
    public MapGenerator mapGenerator;

    // Path 
    private Tile startTile;
    private Tile endTile;

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

        // Check the player tile hovering (todo redo this comment)
        Tile tileUnderMouse = GetTileUnderMouse();

        // If the player is not hovering a tile or hovering the start or end tile, make an early return
        if (tileUnderMouse == null || tileUnderMouse == endTile || tileUnderMouse == startTile)
            return;

        // START (can't be placed on a wall)
        if (Input.GetMouseButtonDown(0) && tileUnderMouse.GetTileType() != TileType.Wall) 
            startTile = tileUnderMouse;

        // END (can't be placed on a wall)
        if (Input.GetMouseButtonDown(1) && tileUnderMouse.GetTileType() != TileType.Wall) 
            endTile = tileUnderMouse;
        
        // PLAINS
        if (Input.GetKey(KeyCode.Q))
            tileUnderMouse.SetTileType(TileType.Plains);
        
        // WOODS
        if (Input.GetKey(KeyCode.W))
            tileUnderMouse.SetTileType(TileType.Woods);

        // WALL
        if (Input.GetKey(KeyCode.E))
            tileUnderMouse.SetTileType(TileType.Wall);

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
