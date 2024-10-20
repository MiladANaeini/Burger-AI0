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
        // Default to failure
        ReturnState aState = ReturnState.s_Success;
        bool anyChildRunning = false; 

        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();

            switch (childState)
            {
                case ReturnState.s_Success:
                    break;
                case ReturnState.s_Failure:
                    aState = ReturnState.s_Failure;
                    break;
                case ReturnState.s_Running:
                    anyChildRunning = true;
                    aState = ReturnState.s_Running;
                    break;
            }
        }

        return anyChildRunning ? ReturnState.s_Running : aState;
    }
}
