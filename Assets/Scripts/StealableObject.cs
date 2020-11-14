using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealableObject : MonoBehaviour
{

    private readonly static HashSet<StealableObject> Pool = new HashSet<StealableObject>();

    private void OnEnable()
    {
        StealableObject.Pool.Add(this);
    }

    private void OnDisable()
    {
        StealableObject.Pool.Remove(this);
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "fox")
        {
            Destroy(this.gameObject);
        }
    }

    public static StealableObject FindNearest(Vector3 thiefPosition)
    {
        StealableObject result = null;

        float searchDistance = 500f;
        var e = StealableObject.Pool.GetEnumerator();
        while (e.MoveNext())
        {
            float objectDistance = (e.Current.transform.position - thiefPosition).sqrMagnitude;
            if (objectDistance < searchDistance)
            {
                result = e.Current;
                searchDistance = objectDistance;
            }
        }

        return result;
    }
}