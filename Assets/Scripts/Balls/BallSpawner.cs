using System.Collections.Generic;
using UnityEngine;
using System;

public class BallSpawner : Spawner
{
    public static BallSpawner instance;

    public Transform bottomLeft;
    public Transform topRight;

    public List<Ball> allBalls = new List<Ball>();
    public Ball activeBall;

    public static event Action onOutOfBounds;
    public static event Action onScore;
    public void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        SetActiveBall();
    }

    private void Update()
    {
        if (activeBall.outOfBounds)
        {
            onOutOfBounds?.Invoke();
            SetActiveBall();
        }

        if (activeBall.playerScored)
        {
            onScore?.Invoke();
        }
    }

    private void SetActiveBall()
    {
        foreach (Ball ball in allBalls)
        {
            if (LevelManager.instance.levelData.levels[LevelManager.instance.currentLevelIndex].ball.GetType() == ball.GetType())
            {
                activeBall = ball;
                activeBall.gameObject.SetActive(true);
                ResetActiveBallPos();
            }
            else
            {
                ball.gameObject.SetActive(false);
            }
        }
    }
    private void ResetActiveBallPos()
    {
        activeBall.transform.position = NewPos(bottomLeft.position, topRight.position);
        activeBall.transform.eulerAngles = Vector2.zero;
        activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Vector3 bottomRight = new Vector3(topRight.position.x, bottomLeft.position.y);
        Vector3 topLeft = new Vector3(bottomLeft.position.x, topRight.position.y);
        Gizmos.DrawLine(bottomLeft.position, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight.position);
        Gizmos.DrawLine(topRight.position, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft.position);
    }
}
