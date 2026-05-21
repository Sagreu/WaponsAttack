using UnityEngine;
using UnityEngine.UI;

public class ResultSlotUI : MonoBehaviour
{
  public Image frame;
  public Image icon;

    public void Setup(WeaponData weaponData)
    {
        icon.sprite = weaponData.sprite;
        frame.color = GetColor(weaponData.rarity);
    }

    Color GetColor(WaeponRaririty rarity)
    {
        switch (rarity)
        {
            case WaeponRaririty.Common:
                return Color.gray;
            case WaeponRaririty.Rare:
                return Color.blue;
            case WaeponRaririty.Epic:
                return new Color(0.6f, 0f, 1f);
            case WaeponRaririty.Legendary:
                return Color.red;
            case WaeponRaririty.Mythic:
                return Color.yellow;
            default:
                return Color.white;

        }
    }
}
