using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public EnemyController _enemyController;
    public int level = 1;
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

    private void Start()
    {
        if (_enemyController == null)
        {
            Debug.LogError("EnemyController NO asignado en WaveManager");
            return;
        }
        StartCoroutine(StartWaveCoroutine());
    }

    // -------------------------
    // INICIO DE OLEADA
    // -------------------------
    IEnumerator StartWaveCoroutine()
    {
        int state = (int)currentState;
        // 🔹 AQUÍ PUEDES MOSTRAR TEXTO
        // Ejemplo futuro:
        // uiText.text = $"Nivel {level} - {currentState}";
        // waveUi.ShowWaveText($"Nivel {level} - {currentState}", 1.5f);
        waveUi.ShowWaveTitle(level, state);
        waveUi.ShowStatus("PREPÁRATE");
        // uiText.SetActive(true);

        Debug.Log($"⏳ Preparando Nivel {level} - Estado {currentState}");

        // Espera antes de iniciar la oleada
        yield return new WaitForSeconds(delayBetweenWaves);

        // uiText.SetActive(false);
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

        // Spawn progresivo
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
        enemiesRemaining--;
        enemiesAlive--;
        waveUi.UpdateEnemiesLeft(enemiesRemaining);
        Debug.Log("OnEnemyDead" + "enemiesAlive " + enemiesAlive + "enemiesToSpawn " + enemiesToSpawn);

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
        // 🔹 AQUÍ PUEDES MOSTRAR TEXTO DE FIN DE OLEADA
        // Ejemplo:
        // uiText.text = "OLEADA COMPLETADA";
        //waveUi.ShowWaveText("OLEADA COMPLETADA", 1.5f);
        waveUi.ShowStatus("OLEADA TERMINADA");
        // uiText.SetActive(true);

        Debug.Log("✅ Oleada completada");

        yield return new WaitForSeconds(delayBetweenWaves);

        // uiText.SetActive(false);
        waveUi.HideStatus();
        waveUi.HideWaveTitle();


        AdvanceState();
        StartCoroutine(StartWaveCoroutine());
    }

    // -------------------------
    // AVANZAR ESTADO / NIVEL
    // -------------------------
    void AdvanceState()
    {
        if (currentState < WaveState.State3)
        {
            currentState++;
        }
        else
        {
            currentState = WaveState.State1;
            level++;
        }

        UpdateWeaponSlots();
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
        int slots = GetSlotsForLevel(level);
        weaponManager.SetMaxSlots(slots);

        Debug.Log($"🔓 Slots desbloqueados: {slots}");
    }

    int GetSlotsForLevel(int lvl)
    {
        if (lvl < 3) return 2;
        if (lvl < 5) return 3;
        if (lvl < 7) return 4;

        return 5;
    }

}
public enum WaveState
{
    State1, 
    State2, 
    State3 
}
