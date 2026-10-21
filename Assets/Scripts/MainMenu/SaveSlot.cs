using TMPro;
using UnityEngine;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    //slot name
    [SerializeField] private TextMeshProUGUI slotText;


    public void SetData(GameData data)
    {
        if (data == null)
        {
            // No data for profileId
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);

            // Show empty text if needed
            if (slotText != null)
            {
                slotText.text = "Empty";
            }
        }
        else
        {
            // Data exists
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            if (slotText != null)
            {
                slotText.text = "Slot " + profileId; 
            }
        }
    }



    public string GetProfileId()
    {
        return this.profileId;
    }
}
