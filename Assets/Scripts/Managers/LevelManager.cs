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
        instance = this;
    }

    private void OnEnable()
    {
        BallSpawner.onPlayerNotScored += SetToLastCheckpoint;
    }

    private void IncreaseLevelIndex()
    {
        currentLevelIndex++;
    }

    private void SetToLastCheckpoint()
    {
        if (StatsManager.instance.attemptsLeft == 0)
        {
            currentLevelIndex = lastCheckpointIndex;
        }
    }
}
