using UnityEngine;

public class FishFood : MonoBehaviour
{
    public int sizeLevel = 1;
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