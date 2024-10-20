using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KimBT : MonoBehaviour
{
    private BehaviorTree myBehaviorTree;

    private void Start()
    {
        Kim myKim = GetComponent<Kim>();

        myBehaviorTree = new BehaviorTree();
        myBehaviorTree.myBlackBoard = new BlackBoard(); // Ensure the blackboard is initialized

        // The root is a sequence
        myBehaviorTree.myRootNode = new Sequence(new List<Node>
        {
            new FindPathTask(myKim, myBehaviorTree.myBlackBoard), // Try to find a path
            new MoveToFinishTask(myKim, myBehaviorTree.myBlackBoard) // Move to the finish if pathfinding was successful
        });

        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }
    private void Update()
    {
        myBehaviorTree.UpdateTree();
    }
}
