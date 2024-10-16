using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
     public enum ReturnState
    {
        s_Success,
        s_Running,
        s_Failure,
    }
    public Node(List<Node> someChildren, BlackBoard aBlackBoard)
    {
        myChildren = someChildren;
        myBlackboard = aBlackBoard;
        foreach (Node c in myChildren) c.Parent = this;
        
    }

    protected List<Node> myChildren = new List<Node>();
    BlackBoard myBlackboard = null;
    public Node Parent = null;

    public virtual ReturnState EvaluateState() => ReturnState.s_Failure;
}
  