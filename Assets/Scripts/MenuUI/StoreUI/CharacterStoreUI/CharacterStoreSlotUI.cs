using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStoreSlotUI : MonoBehaviour
{
    [Header("Referencias")]
    public Image characterImage;
    public TextMeshProUGUI name;
    public TextMeshProUGUI cost;
    public Button buyButton;
    public GameObject lockedOverlay;

    CharacterData characterData;
    ShopManager shopManager;

    public void SetUp(CharacterData data, ShopManager manager)
    {
        characterData = data;
        shopManager = manager;

        characterImage.sprite = characterData.icon;
        name.text = characterData.characterName;
        cost.text = characterData.priceGold.ToString();

        lockedOverlay.SetActive(characterData.purchased);

        buyButton.gameObject.SetActive(!characterData.purchased);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyCharacter);
    }

    void BuyCharacter()
    {
        shopManager.TryBuyCharacter(characterData);
    }
}
