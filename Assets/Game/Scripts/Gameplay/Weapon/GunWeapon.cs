using UnityEngine;

public class GunWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private WeaponFeedbackHandler feedback;
    [SerializeField] private AudioClip audioClipShot;
    [SerializeField] private float volumeShot = 1f;

    public void Fire()
    {
        AudioManager.I.PlaySFX(audioClipShot, firePoint.position, volumeShot);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        feedback?.Play();
    }

    public float GetFireRate() => fireRate;
}
