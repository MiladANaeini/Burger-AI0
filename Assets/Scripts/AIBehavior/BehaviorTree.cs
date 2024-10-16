using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree 
{
    public Node rootNode;
    public BlackBoard blackBoard;
     
    public void UpdateTree()
    {
        rootNode.EvaluateState();
    }
}

public class BlackBoard
{
    public Dictionary<string, object> Data;

}
