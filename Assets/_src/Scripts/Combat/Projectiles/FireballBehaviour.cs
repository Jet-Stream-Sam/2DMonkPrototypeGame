using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class FireballBehaviour : MonoBehaviour
{
    private SoundManager soundManager;
    [ReadOnly] public Transform target;
    private Vector2 directionToShoot;
    
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ProjectileHitCheck projectileHitCheck;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Rigidbody2D fireballRigidbody;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private CircleCollider2D circleHitbox;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private ParticleSystem particleExplosion;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private CollectionSounds explosionSound;

    [TitleGroup("Fireball", Alignment = TitleAlignments.Centered)]
    
    [SerializeField] private float fireballSpeed = 3;
    [SerializeField] private float timeUntilDestruction = 2f;
    
    [SerializeField] private bool damageOnImpact = false;

    [ShowIf("damageOnImpact")] [SerializeField] private float timeOfImpactDetection = 0.3f;
    
    [ShowIf("damageOnImpact")] [SerializeField] private float explosionRadius;

    private bool hasExploded;


    private void Start()
    {
        soundManager = SoundManager.Instance;

        if(target == null)
        {
            directionToShoot = (transform.right * transform.localScale.x).normalized;
        }
        else
        {
            directionToShoot = new Vector2(target.position.x - transform.position.x,
                target.position.y - transform.position.y).normalized;

            if (directionToShoot == Vector2.zero)
                directionToShoot = (transform.right * transform.localScale.x).normalized;
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

        if(explosionSound != null)
            explosionSound.PlaySound(soundManager, transform.position);

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
