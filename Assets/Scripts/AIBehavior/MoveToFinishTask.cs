using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveToFinishTask : Node
{
    private Kim myKim;
    private BlackBoard blackboard; // Change this from BehaviorTree to BlackBoard

    public MoveToFinishTask(Kim kim, BlackBoard blackboard) : base(new List<Node>())
    {
        myKim = kim;
        this.blackboard = blackboard; // Store the reference to the blackboard
        Debug.Log("LLLLLLL");

    }

    public override ReturnState EvaluateState()
    {
        Debug.Log("MoveToFinishTask EvaluateState called.");

        // Get the path from the blackboard
        if (!blackboard.Data.ContainsKey("path"))
        {
            Debug.Log("No path in blackboard.");
            return ReturnState.s_Failure;
        }

        List<Grid.Tile> path = (List<Grid.Tile>)blackboard.Data["path"];

        // Set the walk buffer only if it's not already set
        if (myKim.myWalkBuffer.Count == 0 && path.Count > 0)
        {
            myKim.SetWalkBuffer(path);
            Debug.Log("Path set in Kim's walk buffer.");
        }

        //// Move Kim along the path
        //myKim.UpdateCharacter();

        // Check if destination is reached
        if (myKim.myReachedDestination)
        {
            Debug.Log("Kim has reached the destination.");
            return ReturnState.s_Success;
        }

        return ReturnState.s_Running;
    }
}
