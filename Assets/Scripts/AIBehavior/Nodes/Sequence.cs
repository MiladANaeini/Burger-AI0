using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence(List<Node> someChildren) : base(someChildren)
    {
    }

    public override ReturnState EvaluateState()
    {

        ReturnState aState = ReturnState.s_Failure;

        bool isRunning = false;

        foreach (Node child in myChildren)
        {
            switch (child.EvaluateState()) { 
            case ReturnState.s_Success:
                    aState = ReturnState.s_Success;
                    return aState;
            case ReturnState.s_Failure:
                    break;
            case ReturnState.s_Running:
                    aState = ReturnState.s_Running;
                    break;
            }
        }
        if (isRunning) aState = ReturnState.s_Running;
             return aState;
    }
}
