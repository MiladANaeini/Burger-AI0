using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Node
{
    private int successThreshold;  // How many tasks need to succeed for the Parallel node to succeed

    public Parallel(List<Node> someChildren, int successThreshold) : base(someChildren)
    {
        this.successThreshold = successThreshold;
    }

    public override ReturnState EvaluateState()
    {
        int successCount = 0;

        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();

            if (childState == ReturnState.s_Success)
                successCount++;
            else if (childState == ReturnState.s_Failure)
                return ReturnState.s_Failure; // Fail immediately if any child fails

            if (successCount >= successThreshold)
                return ReturnState.s_Success;
        }

        return ReturnState.s_Running;
    }
}

