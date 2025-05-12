using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwCooldown = 2f;

    private float nextThrowTime;
    private bool hasThrown = false;
    private float lastThrowTime = -999f;


    private WeaponSlot slotWeapon;
    private void Start()
    {
        slotWeapon = GetComponent<WeaponSlot>();
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.G) && Time.time >= nextThrowTime)
        {
             StartThrow();
        }
#endif
    }

    // Called from button or key
    public void StartThrow()
    {
        if (Time.time < nextThrowTime) return;

        if (slotWeapon != null)
        {
            slotWeapon.SetWeaponVisible(false);
            slotWeapon.enabled = false;
        }

        if (animator != null)
            animator.SetTrigger("ThrowGrenade");

        hasThrown = false; // Reset for animation event
        nextThrowTime = Time.time + throwCooldown;
    }

    // Called by animation event at throw moment
    public void ThrowGrenade()
    {
        if (hasThrown) return; // prevent double throw
        hasThrown = true;

        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = throwPoint.forward;
            rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
        }

        lastThrowTime = Time.time;
        nextThrowTime = Time.time + throwCooldown;
    }

    public float GetCooldownRemaining01()
    {
        float t = Time.time - lastThrowTime;
        return Mathf.Clamp01(1f - t / throwCooldown);
    }

    public void ShowWeaponAfterThrow()
    {
        slotWeapon.enabled = true;
        GetComponent<WeaponSlot>()?.SetWeaponVisible(true);
    }

}
