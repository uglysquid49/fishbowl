using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenu;
    public GameObject settingMenu;

    [Header("Action Map Control")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Input References")]
    [SerializeField] private InputActionReference pauseActionReference;

    [Header("Scene Changes")]
    [SerializeField] private string settingSceneName = "";
    [SerializeField] private string quitSceneName = "";

    private bool isPauesd = false;
    private bool isPaused;

    private void OnEnable()
    {
        pauseActionReference.action.Enable();
        pauseActionReference.action.performed += OnPauseTriggered;
    }

    private void OnDisable()
    {
        pauseActionReference.action.performed -= OnPauseTriggered;
        pauseActionReference.action.Disable();
    }

    private void OnPauseTriggered(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
            Debug.Log("game has resumed");
        }
        else
        {
            PauseGame();
            Debug.Log("game has paused");
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);

        Time.timeScale = 1;

        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInput != null)
        {
            playerInput.actions.Enable();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);

        Time.timeScale = 0;

        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInput != null)
        {
            playerInput.actions.Disable();
            pauseActionReference.action.Enable();
        }
    }

    public void OpenSettings()
    {
        SoundManager.instance.PlaySound3D("Interaction", transform.position);
        pauseMenu.SetActive(false);
        settingMenu.SetActive(true); //opens setting panel
        Debug.Log("settingsOpen");
    }

    public void CloseSettings()
    {
        SoundManager.instance.PlaySound3D("Interaction", transform.position);
        settingMenu.SetActive(false); 
        pauseMenu.SetActive(true);  
    }

    public void saveButton()
    {
        DataPersistenceManager.Instance.SaveGame();
    }

    public void MainMenu()
    {
        SoundManager.instance.PlaySound3D("Interaction", transform.position);
        SceneManager.LoadScene(quitSceneName);
    }

}
