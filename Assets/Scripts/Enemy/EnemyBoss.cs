using System;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public float seepEnemy = 4f;
    public Rigidbody2D rigidbody2D;
    public Transform playerTransform;
    public bool stopped = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        PlayerAcha playerAcha = FindAnyObjectByType<PlayerAcha>();

        if (playerAcha != null)
        {
            playerTransform = playerAcha.transform;
        }
        else
        {
            stopped = true;
            Debug.Log("GAME OVER");
        }


    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void Move()
    {
        if (stopped == true || playerTransform == null)
        {
            rigidbody2D.linearVelocity = Vector3.zero;
            return;
        }
        else
        {
            Vector3 direccionPlayer = playerTransform.position - transform.position;
            rigidbody2D.linearVelocity = direccionPlayer.normalized * seepEnemy;
        }
    }

    
}
