using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ZonaPanel : MonoBehaviour
{
    [SerializeField] private int zoneId;
    [SerializeField] private int levelsPerZone = 5;


    [SerializeField] private LevelMenuManager levelMenuManager;

    [SerializeField] private Button button;
    //[SerializeField] private GameObject lockIcon;
    [SerializeField] private TextMeshProUGUI zoneText;

    private void Start()
    {
        CheckIfUnloecked();
    }

    public void CheckIfUnloecked()
    {
        int unLockedLevel = GameProgress.GetUnLockedLevel();
        int lastLevelPreviousZone = (zoneId - 1) * levelsPerZone;

        bool unlocked = unLockedLevel > lastLevelPreviousZone;
        button.interactable = unlocked;
       // lockIcon.SetActive(!unlocked);
    }

    public void OnZonePressed()
    {
        levelMenuManager.OpenZone(zoneId);
    }
}
