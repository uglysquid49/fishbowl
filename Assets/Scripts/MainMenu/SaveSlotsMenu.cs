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
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnReturnClicked()
    {
        if (onReturnPressed != null)
        {
            onReturnPressed.Invoke();
        }
        this.DeactiveMenu();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

        // Loads all profiles that exist cleanly from disk
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        // Loop each slot then set content
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
        }
    }

    public void DeactiveMenu()
    {
        this.gameObject.SetActive(false);
    }
}
