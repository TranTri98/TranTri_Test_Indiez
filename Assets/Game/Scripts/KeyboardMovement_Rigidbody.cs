using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeyboardMovement_Rigidbody : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform modelToRotate; // Optional: rotate character to move direction

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent physics from rotating player
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveInput = new Vector3(h, 0f, v).normalized;

        if (moveInput.magnitude > 0.1f && modelToRotate != null)
        {
            modelToRotate.forward = moveInput;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
