using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;

    public GameObject foodPrefab;
    [SerializeField] private RectTransform spawnCanvas;

    public int startingFoodCount = 5;
    public float spawnDelay = 1f;

    [Header("Spawn Settings")]
    public float topSpawnOffset = 50f;
    public float horizontalPadding = 50f;

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
        if (foodPrefab == null || spawnCanvas == null)
        {
            Debug.LogWarning("Assign the Food Prefab and Spawn Canvas in the Inspector.");
            return;
        }

        Rect canvasRect = spawnCanvas.rect;

        float x = Random.Range(
            canvasRect.xMin + horizontalPadding,
            canvasRect.xMax - horizontalPadding
        );

        Vector3 localSpawnPosition = new Vector3(
            x,
            canvasRect.yMax + topSpawnOffset,
            0f
        );

        Vector3 spawnPosition = spawnCanvas.TransformPoint(localSpawnPosition);

        Instantiate(foodPrefab, spawnPosition, Quaternion.identity);
    }
}