using UnityEngine;

public static class GameProgress 
{
    private const string UNLOCKED_LEVEL_KEY = "unlockedKey";
    private const string BEST_LEVEL_KEY = "bestKEY";

    public static int GetUnLockedLevel()
    {
        return PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY, 1);
    }

    public static void UnLockedLevel(int level)
    {
        int currentUnLocked = GetUnLockedLevel();
        if (level > currentUnLocked)
        {
            PlayerPrefs.SetInt(UNLOCKED_LEVEL_KEY, level);
            PlayerPrefs.Save();
        }
    }

    public static int GetBestLevel()
    {
        return PlayerPrefs.GetInt(BEST_LEVEL_KEY, 1);
    }

    public static void BestLevel(int level)
    {
        int bestLevel = GetBestLevel();
        if (level > bestLevel)
        {
            PlayerPrefs.SetInt(BEST_LEVEL_KEY, level);
            PlayerPrefs.Save();
        }
    }

    /*PARA TESTEAR 
    public static void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(UNLOCKED_LEVEL_KEY);
        PlayerPrefs.DeleteKey(BEST_LEVEL_KEY);
    }
    */
}
