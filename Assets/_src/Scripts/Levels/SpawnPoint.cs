using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPointTransform;

    public void MoveToSpawn(Transform t)
    {
        t.transform.position = spawnPointTransform.position;
    }
}
