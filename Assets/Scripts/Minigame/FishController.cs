using UnityEngine;

public class FishController : MonoBehaviour
{
    public float sizeLevel = 0.25f;
    public float growthAmount = 0.15f;

    public int foodEaten = 0;
    public int foodToWin = 10;

    private bool hasWon = false;

    public void EatFish(FishFood food)
    {
        if (food == null || hasWon) return;

        // Only eat food that is small enough.
        if (sizeLevel >= food.sizeLevel)
        {
            sizeLevel++;

            // Make the fish grow.
            transform.localScale *= (1f + growthAmount);

            // Count the food eaten.
            foodEaten++;

            // Add points to the score.
            if (FeedingGameManager.Instance != null)
            {
                FeedingGameManager.Instance.AddScore(food.points);
            }

            // Remove the eaten food.
            Destroy(food.gameObject);

            // Check whether the player has won.
            if (foodEaten >= foodToWin)
            {
                hasWon = true;

                if (FeedingGameManager.Instance != null)
                {
                    FeedingGameManager.Instance.WinGame();
                }

                return;
            }

            // Spawn replacement food if the player hasn't won.
            if (FoodSpawner.Instance != null)
            {
                FoodSpawner.Instance.SpawnFood();
            }
        }
    }
}
