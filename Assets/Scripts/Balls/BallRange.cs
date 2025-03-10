using System.Threading.Tasks;
using UnityEngine;

public class BallRange : LevelObjectRange
{
    public static BallRange Instance { get; private set; }

    private Ball activeBall => LevelManager.Instance.activeBall;
    private Camera cam => GameManager.Instance.cam;


    private float[] rangeThesholds = new float[4];
    public float currentMinThreshold { get; private set; }
    public float currentMaxThreshold { get; private set; }

    public int currentMinThresholdIndex { get; private set; } = 2;

    private float posY;
    private float activeBallDiameter;

    private void OnDrawGizmos()
    {
        DrawBallResetArea(resetPosWidth, resetPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }

    private void OnValidate()
    {
        SetUp();
    }

    private void Awake()
    {
        SetUp();
    }

    private async void SetUp()
    {
        if (Instance == null) Instance = this;

        while (LevelManager.Instance.activeBall == null) { await Task.Yield(); }

        await SetUpThresholds();
        await SetUpInitTransform();
    }

    private async Task SetUpThresholds()
    {
        while (activeBall == null) { await Task.Yield(); }

        Vector2 viewportbottomLeft = cam.ViewportToWorldPoint(Vector2.zero);

        float minHardLine = activeBallDiameter;
        float minMedLine = resetPosWidth / 3;
        float minEasyLine = minMedLine * 2;

        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(minHardLine, 0);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0)) + new Vector3(minHardLine, activeBallDiameter);
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));

        Vector2 topMed = cam.ViewportToWorldPoint(new Vector2(minMedLine, resetPosHeight));

        Vector2 topEasy = cam.ViewportToWorldPoint(new Vector2(minEasyLine, resetPosHeight));



        activeBallDiameter = activeBall.spriteRendererBall.bounds.size.x;
        maxResetPosY = topLeft.y;
        minResetPosY = bottomLeft.y;


        rangeThesholds[0] = topLeft.x;
        rangeThesholds[3] = topRight.x;

        rangeThesholds[1] = topMed.x;
        rangeThesholds[2] = topEasy.x;

        //updates 3 times each lvl
        currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
        currentMaxThreshold = rangeThesholds[3];
    }

    private async Task SetUpInitTransform()
    {
        float initPosX = rangeThesholds[2];
        posY = minResetPosY;
        Instance.transform.position = new Vector2(initPosX, posY);

        float scaleX = currentMaxThreshold - currentMinThreshold;
        float scaleY = maxResetPosY - minResetPosY;
        Instance.transform.localScale = new Vector2(scaleX, scaleY);

        await Task.Yield();
    }

    public void UpdatePos()
    {
        if (GameManager.Instance.attempts >= 0 && LevelManager.Instance.activeBasket.scoreTrigger.playerScored)
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

                LevelManager.Instance.SetNextLevel();
            }

            float newPosX = currentMinThreshold;
            Instance.transform.position = new Vector2(newPosX, posY);
            LevelManager.Instance.activeBasket.scoreTrigger.playerScored = false;
            GameManager.Instance.attempts = 3;
        }
        else if (GameManager.Instance.attempts == 0)
        {
            currentMinThresholdIndex = 2;
            currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
            currentMaxThreshold = rangeThesholds[3];

            float newPosX = currentMinThreshold;
            Instance.transform.position = new Vector2(newPosX, posY);
            GameManager.Instance.attempts = 3;

        }
    }

    private void DrawBallResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        activeBallDiameter = LevelManager.Instance.levelData.levels[0].ball.spriteRendererBall.sprite.bounds.size.x;

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

        Gizmos.color = color;
        Gizmos.DrawLine(bottomLeft, topLeft);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);

        Gizmos.DrawLine(topMed, bottomMed);
        Gizmos.DrawLine(topEasy, bottomEasy);
    }
}
