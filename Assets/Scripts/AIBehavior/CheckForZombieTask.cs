using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CheckForZombieTask : Node
{
    private Kim myKim;
    private HashSet<Collider> detectedZombies = new HashSet<Collider>();
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
            if (collider.CompareTag("Zombie") && !detectedZombies.Contains(collider))
            {
                detectedZombies.Add(collider);
                zombieFound = true;
                Debug.Log("New zombie detected: " + collider.name);
                break; 
            }
        }

        // Set the zombieDetected flag in the blackboard
        myKim.blackboard.Data["zombieDetected"] = zombieFound;
        Debug.Log("sss" + myKim.blackboard.Data["zombieDetected"]);
        return zombieFound ? ReturnState.s_Failure : ReturnState.s_Running;
    }
    public void ClearDetectedZombies()
    {
        detectedZombies.Clear();
        Debug.Log("Cleared detected zombies.");
    }
}
