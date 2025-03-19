using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;

    public static GameManager Instance { get; private set; }

    public Ball activeBall { get; private set; }
    public Basket activeBasket { get; private set; }

    public int attempts { get; private set; } = 3;

    public bool outOfBounds { get; private set; }

    public int levelCount { get; private set; } = 0;

    public int currentLevelIndex { get; private set; } = 0;
    public int lastCheckpointIndex { get; private set; } = 1;

    private void OnValidate()
    {
        ScreenRangeData.SetUpScreenBounds();
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        SetActiveBallAndBasket();
    }
    private void Update()
    {
        CheckOutOfBounds();
        HandleGameReset();
        SetKeycodeLevel();
    }
    private void HandleGameReset()
    {
        if (InputManager.Instance.resetInput)
        {
            ResetGame();
            SkipTutorial();
            ResetBird();
            ResetBackboard();
        }
    }

    private void CheckOutOfBounds()
    {
        outOfBounds =   activeBall.transform.position.y < ScreenRangeData.bottomLeftWorldPos.y ||  
                        activeBall.transform.position.x > ScreenRangeData.topRightWoldPos.x;
        if (outOfBounds)
        {
            HandleOutOfBounds();
        }
    }

    public void HandleOutOfBounds()
    {
        if (attempts > 0 && !activeBall.playerScored) attempts--;
        if (attempts == 0 || activeBall.playerScored) BallSpawner.Instance.UpdatePos();

        if (attempts == 0)
        {
            if (HeartsUI.Instance.heartsLeft > 0 && lastCheckpointIndex != 1)
            {
                currentLevelIndex = lastCheckpointIndex;
            }
            else
            {
                ResetGame();
            }

            attempts = 3;
            SkipTutorial();
            ResetBird();
            ResetBackboard();
            SetActiveBallAndBasket();
        }
        else if (activeBall.playerScored && BallSpawner.Instance.currentMinThresholdIndex == 2)
        {
            currentLevelIndex++;
            attempts = 3;

            SkipTutorial();
            ResetBird();
            ResetBackboard();
            SetActiveBallAndBasket();

        }
        if (activeBall.playerScored)
        {
            attempts = 3;
            activeBall.playerScored = false;
        }
        if (activeBall.playerHitBird) activeBall.playerHitBird = false;
        BallSpawner.Instance.ResetActiveBallPos();
    }

    private void ResetGame()
    {
        currentLevelIndex = 1;
        PointsUI.Instance.ResetGame();
    }

    private void SetActiveBallAndBasket()
    {
        if (activeBall != null) activeBall.gameObject.SetActive(false);
        if (activeBasket != null) activeBasket.gameObject.SetActive(false);

        foreach (Ball ball in BallSpawner.Instance.allBalls)
        {
            if (levelData.levels[currentLevelIndex].ball.GetType() == ball.GetType())
            {
                activeBall = ball;
                break;
            }
        }

        foreach (Basket basket in BasketSpawner.Instance.allBaskets)
        {
            if (levelData.levels[currentLevelIndex].basket.GetType() == basket.GetType())
            {
                activeBasket = basket;
                break;
            }
        }

        activeBall.gameObject.SetActive(true);
        activeBasket.gameObject.SetActive(true);

        BasketSpawner.Instance.SetNewBasketPos();

        BirdSpawner.Instance.CheckToSpawnBird(currentLevelIndex, levelData);
    }
    public void SetKeycodeLevel()
    {
        int activeInput = Array.FindIndex(InputManager.Instance.numberInputs, input => input);

        if (activeInput >= 0)
        {
            attempts = 3;
            currentLevelIndex = activeInput;
            PointsUI.Instance.gameObject.SetActive(false);
            PointsUI.Instance.ResetGame();
            SetActiveBallAndBasket();
            ResetBird();
            ResetBackboard();
            BallSpawner.Instance.UpdatePos();
            BallSpawner.Instance.ResetActiveBallPos();

            activeBall.playerScored = false;
            activeBall.playerHitBird = false;
        }
    }
    private void SkipTutorial()
    {
        PointsUI.Instance.gameObject.SetActive(true);
        ArcMaterial.tutorialMode = false;
    }
    private void ResetBird()
    {
        BirdSpawner.Instance.spawnedBird.ResetBird();
        activeBall.playerHitBird = false;
    }
    private void ResetBackboard()
    {
        activeBasket.backboardRB.gameObject.transform.localPosition = activeBasket.backboardStartPos;
        activeBasket.backboardRB.gameObject.transform.eulerAngles = Vector2.zero;
        activeBasket.backboardRB.constraints = RigidbodyConstraints2D.FreezeAll;
    }

}
