using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Transform weaponAttachPoint;
    [SerializeField] private MonoBehaviour[] weaponsList; // List of all weapons (GunWeapon, ShotgunWeapon, etc.)
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private Sprite iconS;
    [SerializeField] private Sprite iconSwitchWeapon;

    private int currentWeaponIndex = 0;
    private MonoBehaviour currentWeaponScript;
    private IWeapon weapon;

    private float nextFireTime;
    public int CurrentWeaponIndex => currentWeaponIndex;

    void Start()
    {
        if (weaponsList.Length > 0)
        {
            EquipWeapon(currentWeaponIndex);
        }
    }

    void Update()
    {
        if (weapon == null || !enabled || GameManager.I != null && GameManager.I.IsGameOver) return;


        if (Input.GetKeyDown(KeyCode.Q)) // PC test
        {
            SwitchWeapon();
        }

        // Continuous fire
        //if (isHoldingFire && Time.time >= nextFireTime)
        //{
            
        //    Fire();
        //}
    }

    public void OnFireButtonPressed()
    {
        if (weapon == null || Time.time < nextFireTime || !enabled) return;
        Fire();
    }

    private void Fire()
    {
        weapon.Fire();
        nextFireTime = Time.time + weapon.GetFireRate();
    }

    public void SwitchWeapon()
    {
        int nextIndex = (currentWeaponIndex + 1) % weaponsList.Length;
        EquipWeapon(nextIndex);
    }

    private void SetPositionWeapon()
    {
        currentWeaponScript.gameObject.transform.SetParent(weaponAttachPoint);
        currentWeaponScript.gameObject.transform.localPosition = Vector3.zero;
        currentWeaponScript.gameObject.transform.localRotation = Quaternion.identity;
        currentWeaponScript.gameObject.transform.localScale = Vector3.one;
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weaponPrefabs.Length) return;

        // Destroy old weapon instance
        if (currentWeaponScript != null)
        {
            Destroy(currentWeaponScript.gameObject);
        }

        // Instantiate new weapon
        GameObject weaponGO = Instantiate(weaponPrefabs[index], weaponAttachPoint);

        //weaponGO.transform.localPosition = Vector3.zero;
        //weaponGO.transform.localRotation = Quaternion.identity;
        //weaponGO.transform.localScale = Vector3.one;

        currentWeaponScript = weaponGO.GetComponent<MonoBehaviour>();
        weapon = currentWeaponScript as IWeapon;

        currentWeaponIndex = index;
    }

    public void SetWeaponVisible(bool visible)
    {
        if (currentWeaponScript != null)
            currentWeaponScript.gameObject.SetActive(visible);
    }
}
