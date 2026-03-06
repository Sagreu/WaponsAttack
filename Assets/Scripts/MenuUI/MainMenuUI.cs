using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LevelConteiner;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject StorePanel;
    [SerializeField] private GameObject PlayersPanel;
    [Header("ScrollView")]
    [SerializeField] private GameObject ScrolView;
    [SerializeField] private GameObject worldPanel;
    [SerializeField] private FadeTransition fadeTransition;
    


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
    }

    public void OpenPlayer()
    {
        AllClousePanelns();
        PlayersPanel.SetActive(true);
    }
    public void OpenZonesView()
    {
        fadeTransition.fadeToZonas();
        StartCoroutine(Esperar());
    }


    private void AllClousePanelns()
    {
        SettingsPanel.SetActive(false);
        StorePanel.SetActive(false);
        PlayersPanel.SetActive(false);
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
}
