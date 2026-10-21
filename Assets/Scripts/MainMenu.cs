using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string settingSceneName = "";

    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (MusicManager.Instance != null)
        {
            LoadVolume();
            MusicManager.Instance.Playmusic("Game");
        }
        else
        {
            Debug.Log("MusicManager Instance is missing from this scene..");
        }
    }

    public void ActivateMenu()
    {
        mainMenuPanel.SetActive(true);
    }

    public void DeactivateMenu()
    {
        mainMenuPanel.SetActive(false);
    }

    public void PlayGame()
    {
        StartCoroutine(PlaySoundAndLoadScene(gameSceneName));
    }
    public void LoadGame()
    {
        SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
      //  saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void OnClickReturn()
     {
     StartCoroutine(PlaySoundAndLoadScene(gameSceneName));
     }

    public void SettingMenu()
    {
        StartCoroutine(PlaySoundAndLoadScene(settingSceneName));
    }

    public void QuitGame()
    {
        SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }
    private IEnumerator PlaySoundAndLoadScene(string sceneName)
    {
        SoundManager.instance.PlaySound3D("MainMenuButon", transform.position);
        yield return new WaitForSecondsRealtime(0.2f);

        SceneManager.LoadScene(sceneName);
    }
    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        SaveVolume();
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        SaveVolume();
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        float savedMusic = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
        }
        UpdateMusicVolume(savedMusic);
        UpdateSoundVolume(savedSFX);
    }


}