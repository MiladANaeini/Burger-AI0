using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimBT : MonoBehaviour
{
    private BehaviorTree myBehaviorTree;  // Declare a BehaviorTree instance

    private void Start()
    {
        Debug.Log("KimBT");

        Kim myKim = GetComponent<Kim>();

        // Create the behavior tree
        myBehaviorTree = new BehaviorTree();

        // Initialize the root node with a sequence
        myBehaviorTree.myRootNode = new Sequence(new List<Node>
        {
            new MoveToFinishTask(myKim)
        });

        // Populate the blackboard for the behavior tree
        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }

    private void Update()
    {
        // Evaluate the behavior tree in the game's update loop
        myBehaviorTree.UpdateTree();
    }
}
