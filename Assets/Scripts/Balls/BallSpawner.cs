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
    public Vector2 newBallPos { get; private set; }

    private float minHardThreshold;
    private float minMedThreshold;
    private float minEasyThreshold;

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

        maxResetBallPosX = (camWidth * resetBallPosWidth) + bottomLeftWorldPos.x;
        maxResetBallPosY = (camHeight * resetBallPosHeight) + bottomLeftWorldPos.y;
    }
    private void Start()
    {
        newBallPos = transform.position;
        DrawBallResetArea(resetBallPosWidth, resetBallPosHeight, new Color(1.0f, 0.5f, 0.0f));
    }
    private void Update()
    {
        ResetBallPos(transform.position);
    }
    private void ResetBallPos(Vector2 ballPos)
    {
        Vector2 camWorldPointBottomLeft = SpawnPositionData.bottomLeftWorldPos;
        Vector2 camWorldPointTopRight = SpawnPositionData.topRightWoldPos;
        float camBoundBottom = camWorldPointBottomLeft.y;
        float camBoundRight = camWorldPointTopRight.x;
        float camBoundsLeft = camWorldPointBottomLeft.x;

        if (ballPos.y < camBoundBottom || ballPos.x > camBoundRight)
        {
            float randomX = 0;
            float randomY = 0;
            if (randomX == 0 || randomY == 0)
            {
                randomX = Random.Range(camBoundsLeft, maxResetBallPosX);
                randomY = Random.Range(camBoundBottom, maxResetBallPosY);
            }
            newBallPos = new Vector2(randomX, randomY);
            transform.position = newBallPos;
            activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void DrawBallResetArea(float resetPosWidth, float resetPosHeight, Color color)
    {
        float activeBallWidth = activeBall.spriteRendererBall.sprite.bounds.size.x;
        Vector2 viewportbottomLeft = cam.ViewportToWorldPoint(Vector2.zero);
        
        minHardThreshold = activeBallWidth;
        minMedThreshold = resetPosWidth / 3;
        minEasyThreshold = minMedThreshold * 2;

        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector2(0, resetPosHeight)) + new Vector3(minHardThreshold, 0);
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector2 (0, 0)) + new Vector3(minHardThreshold, activeBallWidth);
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector2(resetPosWidth, resetPosHeight));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(resetPosWidth, 0)) + new Vector3(0, activeBallWidth);


        Vector2 topMed = cam.ViewportToWorldPoint(new Vector2(minMedThreshold, resetPosHeight));
        Vector2 bottomMed = cam.ViewportToWorldPoint(new Vector2(minMedThreshold, 0)) + new Vector3(0, activeBallWidth);

        Vector2 topEasy = cam.ViewportToWorldPoint(new Vector2(minEasyThreshold, resetPosHeight));
        Vector2 bottomEasy = cam.ViewportToWorldPoint(new Vector2(minEasyThreshold, 0)) + new Vector3(0, activeBallWidth);

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
