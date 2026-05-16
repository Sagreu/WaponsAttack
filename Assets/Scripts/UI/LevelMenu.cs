using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Transform content;//el de mi content vertical
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private int levelsPerZone = 5;
    [SerializeField] private ScrollRect scrollRect;
    //[SerializeField] private List<LevelData> allLevels;
    public GameDataBase dataBase;
    [SerializeField] private Button BtnTMP;
    [SerializeField] private ZonaPanel zonaPanel;

    public void GenerateLevels(int zoneId)
    {
        int unlockedLevel = GameProgress.GetUnLockedLevel();

        foreach (LevelData levelData in dataBase.levels)
        {
            if (levelData.zoneId != zoneId)
                continue;
            if (levelData.levelId > unlockedLevel)
                continue;

            GameObject btn = Instantiate(levelButtonPrefab, content);
            LevelButton levelBtn = btn.GetComponent<LevelButton>();
            //int stars = GameProgress.GetStars(level.levelId);
            int stars = 3;

            //bool unlocked = levelData.levelId <= unlockedLevel;

            levelBtn.Setup(levelData.levelId, true, levelData.description, levelData.levelName, stars);
        }
        StartCoroutine(MoveScrollToTop());
    }

    IEnumerator MoveScrollToTop()
    {
        yield return null; // espera 1 frame
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    public void OnCloseZone()
    {
        BtnTMP.gameObject.SetActive(false);
        ZoneBackgroundController.Instance.RestoredDefaultBackground();
        Destroy(gameObject);
        zonaPanel.ActiveCloseWordlPanel();

    }

    public void SetZonaPanel(ZonaPanel zona)
    {
        zonaPanel = zona;
    }
}
