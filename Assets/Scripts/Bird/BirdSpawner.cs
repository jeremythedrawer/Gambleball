using UnityEngine;

public class BirdSpawner : Spawner
{
    public float maxYPos;
    public float xPosBuffer;
    public static BirdSpawner Instance { get; private set; }
    
    public Bird spawnedBird { get; private set; }

    private Basket activeBasket => GameManager.Instance.activeBasket;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InstantiateBird();
    }

    private void InstantiateBird()
    {
        Bird bird = GameManager.Instance.levelData.bird;
        spawnedBird = Instantiate(bird, this.transform);
        spawnedBird.gameObject.SetActive(false);
    }

    public void CheckToSpawnBird(int currentLevelIndex, LevelData levelData)
    {
        if (levelData.levels[currentLevelIndex].spawnBird)
        {
            spawnedBird.gameObject.SetActive(false);
            spawnedBird.transform.position = SetBirdPos();
            spawnedBird.gameObject.SetActive(true);
        }
        else if (spawnedBird.isActiveAndEnabled)
        {
            spawnedBird.gameObject.SetActive(false);

        }
    }

    private Vector2 SetBirdPos()
    {
        float randomY = Random.Range(activeBasket.transform.position.y, maxYPos);

        float leftX = ScreenRangeData.bottomLeftWorldPos.x - xPosBuffer;
        float rightX = ScreenRangeData.topRightWoldPos.x + xPosBuffer;
        float randomX = Random.value < 0.5f ? leftX : rightX;

        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmos()
    {
        Vector2 topLeft = new Vector2(ScreenRangeData.bottomLeftWorldPos.x - xPosBuffer, maxYPos);
        Vector2 bottomLeft = new Vector2(ScreenRangeData.bottomLeftWorldPos.x - xPosBuffer, 0);

        Vector2 topRight = new Vector2(ScreenRangeData.topRightWoldPos.x + xPosBuffer, maxYPos);
        Vector2 bottomRight = new Vector2(ScreenRangeData.topRightWoldPos.x + xPosBuffer, 0);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);
    }

}
