using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    public Vector2 newPos { get; protected set; }

    protected void InstantiateLevelDataObjects<T>(List<T> list, Transform thisTransform) where T : Component
    {
        foreach (LevelData.Level level in LevelManager.Instance.levelData.levels)
        {
            T prefab = typeof(T) == typeof(Ball) ? level.ball as T : level.basket as T;
            if (prefab != null)
            {
                T spawnedPrefab = Instantiate(prefab, thisTransform);
                spawnedPrefab.gameObject.SetActive(false);
                list.Add(spawnedPrefab);
            }
        }
    }

    protected Vector2 GetNewPos(float randomMinX, float randomMaxX, float randomMinY, float randomMaxY)
    {
        Vector2 newPos = new Vector2();
        float randomX = 0;
        float randomY = 0;
        if (randomX == 0 || randomY == 0)
        {
            randomX = Random.Range(randomMinX, randomMaxX);
            randomY = Random.Range(randomMinY, randomMaxY);
        }
        return newPos = new Vector2(randomX, randomY);
    }
}
