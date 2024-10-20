using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForZombieTask : Node
{
     private Kim myKim;
    private Vector2Int[] thirdAdjacentOffsets = new Vector2Int[]
    {
        new Vector2Int(0, 3),
        new Vector2Int(1, 3),
        new Vector2Int(0, -3),
        new Vector2Int(-1, -3),
        new Vector2Int(3, 0),
        new Vector2Int(3, 1),
        new Vector2Int(-3, 0),
        new Vector2Int(-3, 1),
        new Vector2Int(3, 2),
        new Vector2Int(3, -2),
        new Vector2Int(-3, 2),
        new Vector2Int(-3, -2)
    };
    public CheckForZombieTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
    }
    public override ReturnState EvaluateState()
    {
        // Loop through the third adjacent tiles and check for zombies
        foreach (var offset in thirdAdjacentOffsets)
        {
            Vector3 checkPosition = myKim.transform.position + new Vector3(offset.x, 0, offset.y);
            Collider[] colliders = Physics.OverlapSphere(checkPosition, 0.5f); // Small radius to detect entities

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Zombie"))
                {
                    return ReturnState.s_Failure;  // Fail if any zombie is found
                }
            }
        }

        return ReturnState.s_Success; // Success if no zombies are found
    }
}
