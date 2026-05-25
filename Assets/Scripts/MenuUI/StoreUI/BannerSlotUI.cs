using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerSlotUI : MonoBehaviour
{
    [Header("UISlotBanner")]
    public Image bannerImage;
    public TextMeshProUGUI bannerNameText;
    public TextMeshProUGUI serieText;

    public WeaponDataBanner weaponDataBanner;
    public ShopManager shopManager;

    public void setUp(WeaponDataBanner data, ShopManager manager)
    {
        weaponDataBanner = data;
        shopManager = manager;

        bannerImage.sprite = weaponDataBanner.bannerImage;
        bannerNameText.text = weaponDataBanner.description;
        serieText.text = weaponDataBanner.serie;
    }

    public void SelectBanner()
    {
        shopManager.SelectBanner(weaponDataBanner);
    }
}
