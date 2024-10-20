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
        // Check if zombie detection flag is true
        if ((bool)myKim.blackboard.Data["zombieDetected"])
        {
            Debug.Log("Zombie detected! Stopping movement.");
            return ReturnState.s_Failure; // Stop moving if zombies are detected
        }

        // Get the path from the blackboard
        if (!myKim.blackboard.Data.ContainsKey("path"))
        {
            Debug.Log("No path in blackboard.");
            return ReturnState.s_Failure;
        }

        List<Grid.Tile> path = (List<Grid.Tile>)myKim.blackboard.Data["path"];

        // Set the walk buffer if it's not already set
        if (myKim.myWalkBuffer.Count == 0 && path.Count > 0)
        {
            myKim.SetWalkBuffer(path);
            Debug.Log("Path set in Kim's walk buffer.");
        }

        // Check if destination is reached
        if (myKim.myReachedDestination)
        {
            Debug.Log("Kim has reached the destination.");
            return ReturnState.s_Success;
        }

        return ReturnState.s_Running; // Still moving
    }
}
