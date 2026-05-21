using System;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalWeapons : MonoBehaviour
{
    public event Action<OrbitalWeapons> OnBroken;


    [SerializeField] private WeaponData data;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Hit Cooldown")]
    [SerializeField] private float hitCooldown = 0.25f;

   // private int durability;
  //private float lifeTimer;
    [Header("Para lanzarla")]
    private Transform target;
    private bool launched = false;
    [SerializeField] private float speed = 7f;
    [Header("Para regresar a su lugar")]
    private Transform homeSlot;
    private bool returning = false;

    [Header("Idle Animation")]
    [SerializeField] private float floatAmplitude = 0.1f;
    [SerializeField] private float floatSpeed = 2f;
    private Vector3 startLocalPos;


    // Guarda el último tiempo que golpeó a cada enemigo
    private Dictionary<int, float> lastHitTime = new Dictionary<int, float>();
    public bool IsReady => !launched && !returning;

    public WeaponData Data => data;
   // public int Durability => durability;
    public int DurabilityMax => data != null ? data.durability : 1;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        homeSlot = transform.parent;
        startLocalPos = transform.localPosition;
        ApplyData();
    }

    private void Update()
    {
        if (data == null) return;


        //vuelo hacia enemigo
        if (launched)
        {
            if (target == null)
            {
                launched = false;
                returning = true;
            }
            else
            {
                transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
            }

        }
        //Regreso a slot
        if (returning && homeSlot != null)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                      homeSlot.position,
                                                      speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, homeSlot.position) < 0.05f)
            {
                transform.SetParent(homeSlot);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                returning = false;
                launched = false;
            }
        }
        if (!launched && !returning)
        {
            float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.localPosition = startLocalPos + new Vector3(0f, offsetY, 0f);
        }
    }

    public void SetData(WeaponData newData)
    {
        data = newData;
        ApplyData();
    }

    private void ApplyData()
    {
        if (data == null) return;

       // durability = data.durability;
        //lifeTimer = 0f;

        if (spriteRenderer != null)
            spriteRenderer.sprite = data.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (data == null) return;

        // Pegamos si el objeto tiene vida de enemigo normal o boss
        var enemy = collision.GetComponent<EnemyAcha>();
        var boss = collision.GetComponent<SlimeJefeVida>();

        if (enemy == null && boss == null)
            return;

        int id = collision.GetInstanceID();

        if (lastHitTime.TryGetValue(id, out float lastTime))
        {
            if (Time.time - lastTime < hitCooldown)
                return;
        }

        lastHitTime[id] = Time.time;

        // Aplicar daño
        if (enemy != null)
            enemy.Die();

        if (boss != null)
            boss.resivirDano(data.damage);

        // Reducir durabilidad
       // durability--;
        target = null;
        launched = false;
        returning = true;
        /*
        if (durability <= 0)
        {
            BreakWeapon();
        }
        */
    }

    private void BreakWeapon()
    {
        OnBroken?.Invoke(this);
        Destroy(gameObject);
    }
    public void LaunchTo(Transform enemyTarget)
    {
        if(launched || returning) return;


        transform.SetParent(null);

        target = enemyTarget;
        launched = true;
    }

    private GameObject FindClosestEnemy()
    {
        EnemyAcha[] enemies = FindObjectsOfType<EnemyAcha>();

        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.gameObject;
            }
        }

        return closest;
    }
}
