using UnityEngine;

public class DanoAPlayer : MonoBehaviour
{
    [SerializeField] private int danoPlayer = 5;
    [SerializeField] private HealtPlayers healtPlayers;
    [SerializeField] private EnemyAcha enemyAcha;
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
            if (enemyAcha != null)
            {
                enemyAcha.Die();
            }
        }
    }
}
