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
    private void SetUp()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        SetNextLevel();   
    }
    public void SetNextLevel()
    {
        levelCount++;

        if(activeBall != null) activeBall.gameObject.SetActive(false);
        if (activeBasket != null) activeBasket.gameObject.SetActive(false);

        activeBall = BallSpawner.Instance.allBalls[levelCount - 1];
        activeBasket = BasketSpawner.Instance.allBaskets[levelCount - 1];

        activeBall.gameObject.SetActive(true);
        activeBasket.gameObject.SetActive(true);
    }
}
