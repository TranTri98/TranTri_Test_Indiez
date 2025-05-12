using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float delay = 2f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private int damage = 2;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip audioClipGrenade;

    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    void Explode()
    {
        AudioManager.I.Play2D(audioClipGrenade, 0.7f);
        // Damage zombie in radius
        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var hit in hits)
        {
            BaseZombie zombie = hit.GetComponent<BaseZombie>();
            if (zombie != null)
                zombie.TakeDamage(damage);
        }

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius); // or explosionRadius
    }
}
