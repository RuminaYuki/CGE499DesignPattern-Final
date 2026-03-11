using UnityEngine;

public class RandomEnemySpawner : MonoBehaviour
{
    [Header("Enemy Types")]
    [SerializeField] GameObject[] enemies;

    [Header("Spawn Radius")]
    [SerializeField] float radius = 5f;

    [Header("Spawn Setting")]
    [SerializeField] int enemyCount = 6;

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 pos = GetRandomPosition();

            GameObject enemy = enemies[Random.Range(0, enemies.Length)];

            Instantiate(enemy, pos, Quaternion.identity);
        }
    }
    public void ClearEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] == null) continue;

            string cloneName = enemies[i].name + "(Clone)";
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

            foreach (GameObject obj in allObjects)
            {
                if (obj.name == cloneName)
                {
                    Destroy(obj);
                }
            }
        }
    }
    Vector2 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        return (Vector2)transform.position + randomCircle;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}