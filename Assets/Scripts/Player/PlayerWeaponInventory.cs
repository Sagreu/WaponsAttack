using UnityEngine;

public class PlayerWeaponInventory : MonoBehaviour
{
    public WeaponData[] weaponSlots = new WeaponData[5];

    public bool TryAddWeapon(WeaponData newWeapon)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                weaponSlots[i] = newWeapon;
                Debug.Log("Arma añadida al slot " + i);

                // Aquí luego actualizamos UI
                return true;
            }
        }

        Debug.Log("Inventario lleno");
        return false;
    }
}
