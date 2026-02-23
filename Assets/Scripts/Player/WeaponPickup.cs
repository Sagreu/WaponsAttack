using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Header("Datos del arma")]
    [SerializeField] private WeaponData weaponData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Buscamos el Player en la raíz del objeto que colisionó
        Transform root = collision.transform.root;

        if (!root.CompareTag("Player")) return;

        WeaponManager manager = root.GetComponentInChildren<WeaponManager>();

        if (manager == null) return;

        if (manager.AddWeapon(weaponData, true))
        {
            Destroy(gameObject);
        }
    }
}
