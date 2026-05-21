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

    public Button Btn1;
    public Button Btn10;
    public InventoryManager inventory;

    [Header("Show Waepons")]
    public Image showWaeponImg;
    public TextMeshProUGUI name;
    public GameObject rareEfect;
    public Button ResultPanel;
    public Button closeSkip;
    private ParticleSystem[] systems;
    public ParticleSystem excludeParticle;
    private bool multiPullMode = false;
    public GameObject singlePanel;
    [Header("References Multi Pull")]
    public GameObject multiResultGrid;
    public Transform gridContet;
    public GameObject gridContetPrefab;

    public Button closePanelWaepons;

    bool canNextPull = true;


    //lista para mostrar los resultados del gacha
    public List<WeaponData> summonResult = new List<WeaponData>();
    int indexRevelation = 0;

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

            if (InventoryManager.instance != null)
            {
                InventoryManager.instance.RefreshCharacters();
            }
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
        closePanelWaepons.gameObject.SetActive(false);
        summonResult.Clear();
        multiResultGrid.SetActive(false);
        singlePanel.SetActive(true);
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
        summonResult.Add(reward);
        ShowSinglePull(reward);

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
        multiPullMode = true;
        closePanelWaepons.gameObject.SetActive(false);
        closeSkip.gameObject.SetActive(true);
        if (!MonedaManager.instance.SpendRelic(900))
        {
            ShowWarning("Fondos Insuficientes");
            return;
        }
        summonResult.Clear();
        singlePanel.SetActive(true);
        multiResultGrid.SetActive(false);
        foreach (Transform child in gridContet)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < 10; i++)
        {
            WeaponData reward = GetRandomWaepon();

            if (reward == null)
                continue;

            summonResult.Add(reward);
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
        Debug.Log("Total guardadas: " + summonResult.Count);
        indexRevelation = 0;
        ResultPanel.gameObject.SetActive(true);
        ShowNextPull();
    }

    public void ShowSinglePull(WeaponData weapon)
    {
        canNextPull = false;
        multiPullMode = false;
        closeSkip.gameObject.SetActive(false);
        ResultPanel.gameObject.SetActive(true);
        StartCoroutine(ShowWaeponReveal(weapon));

    }

    IEnumerator ShowWaeponReveal(WeaponData weapon)
    {
        showWaeponImg.sprite = weapon.sprite;
        name.text = weapon.weaponName;

        ChangeEffectColor(weapon.rarity);
        rareEfect.SetActive(true);

        Color imgColor = showWaeponImg.color;
        Color textColor = name.color;

        imgColor.a = 0;
        textColor.a = 0;
        showWaeponImg.color = imgColor;
        name.color = textColor;

        yield return new WaitForSeconds(0.5f);

        float time = 0f;
        float duration = 0.35f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = time / duration;
            imgColor.a = alpha;
            textColor.a = alpha;

            showWaeponImg.color = imgColor;
            name.color = textColor;
            yield return null;
        }

        imgColor.a = 1;
        textColor.a = 1;

        showWaeponImg.color = imgColor;
        name.color = textColor;
        canNextPull = true;
    }
    public void CloseSingleButton()
    {
        ResultPanel.gameObject.SetActive(false);
        closePanelWaepons.gameObject.SetActive(true);
    }
    public void ChangeEffectColor(WaeponRaririty rarity)
    {
        systems = rareEfect.GetComponentsInChildren<ParticleSystem>();
        Color color = Color.white;

        switch (rarity)
        {
            case WaeponRaririty.Common:
                color = Color.gray;
                break;
            case WaeponRaririty.Rare:
                color = Color.blue;
                break;
            case WaeponRaririty.Epic:
                color = new Color(0.6f, 0f, 1f);
                break;
            case WaeponRaririty.Legendary:
                color = Color.red;
                break;
            case WaeponRaririty.Mythic:
                color = Color.yellow;
                break;
            default:
                color = Color.white;
                break;

        }
        foreach (ParticleSystem particle in systems)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play();
            if (particle == excludeParticle)
                continue;
            var main = particle.main;
            main.startColor = color;
        }
    }

    public void ShowNextPull()
    {
        if (!canNextPull)
            return;
        canNextPull = false;

        if (indexRevelation >= summonResult.Count)
        {
            ShowMultiResult();
            return;
        }
        WeaponData weapon = summonResult[indexRevelation];
        StartCoroutine(ShowWaeponReveal(weapon));
        indexRevelation++;
    }
    public void HandResultClick()
    {
        if (!canNextPull)
            return;

        if (multiPullMode)
            ShowNextPull();
        else
            CloseSingleButton();
    }

    public void ShowMultiResult()
    {

        singlePanel.SetActive(false);
        multiResultGrid.SetActive(true);
        foreach (Transform child in gridContet)
        {
            Destroy(child.gameObject);
        }
        foreach (WeaponData weapon in summonResult)
        {
            GameObject obj = Instantiate(gridContetPrefab, gridContet);
            ResultSlotUI slot = obj.GetComponent<ResultSlotUI>();

            slot.Setup(weapon);
        }
        multiPullMode = false;
        closeSkip.gameObject.SetActive(false);
    }
    public void CloseMultiResult()
    {
        ResultPanel.gameObject.SetActive(false);
        closePanelWaepons.gameObject.SetActive(true);
    }
    public void CloseWaeponRelicPanel()
    {
        panelWaepons.SetActive(false);
    }
}
