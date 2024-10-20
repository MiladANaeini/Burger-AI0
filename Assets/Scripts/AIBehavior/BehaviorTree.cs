using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree 
{
    public Node myRootNode;
    public BlackBoard myBlackBoard;
    public void UpdateTree()
    {

        myRootNode.EvaluateState();
    }
}

public class BlackBoard
{
    public Dictionary<string, object> Data;

    // Initialize the Data dictionary in the constructor
    public BlackBoard()
    {
        Data = new Dictionary<string, object>();
    }
}
