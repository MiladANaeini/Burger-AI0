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
                new CollectBurgersTask(myKim),   // Select target burger
                new FindPathTask(myKim),         // Find path to the selected burger
                new MoveToFinishTask(myKim)      // Move to the burger
            }),
            new Sequence(new List<Node>
            {
                new FindPathTask(myKim),         // Find path to the finish tile (once burgers are done)
                new MoveToFinishTask(myKim)      // Move to the finish tile
            })
        });


        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }
    private void Update()
    {
        myBehaviorTree.UpdateTree();
    }
}
