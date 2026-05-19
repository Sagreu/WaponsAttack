using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    //public List<WeaponData> allWeapons;
    //public GameDataBase dataBase;
    public Transform waeponContent;
    public GameObject weaponSlotPrefabs;

    public List<Image> slotsEquipados;
    public List<WeaponData> weaponsEquipadas = new List<WeaponData>();


    public WeaponInfoPopup infoPanel;
    public DesequiparConfirmacion desequiparPopup;
    public List<Button> botonesSlots;

    [Header("Data Characters")]
    //public List<CharacterData> allCharacters;
    public GameDataBase dataBase;
    public Transform content;
    public GameObject characterSlotPrefab;
    public Image centerCharacterImage;
    public TextMeshProUGUI centerName;
    public TextMeshProUGUI centerRol;
    public TextMeshProUGUI centerGeneret;
    public TextMeshProUGUI centerLore;
    [Header("Botones")]
    public Button btnCerrar;
    [Header("WarningPanel")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    [Header("Jugables")]
    public CharacterData selectedCharacter;
    public GameObject panelInventory;

    private void Start()
    {
        LoadWeapons();
        LoadCharacters();
        desequiparPopup.SetUp(this);
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void LoadWeapons()
    {
        foreach (var weapon in dataBase.weapons)
        {
            if (!weapon.unlocked) continue;
            GameObject obj = Instantiate(weaponSlotPrefabs, waeponContent);
            obj.GetComponent<WaeponSlotUI>().Setup(weapon, this);
        }
    }



    public void SelectWeapon(WeaponData weapon)
    {
        Debug.Log("Seleccionó: " + weapon.weaponName);
        infoPanel.ShowWeapon(weapon);
    }

    public void EquiparArma(WeaponData weapon)
    {
        Debug.Log("Intentando equipar: " + weapon.weaponName);
        if (weaponsEquipadas.Contains(weapon))
        {
            ShowWarning("Esa arma ya está equipada");
            return;
        }
        int bestLevel = GameProgress.GetBestLevel();
        print("bestLevel: " + bestLevel);
        int maxSlot = SlotRules.GetSlotForLevel(bestLevel);

        if(weaponsEquipadas.Count >= maxSlot)
        {
            ShowWarning("No Hay Espacio Para Equipar Mas Armas");
            return;
        }
        weaponsEquipadas.Add(weapon);
        Debug.Log("Equipada. Total: " + weaponsEquipadas.Count);
        RefreshSlots();
    }

    void RefreshSlots()
    {
        for (int i = 0; i < slotsEquipados.Count; i++)
        {
            int index = i;

            botonesSlots[i].onClick.RemoveAllListeners();

            if (i < weaponsEquipadas.Count)
            {
                slotsEquipados[i].sprite = weaponsEquipadas[i].sprite;
                slotsEquipados[i].enabled = true;

                botonesSlots[i].onClick.AddListener(() =>
                {
                    desequiparPopup.Show(index, weaponsEquipadas[index]);
                });
            }
            else
            {
                slotsEquipados[i].sprite = null;
                slotsEquipados[i].enabled = false;
            }
        }
    }


    public void RemoveWeaponAt(int index)
    {
        if (index < 0 || index >= weaponsEquipadas.Count)
            return;

        Debug.Log("Quitando: " + weaponsEquipadas[index].weaponName);

        weaponsEquipadas.RemoveAt(index);

        RefreshSlots();
    }

    void LoadCharacters()
    {
        foreach (var character in dataBase.characters)
        {
            if (!character.unlocked)
                continue;
            GameObject obj = Instantiate(characterSlotPrefab, content);
            obj.GetComponent<CharacterSlotUI>().SetCharacter(character, this);
        }
    }

    public void SelectCharacter(CharacterData character)
    {
        if (!character.unlocked)
        {
            ShowWarning("Personaje Bloqueado");
            return;
        }
        selectedCharacter = character;
        string a = "<color=yellow> ROL: </color> " + character.role;
        string b = "<color=yellow>GENERO: </color>: " + character.gender;
        string c = "<color=yellow>LORE:\n </color>" + character.lore;
        centerCharacterImage.sprite = character.portrait;
        centerName.text = character.characterName;
        centerGeneret.text = b;
        centerRol.text = a;
        centerLore.text = c;
    }

    public void Close()
    {
        panelInventory.SetActive(false);
    }

    public void ShowWarning(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowWarningCorutine(message));
    }

    IEnumerator ShowWarningCorutine(string message)
    {
        warningText.text = message;

        warningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);

        warningPanel.SetActive(false);
    }
    public void RefreshCharacters()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        LoadCharacters();
    }
}
