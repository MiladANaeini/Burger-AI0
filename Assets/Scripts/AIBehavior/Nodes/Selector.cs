using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector(List<Node> someChildren) : base(someChildren) { }

    public override ReturnState EvaluateState()
    {
        if (cachedState != ReturnState.s_Running)
        {
            return cachedState;
        }

        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();
            if (childState == ReturnState.s_Success)
            {
                cachedState = ReturnState.s_Success; // Cache success
                return cachedState;
            }
            if (childState == ReturnState.s_Running)
            {
                return ReturnState.s_Running;
            }
        }

        cachedState = ReturnState.s_Failure; // Cache failure if all children fail
        return cachedState;
    }
}


