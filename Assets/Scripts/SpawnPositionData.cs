using System.Threading.Tasks;
using UnityEngine;

public abstract class SpawnPositionData : MonoBehaviour
{

    public Camera cam;

    public static Vector2 bottomLeftWorldPos {  get; private set; }
    public static Vector2 topRightWoldPos { get; private set; }

    public static float camHeight { get; private set; }
    public static float camWidth { get; private set; }


    private void OnValidate()
    {
        cam = Camera.main;
    }
    public virtual void Awake()
    {
        SetUpSpawnData();
    }

    private async void SetUpSpawnData()
    {
        camHeight = cam.orthographicSize * 2;
        camWidth = camHeight * cam.aspect;
        while (camHeight == 0 ||  camWidth == 0) { await Task.Yield(); }
        bottomLeftWorldPos = (Vector2)cam.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        topRightWoldPos = (Vector2)cam.transform.position + new Vector2(camWidth / 2, camHeight / 2);
    }
}
