using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForZombieTask : Node
{
    private Kim myKim;

    public CheckForZombieTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }


    public override ReturnState EvaluateState()
    {
        Collider[] colliders = Physics.OverlapSphere(myKim.transform.position, myKim.ContextRadius);
        bool zombieFound = false;
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                myKim.myWalkBuffer.Clear();
                myKim.blackboard.Data.Remove("path");
                zombieFound = true;
                break; // Exit loop once a zombie is found
            }
        }

        // Set the zombieDetected flag in the blackboard
        myKim.blackboard.Data["zombieDetected"] = zombieFound;
        Debug.Log("sss" + myKim.blackboard.Data["zombieDetected"]);
        return zombieFound ? ReturnState.s_Failure : ReturnState.s_Running;
    }
}
