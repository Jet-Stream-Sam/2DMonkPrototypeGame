using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageEffectPool : MonoBehaviour
{
    public GameObject effectPrefab;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
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
            GameObject instanceToAdd = Instantiate(effectPrefab);
            
            instanceToAdd.transform.SetParent(transform);
   

            AfterImageEffect effect = instanceToAdd.GetComponent<AfterImageEffect>();
            effect.player = playerTransform;
            effect.effectPool = this;
            effect.playerRenderer = playerSpriteRenderer;
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

    public void UpdatePool(GameObject instance)
    {
        if(effectPrefab != instance)
        {
            effectPrefab = instance;

            Transform[] allObjsInPool = transform.GetComponentsInChildren<Transform>(true);
            int count = 0;
            foreach (Transform t in allObjsInPool)
            {
                if (count != 0)
                {
                    Destroy(t.gameObject);
                }
                count++;
            }
            availableObjects.Clear();

            
            GrowPool();
        }
        
    }
}
