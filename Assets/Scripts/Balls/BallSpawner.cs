using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BallSpawner : Spawner
{
    [Header("Gizmos References")]
    [SerializeField] Camera cam;
    [SerializeField] SpriteRenderer sampleBallSpriteRenderer;

    [Header("Parameters")]
    [Range(0, 1)]
    public float resetPosWidth = 0;
    [Range(0, 1)]
    public float resetPosHeight = 0;

    public static BallSpawner Instance;

    public float maxResetPosY { get; protected set; }
    public float minResetPosY { get; protected set; }

    public int currentMinThresholdIndex { get; set; } = 2;
    public float currentMinThreshold { get; set; }
    public float currentMaxThreshold { get; set; }

    public float[] rangeThesholds { get; set; } = new float[4];

    private Ball activeBall => GameManager.Instance.activeBall;

    public List<Ball> allBalls = new List<Ball>();

    private struct BallRangePoints
    {
        public Vector2 topLeft,     topOneThird,    topTwoThirds,       topRight;
        public Vector2 bottomLeft,  bottomOneThird, bottomTwoThirds,    bottomRight;
    }

    private BallRangePoints ballRangePoints;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        SetUpThresholds();
    }

    private void Start()
    {
        InstantiateLevelDataObjects<Ball>(allBalls, this.transform);
        SetUpInitTransform();
        ResetActiveBallPos();       
    }
    public async void ResetActiveBallPos()
    {
        newPos = GetNewPos( currentMinThreshold, currentMaxThreshold, minResetPosY, maxResetPosY );

        while (activeBall == null) { await Task.Yield(); }

        activeBall.transform.position = newPos;
        activeBall.transform.eulerAngles = Vector2.zero;
        activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void SetUpThresholds()
    {
        ballRangePoints = BallRanges();

        maxResetPosY = ballRangePoints.topLeft.y;
        minResetPosY = ballRangePoints.bottomLeft.y;

        rangeThesholds[0] = ballRangePoints.topLeft.x;
        rangeThesholds[1] = ballRangePoints.topOneThird.x;
        rangeThesholds[2] = ballRangePoints.topTwoThirds.x;
        rangeThesholds[3] = ballRangePoints.topRight.x;

        //updates 3 times each lvl
        currentMinThreshold = rangeThesholds[currentMinThresholdIndex];
        currentMaxThreshold = rangeThesholds[3];
    }

    private void SetUpInitTransform()
    {
        float scaleX = currentMaxThreshold - currentMinThreshold;
        float scaleY = maxResetPosY - minResetPosY;

        transform.position = ballRangePoints.bottomTwoThirds;
        transform.localScale = new Vector2(scaleX, scaleY);
    }

    public void UpdatePos()
    {
        currentMinThresholdIndex--;
        currentMaxThreshold = Mathf.Max(currentMaxThreshold, 0);

        if (currentMinThresholdIndex >= 0 && GameManager.Instance.attempts > 0 && !InputManager.Instance.resetInput)
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
        transform.position = new Vector2(newPosX, ballRangePoints.bottomLeft.y);
    }

    private BallRangePoints BallRanges()
    {
        float activeBallDiameter = sampleBallSpriteRenderer.bounds.size.x;

        Vector2 viewportbottomLeft = cam.ViewportToWorldPoint(Vector2.zero);

        float minHardLine = activeBallDiameter;
        float minMedLine = resetPosWidth / 3;
        float minEasyLine = minMedLine * 2;

        BallRangePoints points = new BallRangePoints
        {
            topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(minHardLine, 0),
            bottomLeft = cam.ViewportToWorldPoint(new Vector2(0, 0)) + new Vector3(minHardLine, activeBallDiameter),
            topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight)),
            bottomRight = cam.ViewportToWorldPoint(new Vector3(resetPosWidth, 0)) + new Vector3(0, activeBallDiameter),

            topOneThird = cam.ViewportToWorldPoint(new Vector2(minMedLine, resetPosHeight)),
            bottomOneThird = cam.ViewportToWorldPoint(new Vector2(minMedLine, 0)) + new Vector3(0, activeBallDiameter),

            topTwoThirds = cam.ViewportToWorldPoint(new Vector2(minEasyLine, resetPosHeight)),
            bottomTwoThirds = cam.ViewportToWorldPoint(new Vector2(minEasyLine, 0)) + new Vector3(0, activeBallDiameter)
        };

        return points;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        DrawBallResetArea(resetPosWidth, resetPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }

    private void DrawBallResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        ballRangePoints = BallRanges();

        Gizmos.color = color;
        Gizmos.DrawLine(ballRangePoints.bottomLeft, ballRangePoints.topLeft);
        Gizmos.DrawLine(ballRangePoints.topLeft, ballRangePoints.topRight);
        Gizmos.DrawLine(ballRangePoints.topRight, ballRangePoints.bottomRight);
        Gizmos.DrawLine(ballRangePoints.bottomRight, ballRangePoints.bottomLeft);

        Gizmos.DrawLine(ballRangePoints.topOneThird, ballRangePoints.bottomOneThird);
        Gizmos.DrawLine(ballRangePoints.topTwoThirds, ballRangePoints.bottomTwoThirds);
    }
    #endregion
}
