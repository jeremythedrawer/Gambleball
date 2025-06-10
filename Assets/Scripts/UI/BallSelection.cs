using System;
using System.Collections.Generic;
using UnityEngine;

public enum BallType
{
    Normal,
    Moneyball,
    AttemptBoost
}
public class BallSelection : MonoBehaviour
{

    [Serializable]
    public struct BallData
    {
        public Material material;
        public BallType type;

        public BallData(Material material, BallType type)
        {
            this.material = material;
            this.type = type;
        }
    }

    public List<BallData> ballsData;
    public MeshRenderer[] nextBalls;
    public BallData nextBallData { get; private set; }

    private int[] nextBallIndexes = new int[3];


    private void OnEnable()
    {
        PickThreeBalls();
        BallSpawner.onOutOfBounds += UpdateNextBalls;
    }

    private void OnDisable()
    {
        BallSpawner.onOutOfBounds -= UpdateNextBalls;
    }

    private void PickThreeBalls()
    {
        for (int i = 0; i < nextBallIndexes.Length; i++)
        {
            nextBallIndexes[i] = UnityEngine.Random.Range(0, 3);
        }

        for (int i = 0; i < nextBalls.Length; i++)
        {
            nextBalls[i].material = ballsData[nextBallIndexes[i]].material;
        }

        nextBallData = ballsData[nextBallIndexes[0]];
    }


    private void UpdateNextBalls()
    {
        BallSpawner.instance.activeBall.meshRenderer.material = nextBallData.material;
        BallSpawner.instance.type = nextBallData.type;
        for(int i = 0; i < nextBalls.Length; i++)
        {
            if (i <  nextBallIndexes.Length - 1)
            {
                nextBalls[i].material = nextBalls[i + 1].material;
                nextBallIndexes[i] = nextBallIndexes[i + 1];
            }
            else
            {
                nextBalls[i].material = PickLastBall();
            }
        }

        nextBallData = ballsData[nextBallIndexes[0]];
    }
    private Material PickLastBall()
    {
        nextBallIndexes[2] = UnityEngine.Random.Range(0, 3);
        return ballsData[nextBallIndexes[2]].material;
    }
}
