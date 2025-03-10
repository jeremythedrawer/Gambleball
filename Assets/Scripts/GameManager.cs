using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelData levelData;

    public Ball activeBall {  get; private set; }
    public Basket activeBasket { get; private set; }

    public Camera cam {  get; private set; }
    public ActiveRangeController activeRangeController { get; private set; }
    public BallSpawner ballSpawner {  get; private set; }
    public BasketSpawner basketSpawner { get; private set; }

    public int attempts { get; set; } = 0;

    public bool outOfBounds { get; private set; }

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
        activeBall = Instantiate(levelData.levels[0].ball);  
    }
    private void Update()
    {
        if (attempts < 1) attempts = 3;

        outOfBounds = activeBall.transform.position.y < ScreenRangeData.bottomLeftWorldPos.y || activeBall.transform.position.x > ScreenRangeData.topRightWoldPos.x;

        if (outOfBounds)
        {
            attempts--;
            activeRangeController.UpdatePos();
            ballSpawner.ResetBallPos(activeBall.transform.position);
        }
    }
    private void SetUp()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        cam = Camera.main;
        ballSpawner = GetComponentInChildren<BallSpawner>();
        basketSpawner = GetComponentInChildren<BasketSpawner>();
        ScreenRangeData.SetUpScreenBounds();
        activeRangeController = FindFirstObjectByType<ActiveRangeController>();
    }
}
