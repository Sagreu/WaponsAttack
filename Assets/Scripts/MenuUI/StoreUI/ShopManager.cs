using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //public List<CharacterData> allCharacters;
    public GameDataBase dataBase;

    public Transform content;
    public GameObject characterPrefab;
    [Header("Botones Tiendas")]
    public Button character;
    public Button waepons;
    public Button closeStore;
    public Button closeCharacterStore;
    public GameObject characterPanel;
    //public GameObject waeponsPanel;
    public GameObject showCharactersAdnWaepons;
    [Header("WarningPanel")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    [Header("PanelsWaepons")]
    public GameObject panelWaepons;
    public GameObject gachaPanel;
    public GameObject ResultPanel;
    public Button Btn1;
    public Button Btn10;
    public InventoryManager inventory;

    private void Start()
    {
        LoadShop();
    }

    void LoadShop()
    {

        foreach (var character in dataBase.characters)
        {
            GameObject obj = Instantiate(characterPrefab, content);
            obj.GetComponent<CharacterStoreSlotUI>().SetUp(character, this);
        }
    }

    public void TryBuyCharacter(CharacterData character)
    {
        if (character.purchased)
            return;

        if (MonedaManager.instance.SpendGold(character.priceGold))
        {
            character.purchased = true;
            character.unlocked = true;

            ShowWarning("Comprado: " + character.characterName);

            RefreshShop();
           
            if (inventory != null)
                inventory.RefreshCharacters();
        }
        else
        {
            ShowWarning("Oro Insuficiente");
        }
    }

    void RefreshShop()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        LoadShop();
    }

    public void ShowCharacterPanel()
    {
        showCharactersAdnWaepons.SetActive(false);
        closeStore.gameObject.SetActive(false);
        characterPanel.SetActive(true);
    }
    public void ShowWaeponsPanel()
    {
        panelWaepons.SetActive(true);
    }
    public void ClosePanels()
    {
        characterPanel.SetActive(false);
        panelWaepons.SetActive(false);
        showCharactersAdnWaepons.SetActive(false);
        gameObject.SetActive(false);

    }
    public void CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
        closeStore.gameObject.SetActive(true);

        showCharactersAdnWaepons.SetActive(true);
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

    public void SummonWaepon()
    {

        if (!MonedaManager.instance.SpendRelic(100))
        {
            ShowWarning("No tienes Fragmentos suficientes");
            return;
        }

        WeaponData reward = GetRandomWaepon();

        if (reward.unlocked)
        {
            int refund = GetDuplicateReward(reward);
            MonedaManager.instance.AddGold(refund);
            ShowWarning($"Waepon repited:<color=yellow> +{refund} </color> oro obtenido");
        }
        else
        {
            reward.unlocked = true;
            ShowWarning("Obtuviste: " + reward.weaponName);
        }

    }

    public WeaponData GetRandomWaepon()
    {
        int randomValue = Random.Range(1, 101);
        WaeponRaririty selectRaririty;
        if (randomValue <= 60)
        {
            selectRaririty = WaeponRaririty.Common;
        }
        else if (randomValue <= 85)
        {
            selectRaririty = WaeponRaririty.Rare;
        }
        else if (randomValue <= 95)
        {
            selectRaririty = WaeponRaririty.Epic;
        }
        else if (randomValue <= 99)
        {
            selectRaririty = WaeponRaririty.Legendary;
        }
        else
        {
            selectRaririty = WaeponRaririty.Mythic;
        }

        List<WeaponData> pool = new List<WeaponData>();

        foreach (var waepon in dataBase.weapons)
        {
            if (waepon.obtenibleGacha && waepon.rarity == selectRaririty)
            {
                pool.Add(waepon);
            }
        }
        if (pool.Count == 0)
        {
            print("No Hay Armas de rareza:" + selectRaririty);
            return null;
        }
        return pool[Random.Range(0, pool.Count)];
    }

    public int GetDuplicateReward(WeaponData weaponData)
    {
        switch (weaponData.rarity)
        {
            case WaeponRaririty.Common:
                return 200;
            case WaeponRaririty.Rare:
                return 300;
            case WaeponRaririty.Epic:
                return 400;
            case WaeponRaririty.Legendary:
                return 600;
            case WaeponRaririty.Mythic:
                return 1500;
            default:
                return 0;
        }
    }

    public void SummonTenWaepons()
    {
        if (!MonedaManager.instance.SpendRelic(900))
        {
            ShowWarning("Fondos Insuficientes");
            return;
        }
        for(int i = 0; i <10; i++)
        {
            WeaponData reward = GetRandomWaepon();

            if (reward == null)
                continue;

            if (reward.unlocked)
            {
                int refund = GetDuplicateReward(reward);
                MonedaManager.instance.AddGold(refund);
                Debug.Log($"Tirada {i + 1}: {reward.weaponName}  Raririty:  {reward.rarity.ToString()}: {refund}");
            }
            else
            {
                reward.unlocked = true;
                Debug.Log($"Tirada {i + 1}: {reward.weaponName}  Raririty:  {reward.rarity.ToString()}");
            }
        }
        ShowWarning("Tirada x10 completada");
    }
}
