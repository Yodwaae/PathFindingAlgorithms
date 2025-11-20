using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add A security not moving start (at least) when the traveler is spawned
// TODO Add despawn the traveler when it reches the end tile

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


    private void Update()
    { 
        // Handle the Inputs
        bool mapChanged = HandleMapInput();
        bool travelerChanged = HandleTravelerInput();

        // If there was a change repaint the map
        if (mapChanged || travelerChanged)
            RepaintMap();

    }

    private void CalculatePath()
    {
        // NOTE : The tile under the player should NEVER be null
        // If the player is already traveling, dynamically change the starting point to the pile at it's position
        if (traveler != null && traveler.isTraveling) {

            // Creates the rays to check the tile under the player
            Ray ray = new Ray(traveler.transform.position, Vector3.down * 100); 
            RaycastHit rayHit;

            // Get the tile under the player and set it as the new starting point
            bool wasHit = Physics.Raycast(ray, out rayHit, int.MaxValue, LayerMask.GetMask("Tiles"));
            startTile = rayHit.transform.GetComponent<Tile>();
        }

        // Call the path finding algo
        Queue<Tile> path = pathFinding.FindPath(startTile, endTile);
        
        // Change the color of the tiles on the paths (excluding the start and end tile)
        if (path != null)
            foreach (Tile tile in path)
                if (tile != endTile && tile != startTile)
                    tile.SetColor(Color.white);

        // If there's a traveler set it's path
        if (traveler != null)
            traveler.SetPath(path);
    }

    private bool HandleTravelerInput()
    {
        // AStar
        if (Input.GetKey(KeyCode.Q)){
            pathFinding.SetAlgorithm(Algorithm.AStar);
            return true;
        }

        // Dijkstra
        if (Input.GetKey(KeyCode.D)){
            pathFinding.SetAlgorithm(Algorithm.Dijkstra);
            return true;
        }

        // Spawn the traveler
        if (startTile != null && endTile != null && Input.GetKey(KeyCode.F)) {
            // If the travaler has not been instantiated yet, instantiate it on the starting tile, else just place it on the starting tile
            if (traveler == null)
                traveler = Instantiate(travelerPrefab, startTile.GetTravelerInstantiationPos(), Quaternion.identity).GetComponent<Traveler>();

            return true;
        }

        // If the code execute up to here then no changes happened
        return false;
    }

    private bool HandleMapInput()
    {

        // Check which tile is hovered if any 
        Tile tileUnderMouse = GetTileUnderMouse();

        // If the player is not hovering a tile or hovering the start or end tile, make an early return
        if (tileUnderMouse == null || tileUnderMouse == endTile || tileUnderMouse == startTile)
            return false;

        // START
        if (Input.GetMouseButtonDown(0) && CanPlace(tileUnderMouse)) {
            startTile = tileUnderMouse;
            return true;
        }

        // END
        if (Input.GetMouseButtonDown(1) && CanPlace(tileUnderMouse)){        
            endTile = tileUnderMouse;
            return true;
        }
        
        // PLAINS
        if (Input.GetKey(KeyCode.A))
            return tileUnderMouse.SetTileType(TileType.Plains);
        
        // WOODS
        if (Input.GetKey(KeyCode.W))
           return tileUnderMouse.SetTileType(TileType.Woods);
  

        // WALL
        if (Input.GetKey(KeyCode.E))
            return tileUnderMouse.SetTileType(TileType.Wall);
            
        // ROAD
        if (Input.GetKey(KeyCode.R))
            return tileUnderMouse.SetTileType(TileType.Road);

        // RIVER
        if (Input.GetKey(KeyCode.T))
            return tileUnderMouse.SetTileType(TileType.River);

        // If code execute to here then no changes have been made
        return false;
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

    // START and END can only be placed on plains
    private bool CanPlace(Tile tile) { return tile.GetTileType() == TileType.Plains; }

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
