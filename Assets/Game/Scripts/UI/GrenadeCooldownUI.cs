using UnityEngine;
using UnityEngine.UI;

public class GrenadeCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private GrenadeThrower thrower;

    private void Update()
    {
        if (thrower == null || cooldownOverlay == null) return;

        float t = thrower.GetCooldownRemaining01(); //  0 (ready) -> 1 (cooling)
        cooldownOverlay.fillAmount = t;
        cooldownOverlay.enabled = t > 0;
    }
}
