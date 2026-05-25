using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LevelConteiner;
    [SerializeField] private GameObject SettingsPanel;

    [SerializeField] private GameObject characterAndWaeponsPanel;
    [Header("PlayersPanel")]
    [SerializeField] private GameObject PlayersPanel;
    [Header("ScrollView")]
    [SerializeField] private GameObject ScrolView;
    [SerializeField] private GameObject worldPanel;
    [SerializeField] private FadeTransition fadeTransition;

    [Header("REFERENCIAL STORE PANELS")]
    [SerializeField] private GameObject StorePanel;
    public GameObject characterPanel;
    public GameObject panelWaepons;
    public GameObject showCharactersAdnWaepons;
    public Button closeStore;



    private void Start()
    {
        AllClousePanelns();
    }

    public void PlayGame()
    {

        SceneManager.LoadScene("GameCore");
    }

    public void levelConteiner()
    {
        AllClousePanelns();
        LevelConteiner.SetActive(true);
    }

    public void OpenSettings()
    {
        AllClousePanelns();
        SettingsPanel.SetActive(true);
    }

    public void OpenStore()
    {
        AllClousePanelns();
        StorePanel.SetActive(true);
        characterAndWaeponsPanel.SetActive(true);
        closeStore.gameObject.SetActive(true);

    }
    //vamos aqui
    public void OpenPlayer()
    {
        //AllClousePanelns();
        if (worldPanel.active == true)
        {
            worldPanel.SetActive(false);
        }
        PlayersPanel.SetActive(true);
    }
    public void OpenZonesView()
    {
        fadeTransition.fadeToZonas();
        StartCoroutine(Esperar());
    }

    public void CloseZonesView()
    {
        //fadeTransition.fadeToZonas();
        // StartCoroutine(Esperar());
        worldPanel.SetActive(false);
        fadeTransition.fadeToHome();


    }


    private void AllClousePanelns()
    {
        SettingsPanel.SetActive(false);
        StorePanel.SetActive(false);
        //PlayersPanel.SetActive(false);
    }
    IEnumerator Esperar()
    {
        float esperar = 0.5f;

        yield return new WaitForSeconds(esperar);
        worldPanel.SetActive(true);
        ScrolView.SetActive(true);
    }
    IEnumerator CerrarTodo()
    {
        float esperar = 0.5f;

        yield return new WaitForSeconds(esperar);
        worldPanel.SetActive(false);
        ScrolView.SetActive(false);
        SettingsPanel.SetActive(false);
        StorePanel.SetActive(false);
        PlayersPanel.SetActive(false);
        LevelConteiner.SetActive(false);
    }
    public void Cerrar()
    {
        fadeTransition.fadeToHome();
        StartCoroutine(CerrarTodo());

    }

    public void ClosePanelsStore()
    {
        characterPanel.SetActive(false);
        panelWaepons.SetActive(false);
        showCharactersAdnWaepons.SetActive(false);
        StorePanel.SetActive(false);
        closeStore.gameObject.SetActive(false);

    }

    public void CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
        closeStore.gameObject.SetActive(true);

        showCharactersAdnWaepons.SetActive(true);
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

    public void CloseWaeponRelicPanel()
    {
        panelWaepons.SetActive(false);
    }
}
