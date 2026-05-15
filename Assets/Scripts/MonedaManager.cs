using UnityEngine;

public class MonedaManager : MonoBehaviour
{
    public static MonedaManager instance;
    public int gold;
    public int reliquia;

    const string GOLD_KEY = "PLAYER_GOLD";
    const string RELIC_KEY = "PLAYER_RELIC";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMonedas();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void LoadMonedas()
    {
        gold = PlayerPrefs.GetInt(GOLD_KEY, 0);
        reliquia = PlayerPrefs.GetInt(RELIC_KEY, 0);
    }
    void SaveMoneda()
    {
        PlayerPrefs.SetInt(GOLD_KEY, gold);
        PlayerPrefs.SetInt(RELIC_KEY, reliquia);
        PlayerPrefs.Save();
        
    }

    public void AddGold(int amount)
    {
        gold += amount;
        SaveMoneda();
    }

    public void AddRelic(int amount)
    {
        reliquia += amount;
        SaveMoneda();
    }

    public bool SpendGold(int amount)
    { 
        if(gold < amount)
            return false;

        gold -= amount;
        SaveMoneda();
        return true;
    }

     public bool SpendRelic(int amount)
    {
        if(reliquia < amount)
            return false;

        reliquia -= amount;
        SaveMoneda();
        return true;
    }
}
