using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kim : CharacterController
{
    [SerializeField] float ContextRadius;
    public BlackBoard blackboard;

    public override void StartCharacter()
    {
        base.StartCharacter();
        if (blackboard == null)
        {
            blackboard = new BlackBoard(); // Initialize the blackboard
        }
    }

    public override void UpdateCharacter()
    {
        base.UpdateCharacter();
        Zombie closest = GetClosest(GetContextByTag("Zombie"))?.GetComponent<Zombie>();
    }

    GameObject[] GetContextByTag(string aTag)
    {
        Collider[] context = Physics.OverlapSphere(transform.position, ContextRadius);
        List<GameObject> returnContext = new List<GameObject>();
        foreach (Collider c in context)
        {
            if (c.transform.CompareTag(aTag))
            {
                returnContext.Add(c.gameObject);
            }
        }
        return returnContext.ToArray();
    }

    GameObject GetClosest(GameObject[] aContext)
    {
        float dist = float.MaxValue;
        GameObject Closest = null;
        foreach (GameObject z in aContext)
        {
            float curDist = Vector3.Distance(transform.position, z.transform.position);
            if (curDist < dist)
            {
                dist = curDist;
                Closest = z;
            }
        }
        return Closest;
    }
   
}
