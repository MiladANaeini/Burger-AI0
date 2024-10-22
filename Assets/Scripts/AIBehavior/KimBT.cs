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
        myBehaviorTree.myBlackBoard = new BlackBoard();

        myBehaviorTree.myRootNode = new Selector(new List<Node>
        {
             new Sequence(new List<Node>
            {
                new CheckZombieCondition(myKim),      // If a zombie is detected, proceed.
                new FindPathTask(myKim),              // Recalculate the path to avoid the zombie.
                new ResetZombieDetectionTask(myKim),  // Reset zombie detection flag after pathfinding.
                new CheckForZombieTask(myKim),        // Check for zombies in the vicinity.
                new MoveToFinishTask(myKim)           // Move along the newly found path.
            }),

            // If no zombie is detected, proceed with normal pathfinding and movement.
            new Sequence(new List<Node>
            {
                new FindPathTask(myKim),              // Normal pathfinding (if no zombie is detected).
                new CheckForZombieTask(myKim),        // Regular zombie check.
                new MoveToFinishTask(myKim)           // Move to the finish.
            }),
        });
        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }
    private void Update()
    {
        myBehaviorTree.UpdateTree();
    }
}
