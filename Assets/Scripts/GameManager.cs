using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Ball activeBall => LevelManager.Instance.activeBall;
    public Basket activeBasket => LevelManager.Instance.activeBasket;

    public Camera cam {  get; private set; }
    public BallRange activeRangeController { get; private set; }
    public BallSpawner ballSpawner {  get; private set; }
    public BasketSpawner basketSpawner { get; private set; }

    public int attempts { get; set; } = 3;

    public bool outOfBounds { get; private set; }

    public int levelCount { get; private set; } = 0;

    private void OnValidate()
    {
        SetUp();
    }

    private void Awake()
    {
        SetUp();
    }

    private void Update()
    {

        outOfBounds = activeBall.transform.position.y < ScreenRangeData.bottomLeftWorldPos.y || activeBall.transform.position.x > ScreenRangeData.topRightWoldPos.x;

        if (outOfBounds)
        {
            OutOfBounds();
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
        activeRangeController = BallRange.Instance;
    }

    public void OutOfBounds()
    {
        attempts--;
        activeRangeController.UpdatePos();
        ballSpawner.ResetBallPos(activeBall.transform.position);
    }
}
