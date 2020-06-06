using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItem
{
    [SerializeField]
    GameObject objectToPool;
    [SerializeField]
    int amountToPool;
    [SerializeField]
    bool shouldExpand;

    public int AmountToPool => amountToPool;

    public GameObject ObjectToPool => objectToPool;

    public bool ShouldExpand => shouldExpand;
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    
    [SerializeField]
    private List<ObjectPoolItem> itemsToPool;

    private List<GameObject> pooledObjects;

    public List<GameObject> PooledObjects => pooledObjects;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.AmountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(item.ObjectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
            {
                return pooledObjects[i];
            }
        }

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (item.ObjectToPool.tag == tag)
            {
                if (item.ShouldExpand)
                {
                    GameObject obj = Instantiate(item.ObjectToPool);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}