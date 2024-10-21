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
        bool zombieFound = false;

        // Use Physics.OverlapSphere to check for zombies within the ContextRadius
        Collider[] colliders = Physics.OverlapSphere(myKim.transform.position, myKim.ContextRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Zombie"))
            {
                zombieFound = true;
            }
        }

        // Store the result in the blackboard
        myKim.blackboard.Data["zombieDetected"] = zombieFound;

        return zombieFound ? ReturnState.s_Failure : ReturnState.s_Success;
    }
}
