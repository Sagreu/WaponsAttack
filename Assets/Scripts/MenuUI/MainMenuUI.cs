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
        worldPanel.SetActive(true);
        ScrolView.SetActive(true);
    }


    private void AllClousePanelns()
    {

        //LevelPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        StorePanel.SetActive(false);
        PlayersPanel.SetActive(false);
    }
}
