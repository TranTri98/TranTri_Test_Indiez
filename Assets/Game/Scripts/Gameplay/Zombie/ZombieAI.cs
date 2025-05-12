using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAI : MonoBehaviour
{
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float rotationSpeed = 500f;         // Rotation speed toward player
    [SerializeField] private float angleThreshold = 5f;        // Allowed angle difference before stopping
    [SerializeField] private int health = 3;


    private NavMeshAgent agent;
    private Animator animator;
    private Transform target;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            // Chase the player normally
            agent.isStopped = false;
            agent.SetDestination(target.position);

            animator.SetFloat("Speed", agent.velocity.magnitude);
            animator.ResetTrigger("AttackTrigger");
        }
        else
        {
            // Calculate direction to player
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f;

            // Rotate smoothly toward the player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if facing player within angle threshold
            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

            if (angleDiff < angleThreshold)
            {
                // Stop moving only when facing player
                agent.isStopped = true;
                animator.SetFloat("Speed", 0f);

                // Attack only if not already attacking
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                    Time.time - lastAttackTime > attackCooldown)
                {
                    animator.SetTrigger("AttackTrigger");
                    lastAttackTime = Time.time;
                    // TODO: Deal damage (via animation event)
                }
            }
            else
            {
                // Still turning, keep agent moving slightly to prevent jitter
                agent.isStopped = false;
                agent.SetDestination(transform.position); // Soft stop
                animator.SetFloat("Speed", 0.1f);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: play death animation, dissolve shader, add score...
        Destroy(gameObject);
    }
}
