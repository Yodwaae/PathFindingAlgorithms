using System;
using System.Collections.Generic;

public class PathFindingQueue<T>
{
    // First Element is the Tile, 2nd is the cost to reach it
    private List<Tuple<T, float>> elements = new List<Tuple<T, float>>();

    public void Enqueue(T item, float priority){ elements.Add(Tuple.Create(item, priority)); }

    public T Dequeue()
    {
        // Initialisation
        int bestIndex = 0;

        // Get the element with the smallest cost to reach (so the highest priority)
        for (int i = 0; i < elements.Count; i++)
            if (elements[i].Item2 < elements[bestIndex].Item2)
                bestIndex = i;
        
        // Remove and return the highest priority element
        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    // NOTE : I prefer using getters but seeing as PathFindingQueue is a data struct, using a property seems more apropriate
    public int Count => elements.Count;
}

