using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveToFinishTask : Node
{
    private Kim myKim;
    private List<Grid.Tile> lastPath;
    public MoveToFinishTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
        lastPath = null;
    }

    public override ReturnState EvaluateState()
    {

        if (myKim.blackboard.Data.ContainsKey("zombieDetected") && (bool)myKim.blackboard.Data["zombieDetected"])
        {
            Debug.Log("movetask Failed");
          
            return ReturnState.s_Failure; 
        }


        // Check if a valid path exists in the blackboard
        if (!myKim.blackboard.Data.ContainsKey("path") || myKim.blackboard.Data["path"] == null)
        {
            Debug.LogError("No path found in blackboard.");
            return ReturnState.s_Failure;
        }


        List<Grid.Tile> currentPath = (List<Grid.Tile>)myKim.blackboard.Data["path"];

        // Check if the path has changed
        if (lastPath == null || !currentPath.SequenceEqual(lastPath))
        {
            Debug.Log("Path has changed. Resetting MoveToFinishTask.");
            lastPath = new List<Grid.Tile>(currentPath);  // Store the new path

            // Reset Kim's walk buffer with the updated path
            myKim.SetWalkBuffer(currentPath);

            // Perform any other necessary resets here
        }


        if (myKim.myReachedDestination)
        {

            return ReturnState.s_Success;
        }
            Debug.Log("kos");

        return ReturnState.s_Running;
    }
}
