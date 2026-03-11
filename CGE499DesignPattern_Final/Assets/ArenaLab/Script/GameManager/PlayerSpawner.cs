using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Spawn")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private GameObject currentPlayer;

    public void SpawnPlayer()
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        RemoveSceneCamera();

        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
    }

    void RemoveSceneCamera()
    {
        Camera cam = Camera.main;

        if (cam != null)
        {
            Destroy(cam.gameObject);
        }
    }
}
