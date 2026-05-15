using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rolText;
    public Button button;

    CharacterData characterData;
    InventoryManager inventoryManager;

    public void SetCharacter(CharacterData data, InventoryManager manager)
    {
        characterData = data;
        inventoryManager = manager;

        icon.sprite = characterData.icon;
        nameText.text = characterData.characterName;
        rolText.text = characterData.role;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelect);

    }

    void OnSelect()
    {
        inventoryManager.SelectCharacter(characterData);
    }
}
