using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileId = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;

    //slot name
    [SerializeField] private TextMeshProUGUI slotText;

    [SerializeField] private string sceneName = "DayScene3D";

    private bool hasData = false;

    public void SetData(GameData data)
    {
        if (data == null)
        {
            // No data for profileId
            hasData = false;
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
            hasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            if (slotText != null)
            {
                slotText.text = "Slot " + profileId; 
            }
        }
    }

    public void OnSaveSlotClicked()
    {
        if (!hasData)
        {
            Debug.Log("empty slot");
            return;
        }

        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        }

        DataPersistenceManager.Instance.ChangeSelectedProfileId(this.profileId);
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }


    public string GetProfileId()
    {
        return this.profileId.Trim();
    }
}
