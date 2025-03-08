using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : Spawner
{
    [Header("Parameters")]
    [Range(0, 1)]
    public float resetBallPosWidth = 0;
    [Range(0, 1)]
    public float resetBallPosHeight = 0;


    public static BallSpawner Instance;
    public Ball activeBall;

    public float maxResetBallPosX { get; private set; }
    public float maxResetBallPosY { get; private set; }
    public float minResetBallPosX { get; private set; }
    public float minResetBallPosY { get; private set; }

    public Vector2 newBallPos { get; private set; }

    private float activeBallDiameter;

    private float[] rangeThesholds = new float[3]; 

    private float currentMinThreshold;
    private float currentMaxThreshold;

    private int currentThresholdIndex = 0;

    private void OnDrawGizmos()
    {
        DrawBallResetArea(resetBallPosWidth, resetBallPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }

    public override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        SetThresholds();
    }
    private void Start()
    {
        newBallPos = activeBall.transform.position;
        DrawBallResetArea(resetBallPosWidth, resetBallPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }
    private void Update()
    {
        ResetBallPos(activeBall.transform.position);
    }

    private void SetThresholds()
    {
        maxResetBallPosY = (camHeight * resetBallPosHeight) + bottomLeftWorldPos.y;
        minResetBallPosY = bottomLeftWorldPos.y + activeBallDiameter;

        maxResetBallPosX = (camWidth * resetBallPosWidth) + bottomLeftWorldPos.x;
        minResetBallPosX = bottomLeftWorldPos.x + activeBallDiameter;

        float totalSpawnDistance = maxResetBallPosX - minResetBallPosX;

        rangeThesholds[0] = minResetBallPosX + ((totalSpawnDistance / 3) * 2); // easy
        rangeThesholds[1] = minResetBallPosX + (totalSpawnDistance / 3); // medium
        rangeThesholds[2] = minResetBallPosX; // hard

        //updates 3 times each lvl
        currentMinThreshold = rangeThesholds[currentThresholdIndex];
        currentMaxThreshold = maxResetBallPosX;
    }
    private void ResetBallPos(Vector2 ballPos)
    {

        Vector2 camWorldPointBottomLeft = SpawnPositionData.bottomLeftWorldPos;
        Vector2 camWorldPointTopRight = SpawnPositionData.topRightWoldPos;

        float camBoundRight = camWorldPointTopRight.x;
        float camBoundBottom = camWorldPointBottomLeft.y;
        float camBoundsLeft = camWorldPointBottomLeft.x;

        if (ballPos.y < camBoundBottom || ballPos.x > camBoundRight)
        {
            GameManager.Instance.attempts++;

            UpdateAttemptsAndRanges();

            float randomX = 0;
            float randomY = 0;
            if (randomX == 0 || randomY == 0)
            {
                randomX = Random.Range(currentMinThreshold, currentMaxThreshold);
                randomY = Random.Range(minResetBallPosY, maxResetBallPosY);
            }
            newBallPos = new Vector2(randomX, randomY);
            activeBall.transform.position = newBallPos;
            activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;

        }
    }

    private void UpdateAttemptsAndRanges()
    {

        if (GameManager.Instance.attempts == 3)
        {
            currentThresholdIndex++;
            if (currentThresholdIndex < rangeThesholds.Length)
            {
                currentMinThreshold = rangeThesholds[currentThresholdIndex];
                currentMaxThreshold = rangeThesholds[currentThresholdIndex - 1];
            }
            else
            {
                currentThresholdIndex = 0;
                currentMinThreshold = rangeThesholds[currentThresholdIndex];
                currentMaxThreshold = maxResetBallPosX;
            }
        }
    }
    private void DrawBallResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        activeBallDiameter = activeBall.spriteRendererBall.sprite.bounds.size.x;
        Vector2 viewportbottomLeft = cam.ViewportToWorldPoint(Vector2.zero);
        
        float minHardLine = activeBallDiameter;
        float minMedLine = resetPosWidth / 3;
        float minEasyLine = minMedLine * 2;

        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(minHardLine, 0);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector2 (0, 0)) + new Vector3(minHardLine, activeBallDiameter);
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(resetPosWidth, 0)) + new Vector3(0, activeBallDiameter);


        Vector2 topMed = cam.ViewportToWorldPoint(new Vector2(minMedLine, resetPosHeight));
        Vector2 bottomMed = cam.ViewportToWorldPoint(new Vector2(minMedLine, 0)) + new Vector3(0, activeBallDiameter);

        Vector2 topEasy = cam.ViewportToWorldPoint(new Vector2(minEasyLine, resetPosHeight));
        Vector2 bottomEasy = cam.ViewportToWorldPoint(new Vector2(minEasyLine, 0)) + new Vector3(0, activeBallDiameter);

        if (Application.isPlaying)
        {
            Debug.DrawLine(bottomLeft, topLeft);
            Debug.DrawLine(topLeft, topRight, color);
            Debug.DrawLine(topRight, bottomRight, color);
            Debug.DrawLine(bottomRight, bottomLeft);

            Debug.DrawLine(topMed, bottomMed);
            Debug.DrawLine(topEasy, bottomEasy);
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
