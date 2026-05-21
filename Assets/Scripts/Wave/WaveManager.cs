using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public EnemyController _enemyController;
    public int level;
    public WaveState currentState;

    private int enemiesAlive;
    private int enemiesToSpawn;
    private int totalEnemies;
    private int enemiesRemaining;

    [Header("Timing")]
    public float delayBetweenWaves = 2f;
    public float spawnInterval = 0.5f;

    [Header("Canvas")]
    public WaveUi waveUi;

    public SlimeJefeVida slimeJefeVida;

    [SerializeField] private WeaponManager weaponManager;
    private const string UNLOCKED_LEVEL_KEY = "unlockedKey";
    private const string BEST_LEVEL_KEY = "bestKEY";
    private bool levelCompleted = false;
    private IEnumerator Start()
    {
        if (_enemyController == null)
        {
            Debug.LogError("EnemyController NO asignado en WaveManager");
            yield break;
        }
       
        yield return null;
        weaponManager = FindObjectOfType<WeaponManager>();

        Debug.Log("WeaponManager encontrado: " + weaponManager);
        StartCoroutine(StartWaveCoroutine());
    }
    private void Awake()
    {
        level = GameData.SelectedLevel;
        
        // PARA OBTENER EL SELECCIONADO level = GameData.SelectedLevel;

    }

    // -------------------------
    // INICIO DE OLEADA
    // -------------------------
    IEnumerator StartWaveCoroutine()
    {
        int state = (int)currentState;
        waveUi.ShowWaveTitle(level, state);
        waveUi.ShowStatus("PREPÁRATE");
        Debug.Log($"⏳ Preparando Nivel {level} - Estado {currentState}");

        // Espera antes de iniciar la oleada
        yield return new WaitForSeconds(delayBetweenWaves);
        waveUi.HidenTitle();
        totalEnemies = GetEnemyCount();
        if (currentState == WaveState.State3)
        {
            totalEnemies += 1;
        }
        enemiesToSpawn = totalEnemies;
        enemiesAlive = 0;
        enemiesRemaining = totalEnemies;
        waveUi.UpdateEnemiesLeft(enemiesRemaining);

        // Spawn progre
        yield return StartCoroutine(SpawnEnemiesGradually());
    }

    // -------------------------
    // SPAWN PROGRESIVO
    // -------------------------
    IEnumerator SpawnEnemiesGradually()
    {
        waveUi.HideStatus();
        if (currentState == WaveState.State3)
        {
            _enemyController.SpawnBoos(RegisterBoss);
            enemiesToSpawn--;
            enemiesAlive++;
            waveUi.UpdateEnemiesLeft(enemiesRemaining);
        }
        while (enemiesToSpawn > 0)
        {
            _enemyController.SpawnEnemy(RegisterEnemy);


            enemiesToSpawn--;   // 🔑 CLAVE
            enemiesAlive++;

            yield return new WaitForSeconds(spawnInterval);
        }

        Debug.Log($"🟢 Nivel {level} - Estado {currentState} - Total {totalEnemies}");
    }

    // -------------------------
    // REGISTRO DEL ENEMIGO
    // -------------------------
    void RegisterEnemy(EnemyAcha enemy)
    {
        enemy.OnDead += OnEnemyDead;
    }
    void RegisterBoss(SlimeJefeVida slimeJefeVida)
    {
        slimeJefeVida.OnDead += OnEnemyDead;
    }

    // -------------------------
    // CUANDO UN ENEMIGO MUERE
    // -------------------------
    void OnEnemyDead()
    {
        if (levelCompleted) return;
        print("levelCompleted" +  levelCompleted);
        enemiesRemaining--;
        enemiesAlive--;
        waveUi.UpdateEnemiesLeft(enemiesRemaining);

        if (enemiesAlive <= 0 && enemiesToSpawn <= 0)
        {
            StartCoroutine(EndWaveCoroutine());
        }
    }

    // -------------------------
    // FIN DE OLEADA
    // -------------------------
    IEnumerator EndWaveCoroutine()
    {
        if (levelCompleted) yield break;
        waveUi.ShowStatus("OLEADA TERMINADA");

        Debug.Log("✅ Oleada completada");

        yield return new WaitForSeconds(delayBetweenWaves);
        waveUi.HideStatus();
        waveUi.HideWaveTitle();
        AdvanceState();

    }

    // -------------------------
    // AVANZAR ESTADO / NIVEL
    // -------------------------
    void AdvanceState()
    {
        if (currentState < WaveState.State3)
        {
            currentState++;
            UpdateWeaponSlots();
            StartCoroutine(StartWaveCoroutine());
        }
        else
        {
            //currentState = WaveState.State1;
            //level++;
            CompletedLevel();
        }


    }

    // -------------------------
    // CÁLCULO DE ENEMIGOS
    // -------------------------
    int GetEnemyCount()
    {
        int baseEnemies = 5;
        int stateMultiplier = (int)currentState + 1;

        return baseEnemies * level * stateMultiplier;
    }

    void UpdateWeaponSlots()
    {
        int slots = SlotRules.GetSlotForLevel(level); 
        weaponManager.SetMaxSlots(slots);

        Debug.Log($"🔓 Slots desbloqueados: {slots}");
    }

    void CompletedLevel()
    {
        if (levelCompleted) return;
        levelCompleted = true;
        StopAllCoroutines();
        print("Nivel {level} completado");
        GameProgress.UnLockedLevel(level + 1);
        GameProgress.BestLevel(level);

        // Mostrar UI de victoria (opcional)
        waveUi.ShowStatus("¡NIVEL COMPLETADO!");

        // Terminar partida después de unos segundos
        StartCoroutine(EndGameAfterDelay());
    }

    IEnumerator EndGameAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        // Regresar al menú de niveles
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
public enum WaveState
{
    State1, 
    State2, 
    State3 
}
