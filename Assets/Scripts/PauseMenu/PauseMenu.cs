using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenu;
    public GameObject settingMenu;

    [Header("Map Control")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Input References")]
    [SerializeField] private InputActionReference pauseActionReference;

    [Header("Scene Changes")]
    [SerializeField] private string quitSceneName = "";


    private bool isPaused = false;
    private bool pausePressedThisFrame = false; 

    private void OnEnable()
    {
        if (pauseActionReference != null && pauseActionReference.action != null)
        {
            pauseActionReference.action.Enable();
            pauseActionReference.action.performed += OnPauseTriggered;
        }
    }

    private void OnDisable()
    {
        if (pauseActionReference != null && pauseActionReference.action != null)
        {
            pauseActionReference.action.performed -= OnPauseTriggered;
            pauseActionReference.action.Disable();
        }
    }

    private void OnPauseTriggered(InputAction.CallbackContext context)
    {
        pausePressedThisFrame = true;
    }

    private void Update()
    {
        if (pausePressedThisFrame)
        {
            pausePressedThisFrame = false;

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
    }

    public void ResumeGame()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        }

        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);

        Time.timeScale = 1;
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    public void PauseGame()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        }

        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);

        Time.timeScale = 0;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerInput != null)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
    }

    public void OpenSettings()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        }
        pauseMenu.SetActive(false);
        settingMenu.SetActive(true);
        Debug.Log("settingsOpen");
    }

    public void CloseSettings()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        }
        settingMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound3D("MainMenuButton", transform.position);
        }
        SceneManager.LoadScene(quitSceneName);
    }
}
