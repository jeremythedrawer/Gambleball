using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BallSpawner : Spawner
{
    public static BallSpawner Instance;

    private Ball activeBall => LevelManager.Instance.activeBall;
    private BallRange ballRange => BallRange.Instance;

    public List<Ball> allBalls = new List<Ball>();

    public void Awake()
    {
        if (Instance == null) Instance = this;
        SetActiveBallPos();
        InstantiateLevelDataObjects<Ball>(allBalls, this.transform);
    }

    private async void SetActiveBallPos()
    {
        while (activeBall == null || BallRange.Instance.currentMaxThreshold == 0) { await Task.Yield(); }
        ResetBallPos(activeBall.transform.position);
    }

    public void ResetBallPos(Vector2 ballPos)
    {
        newPos = GetNewPos(ballRange.currentMinThreshold, ballRange.currentMaxThreshold, ballRange.minResetPosY, ballRange.maxResetPosY);
        activeBall.transform.position = newPos;
        activeBall.transform.eulerAngles = Vector2.zero;
        activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
