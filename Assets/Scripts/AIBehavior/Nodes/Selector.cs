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
        foreach (Node child in myChildren)
        {
            ReturnState childState = child.EvaluateState();
            switch (childState)
            {
                case ReturnState.s_Success:
                    return ReturnState.s_Success; 
                case ReturnState.s_Failure:
                    continue; 
                case ReturnState.s_Running:
                    return ReturnState.s_Running; 
            }
        }

        return ReturnState.s_Failure;
    }
}
