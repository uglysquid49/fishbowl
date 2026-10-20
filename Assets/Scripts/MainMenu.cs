using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";
    [SerializeField] private string settingSceneName = "";

    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Awake()
    {
        LoadVolume();
        MusicManager.Instance.Playmusic("Game");
    }
    public void PlayGame()
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
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}