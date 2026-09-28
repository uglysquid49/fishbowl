using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "QuestSystem/QuestInfo")]
public class QuestInfo : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    [field: SerializeField] public string questName { get; private set; }

    [Header("Scene Progression")]
    [Tooltip("Name of scene that needs to be loaded/activated for quest")]
    [SerializeField] private string targetSceneName;
    public string TargetSceneName => targetSceneName;
    
    //[Header("General")]
    //public string displayName;

    //[Header("Requirements")]
    //public QuestInfo[] questPrerequisites;

    //[Header("Steps")]
    //public GameObject[] questStepPrefabs;

    // Ensure the id is always the name of the Scriptable Object asset
    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (string.IsNullOrEmpty(id))
        {
            id = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
