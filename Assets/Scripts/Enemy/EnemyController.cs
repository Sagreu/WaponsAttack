using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject enemyBoos;
    public GameObject enemyPrefab;
    public int countEnemy = 10;
    public Transform spawnPointsDerechaUno, spawnPointsDerechaDos, spawnPointsHizquierdaUno, spawnPointsHizquierdaDos;
    int cantidadSpawnPoints = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnEnemy(Action<EnemyAcha> onSpawned)
    {
        Vector3 spawn = SelectRandomPosition();
        GameObject enemyObject = Instantiate(enemyPrefab, spawn, Quaternion.identity);


        EnemyAcha enemyAcha = enemyObject.GetComponent<EnemyAcha>();
        if (enemyAcha != null)
        {
            onSpawned?.Invoke(enemyAcha);
        }
    }
    private Vector3 SelectRandomPosition()
    {
        Transform selectTransform = null;
        int randomValue = UnityEngine.Random.Range(0, cantidadSpawnPoints);
        SpawnPoints spawnPoints = (SpawnPoints)randomValue;
        switch (spawnPoints)
        {
            case SpawnPoints.DERECHA_UNO:
                selectTransform = spawnPointsDerechaUno;
                break;
            case SpawnPoints.DERECHA_DOS:
                selectTransform = spawnPointsDerechaDos;
                break;
            case SpawnPoints.HIZQUIERDA_UNO:
                selectTransform = spawnPointsHizquierdaUno;
                break;
            case SpawnPoints.HIZQUIERDA_DOS:
                selectTransform = spawnPointsHizquierdaDos;
                break;
            default:
                selectTransform = spawnPointsHizquierdaDos;
                break;
        }
        return selectTransform.position + (Vector3)UnityEngine.Random.insideUnitCircle;


    }

    public void SpawnBoos(Action<SlimeJefeVida> onSpawned)
    {
        Vector3 spawn = SelectRandomPosition();
        GameObject enemyBoosObject = Instantiate(enemyBoos, spawn, Quaternion.identity);

        SlimeJefeVida slimeJefeVida = enemyBoosObject.GetComponent<SlimeJefeVida>();
        if (slimeJefeVida != null)
        {
            onSpawned?.Invoke(slimeJefeVida);
        }
    }
}
public enum SpawnPoints
{
    HIZQUIERDA_UNO = 0,
    HIZQUIERDA_DOS = 1,
    DERECHA_UNO = 2,
    DERECHA_DOS = 3,
}
