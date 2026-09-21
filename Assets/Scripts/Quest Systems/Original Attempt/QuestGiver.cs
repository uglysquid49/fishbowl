using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    // Referencing our quest giver - fish companion
    public string giverName;
    // Reference to our quest/s

    public void Talk(PlayerInteractions3D player)
    {
        // Where we give the player the quest/s
        Debug.Log("TALKING TO " + giverName);
    }
}
