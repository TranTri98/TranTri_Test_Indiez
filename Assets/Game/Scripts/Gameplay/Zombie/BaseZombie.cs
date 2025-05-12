using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject.SpaceFighter;

public abstract class BaseZombie : MonoBehaviour
{
    [SerializeField] protected int health = 3;
    [SerializeField] private GameObject shadow;

    [SerializeField] private AudioClip groanSound;
    [SerializeField] private AudioClip hitSound;
    
    [SerializeField] private float soundRange = 20f;
    [SerializeField] private float minInterval = 4f, maxInterval = 10f;

    public event System.Action OnDied;

    private float nextSoundTime;

    protected Transform target;
    protected Animator animator;

    // Dissolve Zombie
    private SkinnedMeshRenderer[] meshRenderers;
    private MaterialPropertyBlock mpb;
    private float dissolveDuration = 2f;

    private bool isDying = false;
    private bool isDead = false;
    private bool isDissolving = false;

    

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public virtual void TakeDamage(int damage)
    {
        AudioManager.I.PlaySFX(hitSound, transform.position);

        health -= damage;


        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {

        if (isDead) return;
        // Disable NavMeshAgent
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Disable all colliders
        foreach (Collider col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("DieTrigger");
        }
        isDying = true;
        isDead = true;

        OnDied?.Invoke();


    }

    private IEnumerator DissolveCoroutine()
    {
        float time = 0f;

        while (time < dissolveDuration)
        {
            float t = time / dissolveDuration;

            // Apply to all meshes
            foreach (var mesh in meshRenderers)
            {
                mesh.GetPropertyBlock(mpb);
                mpb.SetFloat("_DissolveAmount", t);
                mesh.SetPropertyBlock(mpb);
                
            }

            time += Time.deltaTime;
            yield return null;
        }

        
        Destroy(gameObject);
    }

    public abstract void MoveTowardsTarget();

    protected virtual void Update()
    {
        if (Time.time >= nextSoundTime)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist <= soundRange)
            {
                AudioManager.I.PlaySFX(groanSound, transform.position);
            }

            nextSoundTime = Time.time + Random.Range(minInterval, maxInterval);
        }

        MoveTowardsTarget();

        //Debug.Log($"isDying : {isDying}, !isDissolving : {!isDissolving}, animator : {animator}");
        if (isDying && !isDissolving && animator != null)
        {
            //Debug.Log("Die");
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("ZombieDeath") && state.normalizedTime >= 1f)
            {
                // Turn off shadows
                shadow.SetActive(false);
                StartCoroutine(DissolveCoroutine());
                isDissolving = true;
                this.enabled = false;
            }
        }
    }
}
