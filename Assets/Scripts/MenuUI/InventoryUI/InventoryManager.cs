using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponData> allWeapons;
    public Transform waeponContent;
    public GameObject weaponSlotPrefabs;

    public List<Image> slotsEquipados;
    public List<WeaponData> weaponsEquipadas = new List<WeaponData>();


    public WeaponInfoPopup infoPanel;

    private void Start()
    {
        LoadWeapons();
    }

    void LoadWeapons()
    {
        foreach (var weapon in allWeapons)
        {
            if (!weapon.unlocked) continue;
            GameObject obj = Instantiate(weaponSlotPrefabs, waeponContent);
            obj.GetComponent<WaeponSlotUI>().Setup(weapon, this);
        }
    }



    public void SelectWeapon(WeaponData weapon)
    {
        Debug.Log("Seleccionó: " + weapon.weaponName);
        infoPanel.ShowWeapon(weapon);
    }

    public void EquiparArma(WeaponData weapon)
    {
        Debug.Log("Intentando equipar: " + weapon.weaponName);
        if (weaponsEquipadas.Contains(weapon))
        {
            return;
        }
        if(weaponsEquipadas.Count >= 5)
        {
            Debug.Log("Slots llenos");
            return;
        }
        weaponsEquipadas.Add(weapon);
        Debug.Log("Equipada. Total: " + weaponsEquipadas.Count);
        RefreshSlots();
    }

    void RefreshSlots()
    {
        for (int i = 0; i < slotsEquipados.Count; i++)
        {
            if (i < weaponsEquipadas.Count)
            {
                slotsEquipados[i].sprite = weaponsEquipadas[i].sprite;
                slotsEquipados[i].enabled = true;
            }
            else
            {
               // slotsEquipados[i].sprite = null;
                //slotsEquipados[i].enabled = false;
            }
        }
    }
}
