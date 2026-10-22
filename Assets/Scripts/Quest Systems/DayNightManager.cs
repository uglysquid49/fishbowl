using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayNightManager : MonoBehaviour
{
    [Header("Clock UI")]
    [SerializeField] private Image clockImage;
    [SerializeField] private Sprite[] timeImages;

    [Header("Sun Lighting (Optional)")]
    [SerializeField] private Light sunLight;
    [SerializeField] private float[] sunIntensities;

    [Header("Ambient Lighting (Optional)")]
    [SerializeField] private Color[] ambientColors;

    private int currentTimeIndex = 0;

    private void Start()
    {
        UpdateTimeVisuals();
    }

    public void AdvanceTime()
    {
        // Stop advancing once it reaches the last time stage.
        if (currentTimeIndex >= timeImages.Length - 1)
        {
            return;
        }

        currentTimeIndex++;
        UpdateTimeVisuals();

        Debug.Log("Time advanced to stage: " + currentTimeIndex);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnMinigameSceneChanged;
        SceneManager.sceneUnloaded += OnMinigameSceneChanged;
        UpdateClockVisibility();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMinigameSceneChanged;
        SceneManager.sceneUnloaded -= OnMinigameSceneChanged;
    }

    private void OnMinigameSceneChanged(Scene scene, LoadSceneMode mode)
    {
        UpdateClockVisibility();
    }

    private void OnMinigameSceneChanged(Scene scene)
    {
        UpdateClockVisibility();
    }

    private void UpdateClockVisibility()
    {
        if (clockImage == null)
        {
            return;
        }

        bool minigameIsLoaded =
            SceneManager.GetSceneByName("ClickCircles").isLoaded ||
            SceneManager.GetSceneByName("FeedingFishy").isLoaded ||
            SceneManager.GetSceneByName("QuizBowl").isLoaded;

        clockImage.enabled = !minigameIsLoaded;
    }
    private void UpdateTimeVisuals()
    {
        // Update the clock image.
        if (clockImage != null && timeImages != null && currentTimeIndex < timeImages.Length)
        {
            clockImage.sprite = timeImages[currentTimeIndex];
        }

        // Update the sun's brightness.
        if (sunLight != null && sunIntensities != null && currentTimeIndex < sunIntensities.Length)
        {
            sunLight.intensity = sunIntensities[currentTimeIndex];
        }

        // Update the scene's ambient lighting.
        if (ambientColors != null && currentTimeIndex < ambientColors.Length)
        {
            RenderSettings.ambientLight = ambientColors[currentTimeIndex];
        }
    }
}
