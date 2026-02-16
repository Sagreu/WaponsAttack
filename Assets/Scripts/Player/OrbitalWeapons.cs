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

    private int durability;
    private float lifeTimer;

    // Guarda el último tiempo que golpeó a cada enemigo
    private Dictionary<int, float> lastHitTime = new Dictionary<int, float>();

    public WeaponData Data => data;
    public int Durability => durability;
    public int DurabilityMax => data != null ? data.durability : 1;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ApplyData();
    }

    private void Update()
    {
        if (data == null) return;

        if (data.lifeTime > 0)
        {
            lifeTimer += Time.deltaTime;

            if (lifeTimer >= data.lifeTime)
            {
                BreakWeapon();
            }
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

        durability = data.durability;
        lifeTimer = 0f;

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
        durability--;

        if (durability <= 0)
        {
            BreakWeapon();
        }
    }

    private void BreakWeapon()
    {
        OnBroken?.Invoke(this);
        Destroy(gameObject);
    }
}
