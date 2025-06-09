using System.Collections.Generic;
using UnityEngine;
using System;

public class BallSpawner : Spawner
{
    public static BallSpawner instance;

    public Transform bottomLeft;
    public Transform topRight;

    public Ball activeBall;

    public static event Action onOutOfBounds;
    public static event Action onPlayerScored;
    public static event Action onPlayerNotScored;
    public static event Action onInBasket;

    public Vector2 currentChosenPos;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        ResetActiveBallPos();
    }

    private void Update()
    {
        if (activeBall.outOfBounds)
        {
            onOutOfBounds?.Invoke();
            ResetActiveBallPos();

            if (activeBall.playerScored)
            {
                onPlayerScored?.Invoke();
                activeBall.playerScored = false;
            }
            else
            {
                onPlayerNotScored?.Invoke();
            }
        }

        if (activeBall.inBasket)
        {
            onInBasket?.Invoke();
        }
    }
    private void ResetActiveBallPos()
    {
        currentChosenPos = NewPos(bottomLeft.position, topRight.position);
        activeBall.transform.position = currentChosenPos;
        activeBall.transform.eulerAngles = Vector2.zero;
        activeBall.body.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void PickBall()
    {
        switch (activeBall.type)
        {
            case Ball.BallType.Normal:
            {

            }
            break;
            case Ball.BallType.Moneyball:
            {

            }
            break;
            case Ball.BallType.AttemptBoost:
            {

            }
            break;
        }
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
