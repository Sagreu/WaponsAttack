using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesequiparConfirmacion : MonoBehaviour
{
    public Button desequiparSi;
    public Button desequiparNo;
    public TextMeshProUGUI name;

    InventoryManager inventoryManager;
    int selectedIndex;

    public void SetUp(InventoryManager manager)
    {
        inventoryManager = manager;

        desequiparSi.onClick.AddListener(ConfirmRemove);
        desequiparNo.onClick.AddListener(ClosePopup);
    }

    public void Show(int index, WeaponData data)
    {
        string mns = "¿ DESEAS DESEQUIPAR ? \n\"" + data.weaponName +"\"";
        selectedIndex = index;
        name.text = mns;
        gameObject.SetActive(true);
    }
    void ConfirmRemove()
    {
        inventoryManager.RemoveWeaponAt(selectedIndex);
        gameObject.SetActive(false);
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }

}
