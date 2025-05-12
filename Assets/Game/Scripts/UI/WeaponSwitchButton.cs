using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchButton : MonoBehaviour
{
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite[] weaponIcons; // icon theo index prefab

    private void Start()
    {
        UpdateIcon();
    }

    public void OnSwitchPressed()
    {
        weaponSlot.SwitchWeapon();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        int index = weaponSlot.CurrentWeaponIndex;

        if (index >= 0 && index < weaponIcons.Length)
            iconImage.sprite = weaponIcons[index];
    }
}
