using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private GameObject hitObstaclesEffectPrefab;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = transform.position;
        Quaternion vfxRotation = Quaternion.LookRotation(-transform.forward);
        if (other.CompareTag("Zombie") )
        {
            // Check if object has any BaseZombie component
            BaseZombie zombie = other.GetComponent<BaseZombie>();
            if (zombie != null)
            {
                
                zombie.TakeDamage(damage);

                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, hitPoint, vfxRotation);
                    Destroy(gameObject);
                }

            }
        }
        else if (other.CompareTag("Obstacles"))
        {
            if(hitObstaclesEffectPrefab != null)
            {
                Instantiate(hitObstaclesEffectPrefab, hitPoint, vfxRotation);
                Destroy(gameObject);
            }

        }
        
    }
}
