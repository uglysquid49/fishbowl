using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage")]
    [SerializeField] private string fileName;

    public static DataPersistenceManager Instance { get; private set; }
    private GameData gameData;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private List<IDataPersistence> dataPersistenceObjects;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("More than one DataPersistenceManager found in the scene! Destroying the duplicate.");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        // FIXED: Match the method name below
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {

        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null)
        {
            Debug.Log("No Data was found. Initializing New Game.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null) return; //dont save if there is nothing to save

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        // TODO: save data to file

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        // FIXED: Uses FindObjectsByType to fetch ALL MonoBehaviours in the scene, 
        // then filters them to find scripts implementing IDataPersistence
        IEnumerable<IDataPersistence> dataPersistenceObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
