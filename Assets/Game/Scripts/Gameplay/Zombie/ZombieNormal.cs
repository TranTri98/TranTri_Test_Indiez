using UnityEngine;
using UnityEngine.AI;
using Zenject.SpaceFighter;

public class ZombieNormal : BaseZombie
{
    private NavMeshAgent agent;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float rotationSpeed = 500f;
    [SerializeField] private float angleThreshold = 5f;
    [SerializeField] private int damgeDeal = 1;
    [SerializeField] private AudioClip attackSound;

    private bool isAttacting = false;

    private float lastAttackTime;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();

    }

    public override void MoveTowardsTarget()
    {
        if (target == null || !agent.isOnNavMesh || GameManager.I.IsGameOver) return;

        float distance = Vector3.Distance(transform.position, target.position);

        animator.SetBool("isAttacking", isAttacting);

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            isAttacting = false;
            if (animator != null)
            {
                animator.SetFloat("Speed", agent.velocity.magnitude);
                animator.ResetTrigger("AttackTrigger");
            }
        }
        else
        {
            isAttacting = true;
            // Rotate toward player before attacking
            Vector3 direction = (target.position - transform.position).normalized;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);

            if (angleDiff < angleThreshold && isAttacting)
            {
                agent.isStopped = true;

                if (animator != null )
                {
                    
                   
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
                        Time.time - lastAttackTime > attackCooldown)
                    {
                        DealDamage();
                        animator.SetTrigger("AttackTrigger");
                        lastAttackTime = Time.time;
                    }
                    animator.SetFloat("Speed", 0f);
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(transform.position);
                animator.SetFloat("Speed", 0.1f);
            }
        }
    }

    // Called by animation event in "Attack" animation
    public void DealDamage()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
        {
            PlayerHealth player = target.GetComponent<PlayerHealth>();
            if (player != null)
            {
                AudioManager.I.PlaySFX(attackSound, transform.position);
                player.TakeDamage(damgeDeal);
            }
        }
    }
}
