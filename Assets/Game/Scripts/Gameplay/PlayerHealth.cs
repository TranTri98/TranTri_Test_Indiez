using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private GameObject bloodFX;

    [SerializeField] private int currentHealth = 1;
    
    private bool isDead = false;
    private Animator animator;

    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();

    void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth);
        animator = GetComponent<Animator>();

    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        OnHealthChanged.Invoke(currentHealth);

        if (bloodFX != null)
            Instantiate(bloodFX, transform.position + Vector3.up, Quaternion.identity);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged.Invoke(currentHealth);
    }

    private void Die()
    {
        isDead = true;

        GameManager.I.SetGameOver();             // freeze input & time
        UIManager.I.ShowGameOver();              // fade-in mask

        Debug.Log("Player has died.");
    }

}
