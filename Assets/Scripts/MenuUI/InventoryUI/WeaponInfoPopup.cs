using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoPopup : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI history;
    public TextMeshProUGUI rareza;
    public Button BtnEquipar;
    public Button BtnCerrar;
    WeaponData selectedWeapon;
    public InventoryManager inventoryManager;

    private void Start()
    {
        BtnCerrar.onClick.AddListener(ClosePopup);
        BtnEquipar.onClick.AddListener(OnEquip);
    }

    public void ShowWeapon(WeaponData weapon)
    {
        gameObject.SetActive(true);
        selectedWeapon = weapon;
        string a;
        if (weapon.lifeTime == 0)
        {
            a = "Permanente";
        }
        else
        {
            a = weapon.lifeTime.ToString() + " s.";
        }
        icon.sprite = weapon.sprite;
        name.text = weapon.weaponName;
        stats.text = "Daño: " + weapon.damage + "\n" +
              "Durabilidad: " + weapon.durability + "\n" +
              "Tiempo De vida: " + a;
        history.text = weapon.history;
        rareza.text = weapon.rareza;
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    void OnEquip()
    {
        inventoryManager.EquiparArma(selectedWeapon);
    }


}
