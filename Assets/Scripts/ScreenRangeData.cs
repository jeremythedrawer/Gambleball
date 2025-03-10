using System.Threading.Tasks;
using UnityEngine;

public static class ScreenRangeData
{
    private static Camera cam;

    public static float camHeight { get; private set; }
    public static float camWidth { get; private set; }

    public static Vector2 bottomLeftWorldPos { get; private set; }
    public static Vector2 topRightWoldPos { get; private set; }


    public static async void SetUpScreenBounds()
    {
        cam = GameManager.Instance.cam;

        while (cam == null) { await Task.Yield(); }
        camHeight = cam.orthographicSize * 2;
        camWidth = camHeight * cam.aspect;
        bottomLeftWorldPos = (Vector2)cam.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        topRightWoldPos = (Vector2)cam.transform.position + new Vector2(camWidth / 2, camHeight / 2);
    }
}
