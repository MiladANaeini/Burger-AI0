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

        myBehaviorTree.myRootNode = new Sequence(new List<Node>
        {
            new Sequence(new List<Node>
            {

                new FindPathTask(myKim),       
                new CheckForZombieTask(myKim),       
                new MoveToFinishTask(myKim)   
            }),
        });

        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);
    }
    private void Update()
    {
        myBehaviorTree.UpdateTree();
    }
}
