using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForZombieTask : Node
{
    private Kim myKim;
    private List<Vector2Int> thirdAdjacentOffsets;

    public CheckForZombieTask(Kim kim) : base(new List<Node>())
    {
        myKim = kim;
        thirdAdjacentOffsets = GetThirdAdjacentOffsets();
    }

    private List<Vector2Int> GetThirdAdjacentOffsets()
    {
        List<Vector2Int> offsets = new List<Vector2Int>();

        for (int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                if (x == 0 && y == 0) continue; // Skip the current tile
                offsets.Add(new Vector2Int(x, y));
            }
        }

        return offsets;
    }

    public override ReturnState EvaluateState()
    {
        bool zombieFound = false;

        foreach (var offset in thirdAdjacentOffsets)
        {
            Vector3 checkPosition = myKim.transform.position + new Vector3(offset.x, 0, offset.y);
            Collider[] colliders = Physics.OverlapSphere(checkPosition, 0.3f); // Use a smaller radius

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Zombie"))
                {
                    Debug.Log("Zombie detected!");
                    zombieFound = true;
                    break;
                }
            }

            if (zombieFound) break;
        }

        myKim.blackboard.Data["zombieDetected"] = zombieFound;

        return zombieFound ? ReturnState.s_Failure : ReturnState.s_Success;
    }
}
