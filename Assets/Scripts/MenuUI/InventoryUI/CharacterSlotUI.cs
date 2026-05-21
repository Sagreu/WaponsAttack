using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rolText;
    public Button button;
    [Header("MARCOS")]
    public Sprite marcoCharacter;
    public Image marco;
    public Sprite marcoSeleccionCharacter;
    private bool isSelected;
    public GameObject bg;

    CharacterData characterData;
    InventoryManager inventoryManager;


    public void SetCharacter(CharacterData data, InventoryManager manager)
    {
        characterData = data;
        inventoryManager = manager;

        icon.sprite = characterData.icon;
        nameText.text = characterData.characterName;
        rolText.text = characterData.role;
       // bg.sprite = data.portrait;

        SetSelected(false);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnSelect);

    }

    void OnSelect()
    {
        inventoryManager.SelectCharacter(characterData);
        
        //Avisa al manager que slot esta seleccionado
        inventoryManager.UbdateCharacterSelection(this);
    }

    public void SetSelected(bool selected)
    {

        isSelected = selected;
        if (isSelected)
        {
            ColorUtility.TryParseHtmlString("#FFDD00", out Color selectedColor);
            marco.color = selectedColor;
        }
        else
        {
            marco.color = Color.white;
        }
        marco.sprite = isSelected ? marcoSeleccionCharacter : marcoCharacter;
    }
}
