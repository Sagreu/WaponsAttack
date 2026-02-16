using UnityEngine;

public class DanoABoss : MonoBehaviour
{
    [SerializeField] private int danoABoss = 10;
    //[SerializeField] private SlimeJefeVida slimeJefeVida;
    //[SerializeField] private EnemyBoss enemyBoss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        SlimeJefeVida slimeJefeVida = collision.GetComponent<SlimeJefeVida>();

        Debug.Log(collision.name);

        if (slimeJefeVida != null)
        {
            if (collision.CompareTag("Boss"))
            {
                slimeJefeVida.resivirDano(danoABoss);
            }
        }
    }
}
