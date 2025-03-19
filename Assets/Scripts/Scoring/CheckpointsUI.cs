using System.Collections.Generic;
using UnityEngine;

public class CheckpointsUI : MonoBehaviour
{
    public static CheckpointsUI Instance {  get; private set; }
    public Checkpoint checkpointPrefab;
    public List<Checkpoint> checkpoints { get; private set; } = new List<Checkpoint>();

    private float spriteOffset;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        InstantiateCheckpoints();
    }

    private void InstantiateCheckpoints()
    {
        foreach (LevelData.Level level in GameManager.Instance.levelData.levels)
        {
            if (level.checkpoint)
            {
                Checkpoint spawnedCheckPoint = Instantiate(checkpointPrefab, this.transform);
                checkpoints.Add(spawnedCheckPoint);

                spawnedCheckPoint.transform.position = new Vector2 (transform.position.x + spriteOffset, transform.position.y);
                spawnedCheckPoint.gameObject.SetActive(false);
                spriteOffset += spawnedCheckPoint.maxXPoint;
            }
        }
    }

    public void HideCheckpoints()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.isActiveAndEnabled)
            {
                checkpoint.gameObject.SetActive(false);
            }
        }
    }

    public void SetNextCheckpointActive()
    {
        Checkpoint nextCheckpoint = checkpoints.Find(checkpoint => !checkpoint.gameObject.activeInHierarchy);
        if (nextCheckpoint != null) nextCheckpoint.gameObject.SetActive(true);
    }
}
