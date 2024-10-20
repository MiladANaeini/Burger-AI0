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
            // Main sequence: Find path, check for zombies, move and detect zombies during movement
            new Sequence(new List<Node>
            {
                new FindPathTask(myKim),       // Step 1: Find a path
                new CheckForZombieTask(myKim), // Step 2: Initial check for zombies before moving
                new MoveAndDetectTask(myKim)   // Step 3: Continuously move and check for zombies
            }),
        });

        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }
    private void Update()
    {
        myBehaviorTree.UpdateTree();
    }
}
