using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Referencias Player")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject playerPrefab;
    public Button attackButton;
    void Start()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        GameObject prefabToSpawn = playerPrefab;
        if (GameSession.selectedCharacter != null)
        {
            prefabToSpawn = GameSession.selectedCharacter.prefab;
        }
        GameObject player = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        WeaponManager wm = player.GetComponentInChildren<WeaponManager>();

        Debug.Log("WeaponManager: " + wm);

        if (wm != null)
        {
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(wm.LaunchSmartWeapon);
        }
        else
        {
            Debug.LogError("No encontró WeaponManager en el player");
        }
    }
}
