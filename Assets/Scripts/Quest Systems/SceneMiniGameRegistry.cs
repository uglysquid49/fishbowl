using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// We need this line to access URP specific camera functions
using UnityEngine.Rendering.Universal;

public class SceneMiniGameRegistry : MonoBehaviour
{
    public static SceneMiniGameRegistry instance { get; private set; }

    [System.Serializable]
    public struct MiniGameSceneMapping
    {
        public string prefabIdentifierID;
        public string actualSceneName;
    }

    [Header("Scene Build Mapping Registry")]
    [SerializeField] private List<MiniGameSceneMapping> miniGameScenes = new List<MiniGameSceneMapping>();

    private Dictionary<string, string> sceneMap = new Dictionary<string, string>();

    private void Awake()
    {
        instance = this;

        foreach (var mapping in miniGameScenes)
        {
            if (!string.IsNullOrEmpty(mapping.actualSceneName))
            {
                sceneMap.Add(mapping.prefabIdentifierID, mapping.actualSceneName);
            }
        }
    }

    public void ToggleMiniGameHierarchy(string identifier, bool activate)
    {
        if (sceneMap.TryGetValue(identifier, out string sceneName))
        {
            if (activate)
            {
                bool isAlreadyLoaded = false;
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    if (SceneManager.GetSceneAt(i).name == sceneName)
                    {
                        isAlreadyLoaded = true;
                        break;
                    }
                }

                if (!isAlreadyLoaded)
                {
                    StartCoroutine(LoadAndLinkCameraStack(sceneName));
                }
            }
            else
            {
                Scene targetScene = SceneManager.GetSceneByName(sceneName);
                if (targetScene.isLoaded)
                {
                    UnlinkCameraFromStack(sceneName);
                    SceneManager.UnloadSceneAsync(sceneName);
                }
            }
        }
    }

    private System.Collections.IEnumerator LoadAndLinkCameraStack(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 2. Find the newly spawned 2D scene camera
        Scene newlyLoadedScene = SceneManager.GetSceneByName(sceneName);
        Camera overlayCamera = null;

        foreach (GameObject rootObj in newlyLoadedScene.GetRootGameObjects())
        {
            Camera cam = rootObj.GetComponentInChildren<Camera>();
            if (cam != null && cam.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay)
            {
                overlayCamera = cam;
                break;
            }
        }

        Camera main3DCamera = Camera.main;

        if (main3DCamera != null && overlayCamera != null)
        {
            // Get the URP camera data controller component
            var baseCameraData = main3DCamera.GetUniversalAdditionalCameraData();

            // Inject the new camera directly into the stack container
            if (!baseCameraData.cameraStack.Contains(overlayCamera))
            {
                baseCameraData.cameraStack.Add(overlayCamera);
                Debug.Log($"[SUCCESS] Programmatically added '{overlayCamera.name}' to the Main Camera Stack!");
            }
        }
        else
        {
            Debug.LogError("Failed to link camera stacking. Main Camera or 2D Overlay Camera could not be found.");
        }
    }

    private void UnlinkCameraFromStack(string sceneName)
    {
        Camera main3DCamera = Camera.main;
        if (main3DCamera == null) return;

        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        var baseCameraData = main3DCamera.GetUniversalAdditionalCameraData();

        foreach (GameObject rootObj in loadedScene.GetRootGameObjects())
        {
            Camera cam = rootObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                baseCameraData.cameraStack.Remove(cam);
                Debug.Log($"Cleanly unlinked '{cam.name}' from Main Camera Stack.");
                break;
            }
        }
    }
}


