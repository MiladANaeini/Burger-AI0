using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDetectTask : Node
{
    private Kim myKim;
    public MoveAndDetectTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }
    public override ReturnState EvaluateState()
    {
        var checkForZombieTask = new CheckForZombieTask(myKim);
        var zombieState = checkForZombieTask.EvaluateState();

        if (zombieState == ReturnState.s_Failure)
        {
            Debug.Log("Zombie detected! Stopping movement and finding another path.");

            //myKim.myWalkBuffer.Clear();

            return ReturnState.s_Failure;
        }

        var moveToFinishTask = new MoveToFinishTask(myKim);
        var moveState = moveToFinishTask.EvaluateState();

        // Debug movement state for tracking behavior
        Debug.Log("Movement state: " + moveState);

        if (moveState == ReturnState.s_Success)
        {
            Debug.Log("Kim has reached the destination.");
            return ReturnState.s_Success;
        }

        // Otherwise, keep moving towards the finish
        return ReturnState.s_Running;
    }
}
