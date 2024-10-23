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

        if ((bool)myKim.blackboard.Data["zombieDetected"])
        {
            Debug.Log("Zombie detected! Stopping movement. " +
                "Blackboard value - zombieDetected: " + myKim.blackboard.Data["zombieDetected"]);
            return ReturnState.s_Failure; 
        }


        if (!myKim.blackboard.Data.ContainsKey("path"))
        {
            Debug.Log("No path in blackboard.");
            return ReturnState.s_Failure;
        }


        List<Grid.Tile> path = (List<Grid.Tile>)myKim.blackboard.Data["path"];
   
        if (myKim.myWalkBuffer.Count == 0 && path.Count > 0)
        {
           
                myKim.SetWalkBuffer(path);
                Debug.Log("Path set in Kim's walk buffer.");
            
        }

    
        if (myKim.myReachedDestination)
        {
            return ReturnState.s_Running;
        }

        return ReturnState.s_Success;
    }
}
