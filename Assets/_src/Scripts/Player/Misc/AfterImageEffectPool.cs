using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffectPool : MonoBehaviour
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private int instantiatedImagesCount = 15;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    private void Awake()
    {
        GrowPool();
    }
    private void GrowPool()
    {
        for (int i = 0; i < instantiatedImagesCount; i++)
        {
            var instanceToAdd = Instantiate(effectPrefab);
            
            instanceToAdd.transform.SetParent(transform);
            if (instanceToAdd is GameObject g) g.GetComponent<AfterImageEffect>().effectPool = this;
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
