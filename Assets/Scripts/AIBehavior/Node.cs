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
    public Node(List<Node> someChildren)
    {
        myChildren = someChildren;
        foreach (Node c in myChildren) c.Parent = this;
        
    }

    protected List<Node> myChildren = new List<Node>();
    BlackBoard myBlackBoard = null;
    public Node Parent = null;

    public void PopulateBlackboard(BlackBoard aBlackBoard)
    {
        myBlackBoard = aBlackBoard;
        foreach (Node child in myChildren) { 
        child.PopulateBlackboard(myBlackBoard);
        }
    }

    public virtual ReturnState EvaluateState() => ReturnState.s_Failure;
}
  