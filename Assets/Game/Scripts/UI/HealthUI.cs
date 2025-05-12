using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start()
    {
        playerHealth.OnHealthChanged.AddListener(UpdateHealthText);
    }

    private void UpdateHealthText(int current)
    {
        healthText.text = current.ToString();
    }
}
