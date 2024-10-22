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
                zombieFound = true;
            }
        }

        myKim.blackboard.Data["zombieDetected"] = zombieFound;

        return zombieFound ? ReturnState.s_Failure : ReturnState.s_Running;
    }
}
