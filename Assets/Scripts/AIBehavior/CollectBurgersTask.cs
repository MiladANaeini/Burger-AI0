using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectBurgersTask : Node
{
    private Kim myKim;
    private List<GameObject> burgers;
    private GameObject currentTargetBurger;

    public CollectBurgersTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
        burgers = new List<GameObject>();
        currentTargetBurger = null;
    }

    public override ReturnState EvaluateState()
    {
        // If there are no burgers left, return success
        if (burgers.Count == 0)
        {
            FindBurgers();
            if (burgers.Count == 0)
            {
                Debug.Log("No burgers left, moving to finish tile.");
                return ReturnState.s_Success;
            }
        }

        // Set the current target burger
        if (currentTargetBurger == null && burgers.Count > 0)
        {
            currentTargetBurger = burgers[0]; // Pick the closest burger
            myKim.blackboard.Data["target"] = currentTargetBurger.transform.position;
            Debug.Log("New target set: " + currentTargetBurger.name);
        }

        return ReturnState.s_Running;
    }

    // Find burgers in the scene
    private void FindBurgers()
    {
        burgers = GameObject.FindGameObjectsWithTag("Burger")
            .OrderBy(burger => Vector3.Distance(myKim.transform.position, burger.transform.position))
            .ToList();

        if (burgers.Count > 0)
        {
            Debug.Log($"Found {burgers.Count} burgers.");
        }
    }
}



