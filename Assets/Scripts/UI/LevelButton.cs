using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{

    [SerializeField] private Button button;
   // [SerializeField] private GameObject lockIcon;
    private int levelId;
    [Header(" Textos, and starts")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image[] stars;



    public void Setup(int id, bool unlocked, string info, string name, int starCount)
    {
        levelId = id;
        levelText.text = name;
        infoText.text = info;

        for (int i = 0; i < stars.Length; i++) {
            stars[i].gameObject.SetActive(i < starCount);
            }

    }

    public void OnPressed()
    {
        GameData.SelectedLevel = levelId;
        print("Cargar nivel");
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameCore");
    }
}
