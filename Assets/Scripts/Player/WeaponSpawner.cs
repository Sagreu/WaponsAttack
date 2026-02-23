using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] weaponPickupPrefabs;
    [SerializeField] private float spawnInterval = 15f;
    //[SerializeField] private float spawnRadius = 5f;
    [SerializeField] private Vector2 areaSize = new Vector2(20, 10);

    private Transform player;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnWeapon), spawnInterval, spawnInterval);
    }

    private void SpawnWeapon()
    {
        if (weaponPickupPrefabs.Length == 0) return;

        float randomX = Random.Range(
            transform.position.x - areaSize.x / 2,
            transform.position.x + areaSize.x / 2);

        float randomY = Random.Range(
            transform.position.y - areaSize.y / 2,
            transform.position.y + areaSize.y / 2);

        Vector2 spawnPos = new Vector2(randomX, randomY);

        int randomIndex = Random.Range(0, weaponPickupPrefabs.Length);

        Instantiate(weaponPickupPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

            Gizmos.DrawWireCube(transform.position, areaSize);
    }
}
