using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class SaveSlotsMenu : MonoBehaviour
{
    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>(true);
        Debug.Log("SaveSlotsMenu found " + saveSlots.Length + " save slot cards in the hierarchy.");
    }


    public void OnReturnClicked()
    {
        this.DeactivateMenu();
       // MainMenu.ActivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);

        this.isLoadingGame = isLoadingGame;

        // Loads all profiles that exist cleanly from disk
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

            foreach (string diskid in profilesGameData.Keys)
            {
                Debug.Log("found save file");
            }
                

            // Loop each slot then set content
            foreach (SaveSlot saveSlot in saveSlots)
            {
                GameData profileData = null;
                profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
                saveSlot.SetData(profileData);
            if (profileData != null && isLoadingGame)
            {
          //      saveSlot.SetInteractable(false);
            }
            else
            {
             //   saveSlot.SetInteractable(true);
            }
            }
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
