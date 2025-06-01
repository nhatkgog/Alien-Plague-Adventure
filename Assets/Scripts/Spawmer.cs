using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] public int numberOfEnemies;
    [SerializeField] public float spawnRange;
    [SerializeField] public float spawnDelay;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = transform.position + new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
