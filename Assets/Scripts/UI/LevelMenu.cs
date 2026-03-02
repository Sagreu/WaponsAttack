using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Transform content;//el de mi content vertical
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private int levelsPerZone = 5;
    [SerializeField]private ScrollRect scrollRect;

    public void GenerateLevels(int zoneId)
    {
        int unlockedLevel = GameProgress.GetUnLockedLevel();

        int maxLevelInZone = zoneId * levelsPerZone;
        int minLevelInZone = maxLevelInZone - levelsPerZone + 1;

        //Se calcula hasta que nivel de esa zona esta desbloqueado

        int nivelDesbloqueado = Mathf.Min(unlockedLevel, maxLevelInZone);

        if(nivelDesbloqueado < minLevelInZone)
            return;

        for (int level = nivelDesbloqueado; level >= minLevelInZone; level--)
        {
            GameObject btn = Instantiate(levelButtonPrefab, content);
            LevelButton levelBtn = btn.GetComponent<LevelButton>();
            levelBtn.Setup(level, true);

        }
        StartCoroutine(MoveScrollToTop());
    }

    IEnumerator MoveScrollToTop()
    {
        yield return null; // espera 1 frame
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }
}
