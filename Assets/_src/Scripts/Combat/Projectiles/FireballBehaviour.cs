using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballBehaviour : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [SerializeField] private Rigidbody2D fireballRigidbody;
    private Vector3 directionToShoot;
    [SerializeField] private float fireballSpeed = 3;
    [SerializeField] private float timeUntilDestruction = 2f;

    private void Start()
    {
        directionToShoot = (target.position - transform.position).normalized;
        fireballRigidbody.velocity = directionToShoot * fireballSpeed;
        Destroy(gameObject, timeUntilDestruction);
    }

}
