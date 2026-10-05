using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }
    public QuestEvents questEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        questEvents = new QuestEvents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger is activated!");
            SceneManager.LoadScene("MiniGameScene", LoadSceneMode.Additive);
            Camera.main.gameObject.SetActive(false); // Disabling the 3d camera
        }
    }
}
