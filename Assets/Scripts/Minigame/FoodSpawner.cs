
using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;

    public GameObject foodPrefab;
    public int startingFoodCount = 5;
    public float spawnDelay = 1f;

    [Header("Spawn Settings")]
    public float topSpawnY = 5f;
    public float leftSpawnX = -8f;
    public float rightSpawnX = 8f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnStartingFood());
    }

    private IEnumerator SpawnStartingFood()
    {
        for (int i = 0; i < startingFoodCount; i++)
        {
            SpawnFood();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnFood()
    {
        if (foodPrefab == null) return;

        float x = Random.Range(leftSpawnX, rightSpawnX);

        Vector3 spawnPosition = new Vector3(x, topSpawnY, 0f);

        Instantiate(foodPrefab,spawnPosition,Quaternion.identity);
    }
}
