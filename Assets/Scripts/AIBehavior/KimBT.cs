using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;

public class KimBT : MonoBehaviour
{
    private BehaviorTree myBehaviorTree;

    private void Start()
    {
        Kim myKim = GetComponent<Kim>();

        myBehaviorTree = new BehaviorTree();
        myBehaviorTree.myBlackBoard = new BlackBoard();

        myBehaviorTree.myRootNode = new Selector(new List<Node>
{
    // First sequence: Check for zombies and find a path
    new Sequence(new List<Node>
    {
        new GoAroundZombieTask(myKim), // Always check for zombies and find a new path if necessary
        new FindPathTask(myKim),       // Once a safe path is found
    }),

    // Second sequence: Continuous movement and zombie detection
    new Sequence(new List<Node>
    {
        new Parallel(new List<Node>
        {
            new CheckForZombieTask(myKim), // Continuous zombie detection
            new MoveToFinishTask(myKim),   // Continuous movement towards the target
        }, 1), // Parallel node requires only 1 success
    }),
});
        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);

    }
    private void Update()
    {
        // Evaluate the root node on every frame
        ReturnState treeState = myBehaviorTree.myRootNode.EvaluateState();

        // Check if the tree failed, then reset
        if (treeState == ReturnState.s_Failure)
        {
            // Optionally check for specific failure reasons (e.g., zombie detected)
            if ((bool)myBehaviorTree.myBlackBoard.Data["zombieDetected"])
            {
                Debug.Log("Zombie detected, resetting zombie-related tasks.");
                ResetZombieRelatedTasks(); // Specific reset logic for zombie tasks
            }
            else
            {
                Debug.Log("Tree failed, resetting entire behavior tree.");
                // General reset if failure happens, not limited to zombies
                myBehaviorTree.myRootNode.ResetCachedState();
            }
        }
    }

    private void ResetZombieRelatedTasks()
    {
        // Reset all relevant nodes within the behavior tree
        foreach (Node child in myBehaviorTree.myRootNode.myChildren)
        {
            if (child is Sequence sequence)
            {
                // Iterate through the sequence's children and reset them
                foreach (Node sequenceChild in sequence.myChildren)
                {
                    // Add conditional checks if needed (e.g., for specific zombie-related tasks)
                    Debug.Log($"Resetting task: {sequenceChild.GetType().Name}");
                    sequenceChild.ResetCachedState();
                }
            }
        }
    }

}
