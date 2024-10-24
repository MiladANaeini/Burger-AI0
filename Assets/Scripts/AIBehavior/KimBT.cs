using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;

public class KimBT : MonoBehaviour
{
    private BehaviorTree myBehaviorTree;
    private CheckForZombieTask zombieCheckTask;
    private bool pathFound = false; 
    private void Start()
    {
        Kim myKim = GetComponent<Kim>();

        myBehaviorTree = new BehaviorTree();
        myBehaviorTree.myBlackBoard = new BlackBoard();
        zombieCheckTask = new CheckForZombieTask(myKim);
        myBehaviorTree.myRootNode = new Selector(new List<Node>
{
   
    new Sequence(new List<Node>
    {
        new GoAroundZombieTask(myKim, this),
        new FindPathTask(myKim),       
    }),

    new Sequence(new List<Node>
    {
        new Parallel(new List<Node>
        {
            new CheckForZombieTask(myKim), 
            new MoveToFinishTask(myKim),  
        }, 1), 
    }),
});
        myBehaviorTree.myRootNode.PopulateBlackboard(myBehaviorTree.myBlackBoard);

    }

    private void Update()
    {
        ReturnState treeState = myBehaviorTree.myRootNode.EvaluateState();

        // Add logging to see the tree's state.
        Debug.Log("Evaluating tree state: " + treeState);

        if (treeState == ReturnState.s_Failure || treeState == ReturnState.s_Success)
        {
            // Reset the entire tree so it can evaluate again next frame.
            Debug.Log("Tree completed with state: " + treeState + ", resetting for next evaluation.");
            myBehaviorTree.myRootNode.ResetCachedState();
            zombieCheckTask.ClearDetectedZombies();  // Clear zombie detection if needed.
        }
    }
    public bool IsPathFound()
    {
        return pathFound;
    }
    public void SetPathFound(bool found)
    {
        pathFound = found;
    }

    private void ResetZombieRelatedTasks()
    {
        foreach (Node child in myBehaviorTree.myRootNode.myChildren)
        {
            if (child is Sequence sequence)
            {
                foreach (Node sequenceChild in sequence.myChildren)
                {
                    Debug.Log($"Resetting task: {sequenceChild.GetType().Name}");
                    sequenceChild.ResetCachedState();
                }
            }
        }
    }

}
