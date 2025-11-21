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
        // Initialisation
        Vector3 startPos;
        Vector3 endPos;
        Tile lastTile = pathToFollow.Pop();

        // If the traveler was not yet traveling set the startingTilePos as the startPos
        // Else the set the Traveler pos as the startPos
        if (!isTraveling) {

            // Wait a small time before starting moving along the path
            yield return new WaitForSeconds(waitTime);

            isTraveling = true;
            startPos = lastTile.GetPathPos();

        }
        else
            startPos = gameObject.transform.position;


        // While there is still tile in the path
        while (pathToFollow.Count > 0) {

            // Loop Initialisation
            float lerpValue = 0;
            Tile nextTile = pathToFollow.Pop();
            endPos = nextTile.GetPathPos();
            transform.LookAt(endPos, Vector3.up);

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
                transform.position = Vector3.Lerp(startPos, endPos, lerpValue);

                // Mandatory so we send back the control to Unity so it can move the traveler
                yield return null;
            }

            // When arrived, the EndPos becomes the startPos, idem for last and next tile
            startPos = endPos;
            lastTile = nextTile;
        }

        // When the traveler as reached the destination, wait a small time before destroying the game object
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
