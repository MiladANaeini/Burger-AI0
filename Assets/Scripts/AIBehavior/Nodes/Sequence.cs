using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(List<Node> someChildren) : base(someChildren) { }

    public override ReturnState EvaluateState()
    {
        // If already evaluated and cached, return cached result
        if (cachedState != ReturnState.s_Running)
        {
            return cachedState;
        }

        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();

            if (childState == ReturnState.s_Failure)
            {
                cachedState = ReturnState.s_Failure; // Cache failure
                return cachedState;
            }

            if (childState == ReturnState.s_Running)
            {
                return ReturnState.s_Running;
            }
        }

        cachedState = ReturnState.s_Success; // Cache success if all children succeed
        return cachedState;
    }
}

