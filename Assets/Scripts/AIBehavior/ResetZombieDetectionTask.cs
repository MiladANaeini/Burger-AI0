using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;

public class ResetZombieDetectionTask : Node
{
    private Kim myKim;

    public ResetZombieDetectionTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    public override ReturnState EvaluateState()
    {
        Debug.Log("Resetting zombieDetected flag to false");
        myKim.blackboard.Data["zombieDetected"] = false;
        return ReturnState.s_Success;
    }
}
