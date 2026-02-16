using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WeaponManager manager = collision.GetComponentInParent<WeaponManager>();

        if (manager == null) return;

        bool added = manager.AddWeapon(weaponData, randomSlot: true);

        if (added)
        {
            Destroy(gameObject);
        }
    }
}
