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
        bool anyChildRunning = false; // Track if any child is running

        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();

            switch (childState)
            {
                case ReturnState.s_Success:
                    // If any child is successful, continue to check the rest
                    break;
                case ReturnState.s_Failure:
                    // If any child fails, the sequence fails
                    aState = ReturnState.s_Failure;
                    break;
                case ReturnState.s_Running:
                    // If any child is running, mark this sequence as running
                    anyChildRunning = true;
                    aState = ReturnState.s_Running;
                    break;
            }
        }

        // If no child has failed, but at least one is running
        return anyChildRunning ? ReturnState.s_Running : aState;
    }
}
