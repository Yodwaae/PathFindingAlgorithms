using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Algorithm
{
    Dijkstra,
    AStar
}

public class PathFindingAlgorithm : MonoBehaviour
{
    [Header("Path Finding Algorithm")]
    [SerializeField] private Algorithm currentAlgorithm;

    private MapGenerator mapGenerator;
    private void Start(){ mapGenerator = FindFirstObjectByType<MapGenerator>(); }

    public Queue<Tile> FindPath(Tile start, Tile end)
    {
        // Initialisation
        Queue<Tile> path = new Queue<Tile>(); // Queue the function will return, containing the path
        Dictionary<Tile, Tile> NextTileToGoal = new Dictionary<Tile, Tile>(); // Queue we will use to reconstruct the path once the end is found
        Dictionary<Tile, float> costToReachTile = new Dictionary<Tile, float>(); // Queue containing the cost to reach the tiles for every tile we explored
        PathFindingQueue<Tile> frontier = new PathFindingQueue<Tile>(); // Priority queue used to organise in which order we search the tiles

        // Start exploring from the start tile
        frontier.Enqueue(start, 0);
        costToReachTile[start] = 0;

        // While there is still tiles we haven't explored, keep searching
        while (frontier.Count > 0) {
            Tile curTile = frontier.Dequeue();

            // If we reached the end stop the search
            if (curTile == end)
                break;

            // Explore every adjacent tile
            foreach (Tile neighbor in mapGenerator.GetNeighbors(curTile)) {

                // Loop Initialisation
                float priority = 0;
                float newCost = costToReachTile[curTile] + neighbor.GetCost();

                // If the tile has been visited or the path is costier than what we found, skip it
                if (costToReachTile.ContainsKey(neighbor) && newCost >= costToReachTile[neighbor])
                    continue;
                // If the tile is a wall skip it
                if (neighbor.GetTileType() == TileType.Wall)
                    continue;

                // Set the cost to reach the tile
                costToReachTile[neighbor] = newCost;

                // Compute the priority based on the Algorithm used
                if (currentAlgorithm == Algorithm.AStar)
                    priority = newCost + Distance(neighbor, end);
                else if (currentAlgorithm == Algorithm.Dijkstra)
                    priority = newCost;

                // Add the neighbor to the list of tile to explore, then register how to go back to the start from the tile
                // and finally set the text of the tile to the cost to reach it
                frontier.Enqueue(neighbor, priority);
                NextTileToGoal[neighbor] = curTile;
                if (neighbor != end)
                    neighbor.SetText(costToReachTile[neighbor].ToString());
          
            }
        }

        // Check if tile is reachable
        if (NextTileToGoal.ContainsKey(end) == false)
            return null;


        // Rebuild the path by walking from end -> start using NextTileToGoal
        Tile pathTile = end;

        while (pathTile != start) {
            path.Enqueue(pathTile);
            pathTile = NextTileToGoal[pathTile];
        }

        path.Enqueue(start);
        return path;
    }

    public void SetAlgorithm(Algorithm algorithm) {
        currentAlgorithm = algorithm; }

    // Helper function
    float Distance(Tile tile1, Tile tile22) { return Mathf.Abs(tile1.GetXCoord() - tile22.GetXCoord()) + Mathf.Abs(tile1.GetYCoord() - tile22.GetYCoord()); }

}
