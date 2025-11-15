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
    private MapGenerator mapGenerator;

    public Algorithm currentAlgorithm;

    // TODO Faire de MapGenerator un singleton ?
    private void Start(){ mapGenerator = FindFirstObjectByType<MapGenerator>(); }

    public Queue<Tile> FindPath(Tile start, Tile end)
    {
        switch (currentAlgorithm){

            case Algorithm.Dijkstra:
                return new Queue<Tile>();

            case Algorithm.AStar:
                return new Queue<Tile>();
        }

        return null;
    } 




}
