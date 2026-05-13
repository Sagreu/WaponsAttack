using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaeponSlotUI : MonoBehaviour
{
    public Image marco;
    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public Button button;

    WeaponData weapon;
    InventoryManager inventory;

    public void Setup(WeaponData data, InventoryManager manager)
    {
        weapon = data;
        inventory = manager;
        icon.sprite = data.sprite;
        marco.sprite = data.marco;
        name.text = data.weaponName;
        description.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelect);
    }

    private void OnSelect()
    {
        Debug.Log("Seleccionó: " + weapon.weaponName);
        inventory.SelectWeapon(weapon);
    }
}
