using System.Collections.Generic;

public class Node
{
    public enum ReturnState
    {
        s_Success,
        s_Running,
        s_Failure,
    }

    public List<Node> myChildren = new List<Node>();
    public BlackBoard myBlackBoard = null;
    public Node Parent = null;

    // Cache the last evaluated state
    protected ReturnState cachedState = ReturnState.s_Running;

    public Node(List<Node> someChildren)
    {
        myChildren = someChildren;
        foreach (Node c in myChildren) c.Parent = this;
    }

    public void PopulateBlackboard(BlackBoard aBlackBoard)
    {
        myBlackBoard = aBlackBoard;
        foreach (Node child in myChildren)
        {
            child.PopulateBlackboard(myBlackBoard);
        }
    }

    // Base EvaluateState to check cache before actual evaluation
    public virtual ReturnState EvaluateState()
    {
        // If the node is already successful or failed, return the cached state
        if (cachedState != ReturnState.s_Running)
        {
            return cachedState;
        }

        // Default evaluation logic (to be overridden by child nodes)
        return ReturnState.s_Failure;
    }

    // Reset method to clear the cached state
    public virtual void Reset()
    {
        cachedState = ReturnState.s_Running;
        foreach (Node child in myChildren)
        {
            child.Reset();
        }

        // Also reset the parent node
        if (Parent != null)
        {
            Parent.Reset();
        }
    }
    public virtual void ResetCachedState()
    {
        cachedState = ReturnState.s_Running; // Reset to running state
        foreach (Node child in myChildren)
        {
            child.ResetCachedState(); // Reset all children nodes as well
        }
    }
}
