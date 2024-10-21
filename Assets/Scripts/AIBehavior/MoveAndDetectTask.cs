using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndDetectTask : Node
{
    private Kim myKim;
    public MoveAndDetectTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }

    public override ReturnState EvaluateState()
    {
   
        bool zombieFound = false;
        Collider[] colliders = Physics.OverlapSphere(myKim.transform.position, myKim.ContextRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                zombieFound = true;
            }
        }

 
        myKim.blackboard.Data["zombieDetected"] = zombieFound;

        if (zombieFound)
        {
        
            Debug.Log("Zombie detected! Stopping movement and finding another path.");
            myKim.myReachedDestination = false;
            return ReturnState.s_Failure; 
        }

        var moveToFinishTask = new MoveToFinishTask(myKim);
        var moveState = moveToFinishTask.EvaluateState();

        if (moveState == ReturnState.s_Success)
        {
            return ReturnState.s_Success;
        }

        return ReturnState.s_Running;
    }
}
