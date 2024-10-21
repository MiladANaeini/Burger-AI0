using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveToFinishTask : Node
{
    private Kim myKim;
    public MoveToFinishTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    public override ReturnState EvaluateState()
    {
        // Check if the path is set in the blackboard
        if (!myKim.blackboard.Data.ContainsKey("path"))
        {
            Debug.Log("No path in blackboard.");
            return ReturnState.s_Failure;
        }

        List<Grid.Tile> path = (List<Grid.Tile>)myKim.blackboard.Data["path"];

        // If Kim's walk buffer is empty, set it with the path from the blackboard
        if (myKim.myWalkBuffer.Count == 0 && path.Count > 0)
        {
            myKim.SetWalkBuffer(path);
            Debug.Log("Path set in Kim's walk buffer.");
        }

        // Check if Kim has reached her destination
        if (myKim.myReachedDestination)
        {
            Debug.Log("Destination reached!");

            // Clear the walk buffer to prepare for the next task
            myKim.myWalkBuffer.Clear();

            // Indicate success (whether it's a burger or the finish)
            return ReturnState.s_Success;
        }

        // Still moving towards the destination
        return ReturnState.s_Running;
    }
}
