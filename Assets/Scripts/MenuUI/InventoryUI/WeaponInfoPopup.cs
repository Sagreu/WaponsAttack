using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
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

        string a = "<color=yellow>" + weapon.weaponName + "</color> ";
        string b = "<color=yellow> Raresa: </color>" + weapon.rarity.ToString();
        icon.sprite = weapon.sprite;
        name.text = a;
        stats.text = "<color=yellow> Daño: </color> " + weapon.damage + "\n";
        history.text = weapon.history;
        rareza.text = b;
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
