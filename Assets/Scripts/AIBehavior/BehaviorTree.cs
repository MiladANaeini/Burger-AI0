using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree 
{
    public Node myRootNode;
    public BlackBoard myBlackBoard;
    public void UpdateTree()
    {
        Debug.Log("BT");

        myRootNode.EvaluateState();
    }
}

public class BlackBoard
{
    public Dictionary<string, object> Data;

}
