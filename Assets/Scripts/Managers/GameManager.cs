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

    public int currentLevelIndex { get; private set; } = 0;
    public int lastCheckpointIndex { get; private set; } = -1;

    private void OnValidate()
    {
       ScreenRangeData.SetUpScreenBounds();
    }

    private void Awake()
    {
        ScreenRangeData.SetUpScreenBounds();
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        SetActiveBallAndBasket();
    }
    private void Update()
    {
        ScreenWrapBall();
        CheckOutOfBounds();
        HandleGameResetInput();
        SetKeycodeLevel();
    }
    private void HandleGameResetInput()
    {
        if (InputManager.Instance.resetInput)
        {
            ResetGame(0);
            ResetBird();
            ResetBackboard();
            BallSpawner.Instance.UpdatePos();
            BallSpawner.Instance.ResetActiveBallPos();
            PointsUI.Instance.gameObject.SetActive(false);
            ArcMaterial.tutorialMode = true;
        }
    }

    private void CheckOutOfBounds()
    {
        outOfBounds = activeBall.transform.position.y < ScreenRangeData.bottomLeftWorldPos.y;
        if (outOfBounds)
        {
            HandleOutOfBounds();
        }
    }
    private void ScreenWrapBall()
    {
        if (activeBasket.GetType() == typeof(WrappingBasket))
        {
            ArrowMaterial.onOff = true;
            if (activeBall.transform.position.x > ScreenRangeData.topRightWoldPos.x)
            {
                activeBall.transform.position = new Vector2(ScreenRangeData.bottomLeftWorldPos.x, activeBall.transform.position.y);
            }
        }
        else
        {
            ArrowMaterial.onOff = false;
        }
    }
    public void HandleOutOfBounds()
    {
        if (attempts > 0 && !activeBall.playerScored)
        {
            attempts--;
            HeartsUI.Instance.LooseAttempt();
        }

        if (attempts == 0 || activeBall.playerScored)
        {
            BallSpawner.Instance.UpdatePos();
            HeartsUI.Instance.ReplenishHearts();
        }
        if (attempts == 0)
        {
            if (HeartsUI.Instance.heartsLeft > 1 && lastCheckpointIndex != -1)
            {
                currentLevelIndex = lastCheckpointIndex;
                HeartsUI.Instance.LooseHeart();
            }
            else
            {
                ResetGame(1);
            }

            attempts = 3;
            SkipTutorial();
            ResetBird();
            ResetBackboard();
            SetActiveBallAndBasket();
        }
        else if (activeBall.playerScored && BallSpawner.Instance.currentMinThresholdIndex == 2)
        {
            if (currentLevelIndex != levelData.levels.Count - 1)
            {
                currentLevelIndex++;
            }
            else
            {
                currentLevelIndex = 1;
            }
            attempts = 3;
            SkipTutorial();
            ResetBird();
            ResetBackboard();
            SetActiveBallAndBasket();

            if (levelData.levels[currentLevelIndex].checkpoint)
            {
                if (lastCheckpointIndex == -1) HeartsUI.Instance.ResetHearts();

                CheckpointsUI.Instance.SetNextCheckpointActive();
                lastCheckpointIndex = currentLevelIndex;
            }

            if (currentLevelIndex == 1)
            {
                HeartsUI.Instance.ShowOneHeart();
            }
        }

        if (activeBall.playerScored)
        {
            attempts = 3;
            activeBall.playerScored = false;
        }

        if (activeBall.playerHitBird) activeBall.playerHitBird = false;

        BallSpawner.Instance.ResetActiveBallPos();
    }
    private void ResetGame(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        lastCheckpointIndex = -1;
        PointsUI.Instance.ResetGame();
        HeartsUI.Instance.HideHearts();
        HeartsUI.Instance.ShowOneHeart();
        CheckpointsUI.Instance.HideCheckpoints();
        SetActiveBallAndBasket();
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
