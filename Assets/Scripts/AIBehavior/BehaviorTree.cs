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
    public Dictionary<string, object> Data { get; private set; }

    public BlackBoard()
    {
        Data = new Dictionary<string, object>();
        Data["zombieDetected"] = false; // Initialize the flag
    }
}
