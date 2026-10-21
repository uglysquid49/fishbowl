using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class SaveSlotsMenu : MonoBehaviour
{
    [Header("Return Event Control")]
    [SerializeField] private UnityEvent onReturnPressed;

    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>(true);
        Debug.Log("SaveSlotsMenu found " + saveSlots.Length + " save slot cards in the hierarchy.");
    }


    public void OnReturnClicked()
    {
        if (onReturnPressed != null)
        {
            onReturnPressed.Invoke();
        }
        this.DeactivateMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        // Loads all profiles that exist cleanly from disk
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();
        if (profilesGameData == null || profilesGameData.Count == 0 )
        {
            Debug.Log("data check: 0");
        }
        else
        {
            foreach (string diskid in profilesGameData.Keys)
            {
                Debug.Log("found save file");
            }
                
        }
            // Loop each slot then set content
            foreach (SaveSlot saveSlot in saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
            }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
