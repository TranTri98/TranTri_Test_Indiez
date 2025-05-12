using UnityEngine;

public class ShotgunWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float spreadAngle = 15f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private WeaponFeedbackHandler feedback;
    [SerializeField] private AudioClip audioClipShot;
    [SerializeField] private float volumeShot = 1f;
     
    public void Fire()
    {
        AudioManager.I.PlaySFX(audioClipShot, firePoint.position, volumeShot);
        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = Random.Range(-spreadAngle, spreadAngle);
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, angleOffset, 0);
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }

        feedback?.Play();
    }

    public float GetFireRate() => fireRate;
}
