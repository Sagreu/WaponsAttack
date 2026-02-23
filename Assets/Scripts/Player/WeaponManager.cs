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

    [SerializeField] private int maxActiveSlots = 2;

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

        for (int i = 0; i < maxActiveSlots; i++)
        {
            if (equipped[i] == null)
            {
                emptySlots.Add(i);
            }
        }

        if (emptySlots.Count == 0)
        {
            Debug.Log("Slots activos llenos.");
            return false;
        }

        int slotIndex = randomSlot
            ? emptySlots[Random.Range(0, emptySlots.Count)]
            : emptySlots[0];

        OrbitalWeapons newWeapon = Instantiate(weaponsPrefab, slots[slotIndex]);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

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

    public void SetMaxSlots(int amount)
    {
        maxActiveSlots = Mathf.Clamp(amount, 1, slots.Length);
    }
}
