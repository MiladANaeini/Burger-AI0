using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;

public class BehaviorTree
{
    public Node myRootNode;
    public BlackBoard myBlackBoard;

    // Method to reset the entire tree's state before updating
    public void ResetTree(Node node)
    {
        node.ResetCachedState();  // Use method to reset the cached state

        // Recursively reset the states of all child nodes, if any
        foreach (Node child in node.myChildren)
        {
            ResetTree(child);
        }
    }

    public void UpdateTree()
    {
        if (myRootNode == null || myBlackBoard == null)
        {
            Debug.LogError("Root node or Blackboard is not set in the behavior tree!");
            return;
        }

        // Reset the tree before evaluating it
        ResetTree(myRootNode);

        // Populate the blackboard data (if needed)
        myRootNode.PopulateBlackboard(myBlackBoard);

        // Evaluate the tree
        myRootNode.EvaluateState();
    }
}

public class BlackBoard
{
    public Dictionary<string, object> Data { get; private set; }

    public BlackBoard()
    {
        Data = new Dictionary<string, object>
        {
            { "zombieDetected", false },
            { "path", new List<Grid.Tile>() }, 
            { "finishTile", null } 
        };
    }
}
