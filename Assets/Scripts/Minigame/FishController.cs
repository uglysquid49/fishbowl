using UnityEngine;

public class FishController : MonoBehaviour
{
    public int sizeLevel = 1;
    public float growthAmount = 0.15f;

    public void EatFish(FishFood food)
    {
        if (food == null) return;

        if (sizeLevel >= food.sizeLevel)
        {
            sizeLevel++;

            transform.localScale *= (1f + growthAmount);

            if (FeedingGameManager.Instance != null)
            {
                FeedingGameManager.Instance.AddScore(food.points);
            }

            Destroy(food.gameObject);
        }
        else
        {
            if (FeedingGameManager.Instance != null)
            {
                FeedingGameManager.Instance.GameOver();
            }
        }
    }
}
