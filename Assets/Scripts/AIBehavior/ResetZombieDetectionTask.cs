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
        Debug.Log("Resetting zombieDetected111111111111111" + myKim.blackboard.Data["zombieDetected"]);
        if ((bool)myKim.blackboard.Data["zombieDetected"])
        {
            myKim.blackboard.Data["zombieDetected"] = false;
        Debug.Log("Resetting zombieDetected2222222222222222" + myKim.blackboard.Data["zombieDetected"]);
        return ReturnState.s_Success;

        }
        return ReturnState.s_Failure;

    }

}
