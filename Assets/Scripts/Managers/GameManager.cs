using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Ball activeBall => LevelManager.Instance.activeBall;
    public Basket activeBasket => LevelManager.Instance.activeBasket;

    public Camera cam {  get; private set; }
    public BallRange ballRange { get; private set; }
    public BallSpawner ballSpawner {  get; private set; }
    public BasketSpawner basketSpawner { get; private set; }

    public int attempts { get; set; } = 3;

    public bool outOfBounds { get; private set; }

    public int levelCount { get; private set; } = 0;

    private void OnValidate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetUp();
    }

    private void Awake()
    {
  
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        outOfBounds =   activeBall.transform.position.y < ScreenRangeData.bottomLeftWorldPos.y ||  
                        activeBall.transform.position.x > ScreenRangeData.topRightWoldPos.x;

        if (outOfBounds)
        {
            OutOfBounds();
        }

        if (attempts == 0 || InputManager.Instance.resetInput)
        {
            ResetGame();
        }

    }
    private void SetUp()
    {
        cam = Camera.main;
        ballSpawner = GetComponentInChildren<BallSpawner>();
        basketSpawner = GetComponentInChildren<BasketSpawner>();
        ScreenRangeData.SetUpScreenBounds();
        ballRange = BallRange.Instance;
    }

    public void OutOfBounds()
    {
        attempts--;

        ballRange.UpdatePos();
        activeBall.playerScored = false;
        activeBall.playerHitBird = false;

        ballSpawner.ResetBallPos(activeBall.transform.position);
    }

    public void ResetGame()
    {
        PointsUI.Instance.pointsCount = 0;
        LevelManager.Instance.currentLevelIndex = 0;
        LevelManager.Instance.SetNextLevel();

        ballRange.ResetBallRange();
        ballSpawner.ResetBallPos(activeBall.transform.position);

        attempts = 3;

        PointsUI.Instance.gameObject.SetActive(true);

        PointsUI.Instance.pointsCount = 0;
        ArcMaterial.tutorialMode = false;

        BirdSpawner.Instance.spawnedBird.ResetBird();
        activeBall.playerHitBird = false;
        BallSpawner.Instance.ResetBallPos(activeBall.transform.position);
    }

    public void ResetBackboard()
    {
        if (activeBasket.backboardRB.gameObject.transform.localPosition.y != activeBasket.backboardStartPos.y)
        {
            activeBasket.backboardRB.gameObject.transform.localPosition = activeBasket.backboardStartPos;
            activeBasket.backboardRB.gameObject.transform.eulerAngles = Vector2.zero;
            activeBasket.backboardRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
