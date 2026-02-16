using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] slots;

    [Header("Prefabs")]
    [SerializeField] private OrbitalWeapons weaponsPrefab;

    [Header("Starting Weapon")]
    [SerializeField] private WeaponData startingWeapon;

    private OrbitalWeapons[] equipped;

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("WeaponManager: No hay slots asignados.");
            return;
        }

        equipped = new OrbitalWeapons[slots.Length];
    }

    private void Start()
    {
        Debug.Log("WeaponManager START en: " + gameObject.name);

        if (startingWeapon != null)
        {
            Debug.Log("StartingWeapon OK: " + startingWeapon.weaponName);
            AddWeapon(startingWeapon, randomSlot: false);
        }
        else
        {
            Debug.LogError("startingWeapon es NULL");
        }
    }

    public bool AddWeapon(WeaponData data, bool randomSlot = true)
    {
        if (data == null) return false;

        List<int> emptySlots = new List<int>();

        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] == null)
            {
                emptySlots.Add(i);
            }
        }

        if (emptySlots.Count == 0)
        {
            Debug.Log("No hay slots vacíos (ya tienes el máximo).");
            return false;
        }

        int slotIndex;

        if (randomSlot)
            slotIndex = emptySlots[Random.Range(0, emptySlots.Count)];
        else
            slotIndex = emptySlots[0];

        Debug.Log("Instanciando arma en slot: " + slotIndex);

        // Instancia
        OrbitalWeapons newWeapon = Instantiate(weaponsPrefab, slots[slotIndex]);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        newWeapon.transform.localRotation = Quaternion.Euler(0, 0, -40f);
        // Configura datos
        newWeapon.SetData(data);
        newWeapon.OnBroken += HandleWeaponBroken;

        equipped[slotIndex] = newWeapon;

        return true;
    }

    private void HandleWeaponBroken(OrbitalWeapons weapon)
    {
        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] == weapon)
            {
                equipped[i] = null;
                break;
            }
        }
    }

    public int GetEquippedCount()
    {
        int count = 0;

        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] != null)
                count++;
        }

        return count;
    }
}
