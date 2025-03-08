using UnityEngine;

public class CamData : MonoBehaviour
{
    [Header("Parameters")]
    [Range(0, 1)]
    public float resetBallPosWidth = 0;
    [Range(0, 1)]
    public float resetBallPosHeight = 0;

    public static CamData Instance { get; private set; }
    public Camera cam {  get; private set; }

    public Vector2 bottomLeftWorldPos {  get; private set; }
    public Vector2 topRightWoldPos { get; private set; }

    public float maxResetBallPosX { get; private set; }
    public float maxResetBallPosY { get; private set; }

    public float camHeight { get; private set; }
    public float camWidth { get; private set; }
    private void OnDrawGizmos()
    {
        DrawBallResetArea(cam, resetBallPosWidth, resetBallPosHeight, Color.yellow);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cam = Camera.main;

        camHeight = cam.orthographicSize * 2;
        camWidth = camHeight * cam.aspect;

        bottomLeftWorldPos = (Vector2)cam.transform.position - new Vector2(camWidth / 2, camHeight / 2);
        topRightWoldPos = (Vector2)cam.transform.position + new Vector2(camWidth / 2, camHeight / 2);

        maxResetBallPosX = (camWidth * resetBallPosWidth) + bottomLeftWorldPos.x;
        maxResetBallPosY = (camHeight * resetBallPosHeight) + bottomLeftWorldPos.y;
    }

    private void Start()
    {
        DrawBallResetArea(cam, resetBallPosWidth, resetBallPosHeight, Color.yellow);   
    }

    private void DrawBallResetArea(Camera cam, float resetPosWidth, float resetPosHeight, Color color)
    {
        cam = Camera.main;
        Vector2 resetPosArea = new Vector2(resetPosWidth, resetPosHeight);
        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosArea.y));
        Vector2 topRight = cam.ViewportToWorldPoint(resetPosArea);
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(resetPosArea.x, 0));

        if (Application.isPlaying)
        {
            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
        }
        else
        {
            Gizmos.color = color;
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
        }
    }
}
