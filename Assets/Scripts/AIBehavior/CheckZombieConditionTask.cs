using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckZombieCondition : Node
{
    private Kim myKim;

    public CheckZombieCondition(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    // Evaluates the condition: whether a zombie is detected
    public override ReturnState EvaluateState()
    {
        Debug.Log("Con"+ myKim.blackboard.Data["zombieDetected"]);
        if ((bool)myKim.blackboard.Data["zombieDetected"])
        {
            Debug.Log("KIR");

            return ReturnState.s_Success;
        }
        return ReturnState.s_Failure;

    }
}
