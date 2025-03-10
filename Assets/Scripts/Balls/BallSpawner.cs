using System.Threading.Tasks;
using UnityEngine;

public class BallSpawner : Spawner
{
    public static BallSpawner Instance;

    private Ball activeBall => GameManager.Instance.activeBall;

    public Vector2 newBallPos { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetActiveBallPos();
    }

    private async void SetActiveBallPos()
    {
        while (activeBall == null || ActiveRangeController.Instance.currentMaxThreshold == 0) { await Task.Yield(); }
        ResetBallPos(activeBall.transform.position);
    }

    public void ResetBallPos(Vector2 ballPos)
    {
        newBallPos = GetNewBallPos();
        activeBall.transform.position = newBallPos;
        activeBall.transform.eulerAngles = Vector2.zero;
        activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public Vector2 GetNewBallPos()
    {
        Vector2 newBallPos = new Vector2();
        float randomX = 0;
        float randomY = 0;
        if (randomX == 0 || randomY == 0)
        {
            randomX = Random.Range(ActiveRangeController.Instance.currentMinThreshold, ActiveRangeController.Instance.currentMaxThreshold);
            randomY = Random.Range(ActiveRangeController.Instance.minResetBallPosY, ActiveRangeController.Instance.maxResetBallPosY);
        }
       return newBallPos = new Vector2(randomX, randomY);
    }
}
