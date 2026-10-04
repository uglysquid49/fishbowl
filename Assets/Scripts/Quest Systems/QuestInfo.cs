using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "QuestSystem/QuestInfo")]
public class QuestInfo : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    [field: SerializeField] public string questName { get; private set; }

    [Header("hierarchy mini-game configuration")]
    [Tooltip("unique indentifier of mini-game prefabs")]
    [SerializeField] private string prefabTargetID;
    public string PrefabTargetID => prefabTargetID;

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
