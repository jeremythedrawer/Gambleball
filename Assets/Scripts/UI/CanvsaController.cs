using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasCameraFixer : MonoBehaviour
{
    public Canvas canvas;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                canvas.worldCamera = mainCam;
            }
            else
            {
                Debug.LogWarning("No main camera found to assign to canvas.");
            }
        }
    }
}
