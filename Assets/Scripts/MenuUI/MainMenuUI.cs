using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject LevelPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject StorePanel;
    [SerializeField] private GameObject PlayersPanel;



    private void Start()
    {
        AllClousePanelns();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameCore");
    }

    public void OpenLevels()
    {
        AllClousePanelns();
        LevelPanel.SetActive(true);
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

    private void AllClousePanelns()
    {

        LevelPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        StorePanel.SetActive(false);
        PlayersPanel.SetActive(false);
    }
}
