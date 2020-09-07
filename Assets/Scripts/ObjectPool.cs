using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool<T> : 
    MonoBehaviour where T : MonoBehaviour
{
    public T prefab;
    public int poolSize;

    private List<T> availableObjects;
    private List<T> usedObjects;
    UnityEngine.Rendering.ObjectPool<GameObject> foo;

    private void Awake()
    {
        availableObjects = new List<T>(poolSize);
        usedObjects = new List<T>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            var obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            availableObjects.Add(obj);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public T Get()
    {
        int numAvailable = availableObjects.Count;
        if (numAvailable.Equals(0))
        {
            return null;
        }

        T obj = availableObjects[numAvailable - 1];
        availableObjects.RemoveAt(numAvailable - 1);
        usedObjects.Add(obj);
        return obj;

    }

    public void Put(T obj)
    {
        Debug.Assert(usedObjects.Contains(obj));

        usedObjects.Remove(obj);
        availableObjects.Add(obj);

        var pot = obj.transform;
        pot.parent = transform;
        pot.localPosition = Vector3.zero;
        obj.gameObject.SetActive(false);
    }
}
