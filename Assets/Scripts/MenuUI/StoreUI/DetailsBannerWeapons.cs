using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsBannerWeapons : MonoBehaviour
{
    [SerializeField] private Image iconWeaponDetail;
    [SerializeField] private Image colorRare;
    [SerializeField] private Image frameRare;
    [SerializeField] private Image iconRare;
    [SerializeField] private Graphic marcoBrilloso;
    [SerializeField] private Image elementIcon;
    // [SerializeField] private Image atack1;
    //[SerializeField] private Image atack2;
    //[SerializeField] private Image atack3;
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textRare;
    [SerializeField] private TextMeshProUGUI textAtack1;
    [SerializeField] private TextMeshProUGUI textAtack2;
    [SerializeField] private TextMeshProUGUI textAtack3;

    public void SetUp(WeaponData weapon)
    {
        if (weapon.rarity == WaeponRaririty.Legendary)
        {
            frameRare.color = new Color(0.8f, 0.6f, 0.2f);
            colorRare.color = new Color(1f, 0.6f, 0f);
            string rare = "LEGENDARIA";
            textRare.text = rare;
            marcoBrilloso.color = new Color(1f, 0.7f, 0f);
        }
        else
        {
            frameRare.color = new Color(0.4f, 0f, 0.1f);
            colorRare.color = new Color(0.4f, 0f, 0.1f);
            string rare = "MITICA";
            textRare.text = rare;
            marcoBrilloso.color = new Color(0.9f, 0.2f, 0.1f);
        }

        iconRare.sprite = weapon.iconRare;
        iconWeaponDetail.sprite = weapon.sprite;
        textName.text = weapon.weaponName;
        elementIcon.sprite = weapon.elementIcon;

        string baseAtack = weapon.damage.ToString() + " Ataque";
        textAtack1.color = new Color(1f, 1f, 1f);
        textAtack1.text = baseAtack;

        string elementalDagame = "+" + weapon.elementalDagame.ToString()+" Daño de Fuego";
        textAtack2.color = new Color(1f, 0.541f, 0f);
        textAtack2.text = elementalDagame;

        string velocityAtack = "+"+weapon.velocityAtack.ToString()+ " Velocidad";
        textAtack3.color = new Color(0.486f, 1f, 0.486f);
        textAtack3.text = velocityAtack;

    }

}
