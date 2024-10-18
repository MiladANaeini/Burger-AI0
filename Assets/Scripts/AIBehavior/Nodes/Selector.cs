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
        ReturnState aState = ReturnState.s_Failure;

        foreach (Node child in myChildren)
        {
            switch (child.EvaluateState())
            {
                case ReturnState.s_Success:
                    aState = ReturnState.s_Success;
                    return aState;
                case ReturnState.s_Failure:
                    continue;
                case ReturnState.s_Running:
                    aState = ReturnState.s_Running;
                    return aState;

            }
        }
        
        return aState;
    }
}
