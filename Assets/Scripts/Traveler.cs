using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// TODO Case no path available what do I do
public class Traveler : MonoBehaviour
{
    [Header("Movement Values")]
    public float moveSpeed = 1f;
    public float waitTime = 1f;

    // TRAVELING
    public bool isTraveling;
    private Stack<Tile> pathToFollow;

    public void SetPath(Queue<Tile> path)
    {
        // NOTE: The path is ordered from end to start, 
        // we reverse it using a stack so the traveler can move from start to end (FIFO -> LIFO)
        pathToFollow = new Stack<Tile>(path);

        // Stop any movements already in progress then restart the movement
        StopAllCoroutines();
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        // If the traveler was not yet traveling wait a small time before starting moving along the path
        if (!isTraveling){
            isTraveling = true;
            yield return new WaitForSeconds(waitTime);

        }

        // Get the first tile of the path (startTile) as the first pos
        Tile lastTile = pathToFollow.Pop();

        // While there is still tile in the path
        while (pathToFollow.Count > 0)
        {
            // Reset the lerpValue, set the next tile to reach and rotate the model so it faces the path
            float lerpValue = 0;
            Tile nextTile = pathToFollow.Pop();
            transform.LookAt(nextTile.GetPathPos(), Vector3.up);

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
                transform.position = Vector3.Lerp(lastTile.GetPathPos(), nextTile.GetPathPos(), lerpValue);

                // Mandatory so we send back the control to Unity so it can move the traveler
                yield return null;
            }

            // When arrived the nextTile now becomes the lastTile
            lastTile = nextTile;
        }

        // When the traveler as reached the destination, wait a small time before destroying the game object
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
