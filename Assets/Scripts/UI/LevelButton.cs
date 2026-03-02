using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button button;
   // [SerializeField] private GameObject lockIcon;
    private int levelId;

    public void Setup(int id, bool unlocked)
    {
        levelId = id;
        levelText.text = "Nivel" + id;

    }

    public void OnPressed()
    {
        GameData.SelectedLevel = levelId;
        print("Cargar nivel");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameCore");
    }
}
