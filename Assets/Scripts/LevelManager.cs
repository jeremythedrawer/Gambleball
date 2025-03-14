using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public static LevelManager Instance { get; private set; }

    public int levelCount {  get; set; }
    public Ball activeBall { get; private set; }
    public Basket activeBasket { get; private set; }
    private void OnValidate()
    {
        SetUp();
    }

    private void Awake()
    {
        SetUp();
        
    }
    private void Start()
    {
        SetNextLevel();   
    }
    private void SetUp()
    {
        if (Instance == null) Instance = this;
    }
    public void SetNextLevel()
    {
        if (levelCount == levelData.levels.Count)
        {
            levelCount = 1;
        }
        else
        {
            levelCount++;

        }

        if(activeBall != null) activeBall.gameObject.SetActive(false);
        if (activeBasket != null) activeBasket.gameObject.SetActive(false);

        foreach (Ball ball in BallSpawner.Instance.allBalls)
        {
            if (levelData.levels[levelCount - 1].ball.GetType() == ball.GetType())
            {
                activeBall = ball;
                break;
            }
        }

        foreach (Basket basket in BasketSpawner.Instance.allBaskets)
        {
            if (levelData.levels[levelCount - 1].basket.GetType() == basket.GetType())
            {
                activeBasket = basket;
                break;
            }
        }

        activeBall.gameObject.SetActive(true);
       // activeBall.rigidBodyBall.constraints = RigidbodyConstraints2D.FreezeAll;
        activeBasket.gameObject.SetActive(true);
    }
}
