using UnityEngine;

public class FishFood : MonoBehaviour
{
    public float sizeLevel = 0.25f;
    public int points = 10;

    void OnTriggerEnter(Collider other)
    {
        FishController player = other.GetComponentInParent<FishController>();

        if (player != null)
        {
            player.EatFish(this);
        }
    }
}