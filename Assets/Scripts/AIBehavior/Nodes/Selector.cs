using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(List<Node> someChildren) : base(someChildren)
    {
    }

    public override ReturnState EvaluateState()
    {
        // Start with a default state of failure
        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();
            switch (childState)
            {
                case ReturnState.s_Success:
                    return ReturnState.s_Success; // Return immediately if any child succeeds
                case ReturnState.s_Failure:
                    continue; // Continue to next child
                case ReturnState.s_Running:
                    return ReturnState.s_Running; // Return running if any child is running
            }
        }

        // If we reach here, it means all children failed
        return ReturnState.s_Failure;
    }
}
