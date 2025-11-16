using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : MonoBehaviour
{
    [Header("Movement Values")]
    public float moveSpeed = 1f;
    public float startWaitTime = 1f;

    private Queue<Tile> pathToFollow;

    public void SetPath(Queue<Tile> path)
    {
        // Set the path
        pathToFollow = path;

        // Stop any movements already in progress then restart the movement
        StopAllCoroutines();
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        // Wait a small time before starting moving along the path
        yield return new WaitForSeconds(startWaitTime);

        // Get the first tile of the path (startTile) as the first pos
        Tile lastTile = pathToFollow.Dequeue();

        // While there is still tile in the path
        while (pathToFollow.Count > 0)
        {
            // Reset the lerpValue, set the next tile to reach and rotate the model so it faces the path
            float lerpValue = 0;
            Tile nextTile = pathToFollow.Dequeue();
            transform.LookAt(nextTile.transform, Vector3.up);

            // While we haven't the center of the next tile keep moving
            while (lerpValue < 1) {

                // Set the move speed depending on the movement cost of the tiles 
                // NOTE : if lerpValue < .5 we're still on the lastTile else we're on the nextTile
                float effectiveSpeed = moveSpeed;
                if (lerpValue < .5f)
                    effectiveSpeed /= lastTile.GetCost();
                else
                    effectiveSpeed /= nextTile.GetCost();

                // Update the lerpValue and move accordingly
                lerpValue += Time.deltaTime * effectiveSpeed;
                transform.position = Vector3.Lerp(lastTile.transform.position, nextTile.transform.position, lerpValue);

                // Mandatory so we send back the control to Unity so it can move the traveler
                yield return null;
            }

            // When arrived the nextTile now becomes the lastTile
            lastTile = nextTile;
        }
    }
}
