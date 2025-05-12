using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick; // Joystick for movement input
    [SerializeField] private float moveSpeed = 5f;    // Movement speed
    [SerializeField] private AudioClip audioFootStep;

    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private float footstepCooldown = 0.5f;
    private float footstepTimer = 0f;

    private Rigidbody rb;
    private Animator animator;

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance, groundLayer);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GameManager.I.IsGameOver) return;

        if (!IsGrounded())
        {
            rb.velocity += Physics.gravity * Time.fixedDeltaTime;
        }

        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        Vector3 targetVelocity = new Vector3(0f, rb.velocity.y, 0f);

        if (direction.magnitude > 0.1f)
        {
            Vector3 move = direction.normalized * moveSpeed;
            targetVelocity = new Vector3(move.x, rb.velocity.y, move.z);

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 0.2f);

            footstepTimer -= Time.fixedDeltaTime;
            if (footstepTimer <= 0f)
            {
                AudioManager.I.PlaySFX(audioFootStep, transform.position, 0.75f);
                footstepTimer = footstepCooldown;
            }
        }

        rb.velocity = targetVelocity;
        // Update animation based on movement
        if (animator != null)
            animator.SetFloat("Speed", direction.magnitude);
    }
}
