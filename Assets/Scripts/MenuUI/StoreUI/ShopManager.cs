using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using NUnit.Framework;
using TMPro;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameDataBase dataBase;
    public Transform content;
    public GameObject characterPrefab;
    [Header("Botones Tiendas")]
    public Button character;
    public Button waepons;
    public Button closeCharacterStore;
    [Header("WarningPanel")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    [Header("PanelsWaepons")]
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
    public Image iconElemental;
    public Image ui1;
    public Image ui2;
    public Image ui3;
    public Image rareImg;
    public TextMeshProUGUI bannerNameText;


    [Header("References Multi Pull")]
    public GameObject multiResultGrid;
    public Transform gridContet;
    public GameObject gridContetPrefab;
    public Button closePanelWaepons;
    bool canNextPull = true;
    public List<WeaponData> summonResult = new List<WeaponData>();
    int indexRevelation = 0;

    [Header("Global Pools")]
    [SerializeField] private List<WeaponData> commonWeapons;
    [SerializeField] private List<WeaponData> rareWeapons;
    [SerializeField] private List<WeaponData> epicWeapons;
    [Header("Banner System")]
    [SerializeField] private List<WeaponDataBanner> allWeaponDataBanner;
    [SerializeField] private Transform bannerContent;
    [SerializeField] GameObject bannerPrefab;
    [SerializeField] private WeaponDataBanner currentBanner;
    [SerializeField] WeaponDataBanner defaultBanner;
    [Header("Current Banner UI")]
    [SerializeField] private Image currentBannerImage;
    [SerializeField] private TextMeshProUGUI currentBannerName;
    [SerializeField] private TextMeshProUGUI currentBannerSerie;
    [SerializeField] private TextMeshProUGUI currentBannerDescription;
    [SerializeField] private TextMeshProUGUI currentBannerInitialDescription;
    [Header("BANNER DETAILS REFERENCES")]
    [SerializeField] private Button infoDetails;
    [SerializeField] private Button closeDetails;
    [SerializeField] private GameObject detailsBannerPanel;
    [SerializeField] private Image bannerImageDetails;
    [SerializeField] private TextMeshProUGUI nameBannerDetails;
    [SerializeField] private TextMeshProUGUI serieBannerDetails;
    [SerializeField] private TextMeshProUGUI loreDetails;
    [SerializeField] private Transform bannerDetailContect;
    [SerializeField] private GameObject weaponBannerDetails;
    [Header("Animacion")]
    [SerializeField] private SummonAnimationController summonAnimation;

    private void Start()
    {
        LoadShop();
        LoadBanners();
        SelectBanner(defaultBanner);

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


    public void ShowWarning(string message)
    {
        //StopAllCoroutines();
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
        StartCoroutine(SummonWaeponRoutine());
    }
    private IEnumerator SummonWaeponRoutine()
    {
        string warningMessage = "";
        closePanelWaepons.gameObject.SetActive(false);
        summonResult.Clear();
        multiResultGrid.SetActive(false);
        singlePanel.SetActive(true);

        if (!MonedaManager.instance.SpendRelic(100))
        {
            ShowWarning("No tienes Fragmentos suficientes");
            yield break;
        }

        WeaponData reward = GetRandomWaepon();

        if (reward.unlocked)
        {
            int refund = GetDuplicateReward(reward);
            MonedaManager.instance.AddGold(refund);
            warningMessage =
         $"Waepon repited:<color=yellow> +{refund} </color> oro obtenido";
        }
        else
        {
            reward.unlocked = true;
            warningMessage = "Obtuviste: " + reward.weaponName;
        }

        summonResult.Add(reward);

        yield return StartCoroutine(summonAnimation.PlaySingle(reward.rarity));

        Debug.Log("Terminó SummonAnimation, ahora muestro resultado");

        ShowSinglePull(reward);
       
        if (!string.IsNullOrEmpty(warningMessage))
        {
            ShowWarning(warningMessage);
        }
    }

    public WeaponData GetRandomWaepon()
    {
        if (currentBanner == null)
        {
            Debug.LogError("No hay banner seleccionado");
            return null;
        }

        int randomValue = Random.Range(1, 101);

        WaeponRaririty selectRaririty;

        if (randomValue <= 60)
            selectRaririty = WaeponRaririty.Common;

        else if (randomValue <= 85)
            selectRaririty = WaeponRaririty.Rare;

        else if (randomValue <= 95)
            selectRaririty = WaeponRaririty.Epic;

        else if (randomValue <= 99)
            selectRaririty = WaeponRaririty.Legendary;

        else
            selectRaririty = WaeponRaririty.Mythic;


        List<WeaponData> pool = GetPoolFromBanner(selectRaririty);

        if (pool == null || pool.Count == 0)
        {
            Debug.LogWarning("Pool vacío: " + selectRaririty);
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
        StartCoroutine(SummonTenWaeponsRoutine());
    }
    private IEnumerator SummonTenWaeponsRoutine()
    {
        closePanelWaepons.gameObject.SetActive(false);

        summonResult.Clear();
        indexRevelation = 0;
        canNextPull = false;
        multiPullMode = false;

        if (!MonedaManager.instance.SpendRelic(900))
        {
            ShowWarning("No tienes Fragmentos suficientes");
            yield break;
        }

        WaeponRaririty[] rarities = new WaeponRaririty[10];

        for (int i = 0; i < 10; i++)
        {
            WeaponData reward = GetRandomWaepon();

            summonResult.Add(reward);
            rarities[i] = reward.rarity;

            if (reward.unlocked)
            {
                int refund = GetDuplicateReward(reward);
                MonedaManager.instance.AddGold(refund);
            }
            else
            {
                reward.unlocked = true;
            }
        }

        yield return StartCoroutine(summonAnimation.PlayTen(rarities));

        ResultPanel.gameObject.SetActive(true);
        singlePanel.SetActive(true);
        multiResultGrid.SetActive(false);
        closeSkip.gameObject.SetActive(true);

        multiPullMode = true;
        indexRevelation = 0;
        canNextPull = true;

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
        iconElemental.sprite = weapon.elementIcon;
        if ("Agua".Equals(weapon.elemental))
        {
            ui1.gameObject.SetActive(false);
            ui3.gameObject.SetActive(false);
            ui2.gameObject.SetActive(true);
            ui2.sprite = weapon.UI;
        }
        else if ("Viento".Equals(weapon.elemental) || "Fuego".Equals(weapon.elemental))
        {

            ui2.gameObject.SetActive(false);
            ui3.gameObject.SetActive(false);
            ui1.gameObject.SetActive(true);
            ui1.sprite = weapon.UI;
        }
        else if ("None".Equals(weapon.elemental))
        {
            ui1.gameObject.SetActive(false);
            ui2.gameObject.SetActive(false);
            ui3.gameObject.SetActive(true);
            ui3.sprite = weapon.UI;
        }
        bannerNameText.text = currentBanner.bannerName;
        rareImg.sprite = weapon.raresaImg;
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
        indexRevelation++;

        StartCoroutine(ShowWaeponReveal(weapon));
    }
    public void HandResultClick()
    {
        Debug.Log($"Click -> canNextPull:{canNextPull} multiPullMode:{multiPullMode} index:{indexRevelation}");
        if (!canNextPull)
            return;

        if (multiPullMode)
            ShowNextPull();
        else
            CloseSingleButton();
    }

    public void ShowMultiResult()
    {
        Debug.Log("ShowMultiResult ejecutado");

        singlePanel.SetActive(false);
        ResultPanel.gameObject.SetActive(true);
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

        singlePanel.SetActive(true);
        multiResultGrid.SetActive(false);
        closeSkip.gameObject.SetActive(false);

        summonResult.Clear();
        indexRevelation = 0;
        canNextPull = true;
        multiPullMode = false;
    }

    void LoadBanners()
    {
        foreach (var banner in allWeaponDataBanner)
        {
            GameObject obj = Instantiate(bannerPrefab, bannerContent);

            obj.GetComponent<BannerSlotUI>().setUp(banner, this);
        }
    }

    public void SelectBanner(WeaponDataBanner banner)
    {
        currentBanner = banner;

        if (currentBanner.Id == 1)
        {
            currentBannerImage.sprite = currentBanner.bannerImage;
            currentBannerName.text = string.Empty;
            currentBannerDescription.gameObject.SetActive(false);
            currentBannerInitialDescription.gameObject.SetActive(true);
            currentBannerInitialDescription.text = currentBanner.description;
        }
        else
        {
            currentBannerInitialDescription.gameObject.SetActive(false);
            currentBannerDescription.gameObject.SetActive(true);
            currentBannerImage.sprite = currentBanner.bannerImage;

            currentBannerName.color = new Color(0.96f, 0.77f, 0.26f);

            currentBannerName.text = currentBanner.bannerName;
            //currentBannerSerie.text = currentBanner.serie;

            currentBannerDescription.text = currentBanner.description;
        }
            
        

    }

    private List<WeaponData> GetPoolFromBanner(WaeponRaririty rarity)
    {
        switch (rarity)
        {
            case WaeponRaririty.Common:
                return commonWeapons;

            case WaeponRaririty.Rare:
                return rareWeapons;

            case WaeponRaririty.Epic:
                return epicWeapons;

            case WaeponRaririty.Legendary:
                return currentBanner.legendaryWeapons;

            case WaeponRaririty.Mythic:
                return currentBanner.mythicWeapons;

            default:
                return null;
        }
    }
    /*Muestra el pannel de detalles*/
    public void ShowBannerDetails()
    {
        print("Details" + currentBanner);
        if (currentBanner == null) return;
        closePanelWaepons.gameObject.SetActive(false);
        detailsBannerPanel.gameObject.SetActive(true);

        nameBannerDetails.text = currentBanner.bannerName;

        serieBannerDetails.text = "<color=white>Serie: </color> " + currentBanner.serie;
        // NUEVO
        serieBannerDetails.color = currentBanner.serieColor;
        loreDetails.text = currentBanner.lore;
        bannerImageDetails.sprite = currentBanner.bannerFondo;

        foreach (Transform child in bannerDetailContect)
            Destroy(child.gameObject);


        AddWeaponsToDetail(currentBanner.legendaryWeapons);
        AddWeaponsToDetail(currentBanner.mythicWeapons);
        
    }

    private void AddWeaponsToDetail(List<WeaponData> weapons)
    {
        foreach (var weapon in weapons)
        {
            GameObject obj = Instantiate(weaponBannerDetails, bannerDetailContect);
            obj.GetComponent<DetailsBannerWeapons>().SetUp(weapon);
        }
    }

    public void CloseDetails()
    {
        closePanelWaepons.gameObject.SetActive(true );
        detailsBannerPanel.gameObject.SetActive(false);
    }
}
