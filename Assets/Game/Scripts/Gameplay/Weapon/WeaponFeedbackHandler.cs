using UnityEngine;
using Cinemachine;

public class WeaponFeedbackHandler : MonoBehaviour
{
    [Header("Visual Recoil")]
    [SerializeField] private Transform recoilTransform;
    [SerializeField] private Vector3 recoilOffset = new Vector3(0f, 0.05f, -0.1f);
    [SerializeField] private float recoverSpeed = 8f;

    [Header("FX")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private Vector3 originalLocalPos;
    private Vector3 velocity;

    void Start()
    {
        if (recoilTransform != null)
            originalLocalPos = recoilTransform.localPosition;
    }

    void Update()
    {
        if (recoilTransform != null)
        {
            recoilTransform.localPosition = Vector3.SmoothDamp(
                recoilTransform.localPosition,
                originalLocalPos,
                ref velocity,
                1f / recoverSpeed
            );
        }
    }

    public void Play()
    {
        if (recoilTransform != null)
            recoilTransform.localPosition += recoilOffset;

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (impulseSource != null)
            impulseSource.GenerateImpulse();
    }
}
