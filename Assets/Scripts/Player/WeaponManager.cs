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

    [SerializeField] private int maxActiveSlots = 5;

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
        if (GameSession.selectedWeapons != null && GameSession.selectedWeapons.Count > 0)
        {
            LoadFromInventory(GameSession.selectedWeapons);
        }
    }

    public bool AddWeapon(WeaponData data)
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

        int slotIndex = emptySlots[0];

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
    /*
    public void LaunchFirstWeapon()
    {
        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] != null && equipped[i].IsReady)
            {
                equipped[i].Launch();
                break;
            }
        }
    }
    */
    public void LaunchSmartWeapon()
    {
        EnemyAcha[] enemies = FindObjectsOfType<EnemyAcha>();

        if (enemies.Length == 0) return;

        int enemyIndex = 0;

        for (int i = 0; i < equipped.Length; i++)
        {
            if (equipped[i] != null && equipped[i].IsReady)
            {
                equipped[i].LaunchTo(enemies[enemyIndex].transform);

                enemyIndex++;

                if (enemyIndex >= enemies.Length)
                    enemyIndex = 0;

                break;
            }
        }
    }
    public void SpawnStartingWeapons(List<WeaponData> weapons)
    {
        if (weapons == null || weapons.Count == 0) return;

        for (int i = 0; i < weapons.Count; i++)
        {
            if (i >= maxActiveSlots) break;

            AddWeapon(weapons[i]);
        }
    }

    //Carga de armas del inventory al game core
    public void LoadFromInventory(List<WeaponData> inventoryWeapons)
    {
        if(inventoryWeapons == null || inventoryWeapons.Count == 0)
            return;

        SpawnStartingWeapons(inventoryWeapons);
    }
}
