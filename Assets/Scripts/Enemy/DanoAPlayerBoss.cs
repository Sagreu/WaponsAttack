using UnityEngine;

public class DanoAPlayerBoss : MonoBehaviour
{
    [SerializeField] private int danoPlayer = 20;
    [SerializeField] private HealtPlayers healtPlayers;
    [SerializeField] private EnemyBoss enemyBoss;
    private void Start()
    {

        healtPlayers = FindAnyObjectByType<HealtPlayers>();
        //enemyAcha = FindAnyObjectByType<EnemyAcha>();
    }
    private void Awake()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            healtPlayers.ResivirDano(danoPlayer);
            if (enemyBoss != null)
            {
                healtPlayers.ResivirDano(danoPlayer);
            }
        }
    }
}