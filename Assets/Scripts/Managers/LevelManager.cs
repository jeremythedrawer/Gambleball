using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public static LevelManager instance { get; private set; }
    public int currentLevelIndex { get; private set; }
    public int lastCheckpointIndex { get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        BallSpawner.onOutOfBounds += SetLevelIndex;
    }
    public void SetLevelIndex()
    {
        if (StatsManager.instance.attemptsLeft == 0)
        {
            currentLevelIndex = lastCheckpointIndex;
        }
        else if (StatsManager.instance.successfulAttempts % 3 == 0)
        {
            currentLevelIndex++;
        }
    }
}
