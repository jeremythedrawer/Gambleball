using System.Threading.Tasks;
using UnityEngine;

public class ActiveRangeController : MonoBehaviour
{
    [Header("Parameters")]
    [Range(0, 1)]
    public float resetBallPosWidth = 0;
    [Range(0, 1)]
    public float resetBallPosHeight = 0;

    [Header("References")]
    public Ball firstBall;

    public static ActiveRangeController Instance { get; private set; }

    private Ball activeBall;
    private Camera cam;

    private float activeBallDiameter;

    public float maxResetBallPosY { get; private set; }
    public float minResetBallPosY { get; private set; }

    private float posY;

    private float[] rangeThesholds = new float[4];
    public float currentMinThreshold { get; private set; }
    public float currentMaxThreshold { get; private set; }

    private int currentMinThresholdIndex = 2;

    private void OnDrawGizmos()
    {
        DrawBallResetArea(resetBallPosWidth, resetBallPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }

    private void OnValidate()
    {
        SetUp();
    }

    private void Awake()
    {
        SetUp();
    }

    private void Start()
    {
        DrawBallResetArea(resetBallPosWidth, resetBallPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }

    private async void SetUp()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        cam = GameManager.Instance.cam;
        while (GameManager.Instance.activeBall == null) { await Task.Yield(); }
        activeBall = GameManager.Instance.activeBall;

        await SetUpThresholds();
        await SetUpInitTransform();

    }

    private async Task SetUpThresholds()
    {
        while (activeBall == null) { await Task.Yield(); }
        activeBallDiameter = activeBall.spriteRendererBall.bounds.size.x;
        maxResetBallPosY = (ScreenRangeData.camHeight * resetBallPosHeight) + ScreenRangeData.bottomLeftWorldPos.y;
        minResetBallPosY = ScreenRangeData.bottomLeftWorldPos.y + activeBallDiameter;

        rangeThesholds[0] = ScreenRangeData.bottomLeftWorldPos.x + activeBallDiameter; // min
        rangeThesholds[3] = (ScreenRangeData.camWidth * resetBallPosWidth) + ScreenRangeData.bottomLeftWorldPos.x; //max

        float totalSpawnDistance = rangeThesholds[3] - rangeThesholds[0];

        rangeThesholds[1] = rangeThesholds[0] + (totalSpawnDistance / 3); // medium
        rangeThesholds[2] = rangeThesholds[0] + ((totalSpawnDistance / 3) * 2); // easy

        //updates 3 times each lvl
        currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
        currentMaxThreshold = rangeThesholds[3];
    }

    private async Task SetUpInitTransform()
    {
        float initPosX = rangeThesholds[2];
        posY = minResetBallPosY;
        Instance.transform.position = new Vector2(initPosX, posY);

        float scaleX = currentMaxThreshold - currentMinThreshold;
        float scaleY = maxResetBallPosY - minResetBallPosY;
        Instance.transform.localScale = new Vector2(scaleX, scaleY);

        await Task.Yield();
    }

    public void UpdatePos()
    {
        if (GameManager.Instance.attempts == 0)
        {
            currentMinThresholdIndex--;
            currentMaxThreshold = Mathf.Max(currentMaxThreshold, 0);

            if (currentMinThresholdIndex >= 0)
            {
                currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
                currentMaxThreshold = rangeThesholds[currentMinThresholdIndex + 1];
            }
            else
            {
                currentMinThresholdIndex = 2;
                currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
                currentMaxThreshold = rangeThesholds[3];
            }

            float newPosX = currentMinThreshold;
            Instance.transform.position = new Vector2(newPosX, posY);
        }
    }

    private void DrawBallResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        Ball ballToUse;
        ballToUse = activeBall == null ? firstBall : activeBall;
        if (ballToUse == null) { Debug.LogError("ActiveRangeController needs the first ball assigned"); return;  }

        activeBallDiameter = ballToUse.spriteRendererBall.sprite.bounds.size.x;
        Vector2 viewportbottomLeft = cam.ViewportToWorldPoint(Vector2.zero);

        float minHardLine = activeBallDiameter;
        float minMedLine = resetPosWidth / 3;
        float minEasyLine = minMedLine * 2;

        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(minHardLine, 0);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0)) + new Vector3(minHardLine, activeBallDiameter);
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(resetPosWidth, 0)) + new Vector3(0, activeBallDiameter);


        Vector2 topMed = cam.ViewportToWorldPoint(new Vector2(minMedLine, resetPosHeight));
        Vector2 bottomMed = cam.ViewportToWorldPoint(new Vector2(minMedLine, 0)) + new Vector3(0, activeBallDiameter);

        Vector2 topEasy = cam.ViewportToWorldPoint(new Vector2(minEasyLine, resetPosHeight));
        Vector2 bottomEasy = cam.ViewportToWorldPoint(new Vector2(minEasyLine, 0)) + new Vector3(0, activeBallDiameter);

        if (Application.isPlaying)
        {
            Debug.DrawLine(bottomLeft, topLeft, color);
            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
            Debug.DrawLine(bottomRight, bottomLeft, color);

            Debug.DrawLine(topMed, bottomMed, color);
            Debug.DrawLine(topEasy, bottomEasy, color);
        }
        else
        {
            Gizmos.color = color;
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);

            Gizmos.DrawLine(topMed, bottomMed);
            Gizmos.DrawLine(topEasy, bottomEasy);
        }
    }
}
