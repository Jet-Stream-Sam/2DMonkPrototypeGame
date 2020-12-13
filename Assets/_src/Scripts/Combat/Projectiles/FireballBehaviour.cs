using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class FireballBehaviour : MonoBehaviour
{
    [HideInInspector] public Transform target;
    private Vector3 directionToShoot;
    
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ProjectileHitCheck projectileHitCheck;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Rigidbody2D fireballRigidbody;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private CircleCollider2D circleHitbox;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ParticleSystem particleExplosion;

    [TitleGroup("Fireball", Alignment = TitleAlignments.Centered)]
    
    [SerializeField] private float fireballSpeed = 3;
    [SerializeField] private float timeUntilDestruction = 2f;
    
    [SerializeField] private bool damageOnImpact = false;

    [ShowIf("damageOnImpact")] [SerializeField] private float timeOfImpactDetection = 0.3f;
    
    [ShowIf("damageOnImpact")] [SerializeField] private float explosionRadius;

    private bool hasExploded;


    private void Start()
    {
        if(target == null)
        {
            directionToShoot = (transform.right * transform.localScale.x).normalized;
        }
        else
        {
            directionToShoot = (target.position - transform.position).normalized;
        }
        
        fireballRigidbody.velocity = directionToShoot * fireballSpeed;
        StartCoroutine(ExplodeNaturally());
        projectileHitCheck.OnSucessfulHit += Explode;
    }

    private IEnumerator ExplodeNaturally()
    {
        yield return new WaitForSeconds(timeUntilDestruction);
        Explode(new Vector3(), null);
    }

    private void Explode(Vector3 pos, IDamageable dam)
    {
        if (hasExploded)
            return;

        hasExploded = true;

        projectileHitCheck.OnSucessfulHit -= Explode;
        fireballRigidbody.Sleep();

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.gameObject == gameObject)
                continue;
            if (t.gameObject != particleExplosion.gameObject)
            {
                t.gameObject.SetActive(false);
            }
        }
        particleExplosion.Play();
        if (damageOnImpact)
        {
            StartCoroutine(ExplosionDamage(Mathf.Min(particleExplosion.main.duration, timeOfImpactDetection)));
        }
        else
        {
            circleHitbox.enabled = false;
        }

        StartCoroutine(DestroyCompletely(particleExplosion.main.duration));
    }
    private IEnumerator DestroyCompletely(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(gameObject);
    }

    private IEnumerator ExplosionDamage(float seconds)
    {
        circleHitbox.radius = explosionRadius;

        yield return new WaitForSeconds(seconds);
        circleHitbox.enabled = false;
    }
}
