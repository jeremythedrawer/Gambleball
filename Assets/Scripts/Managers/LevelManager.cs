using System;
using System.Linq;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public static LevelManager Instance { get; private set; }
    public int currentLevelIndex { get; set; } = 0;
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
        SetActiveLevelObjects();
    }

    private void Update()
    {
        SetKeycodeLevel();
    }
    private void SetUp()
    {
        if (Instance == null) Instance = this;
    }
    public void SetNextLevel()
    {
        if (currentLevelIndex == levelData.levels.Count - 1) // to loop to start
        {
            currentLevelIndex = 1;
        }
        else
        {
            currentLevelIndex++;
            PointsUI.Instance.gameObject.SetActive(true);
            ArcMaterial.tutorialMode = false;
        }
        SetActiveLevelObjects();
        GameManager.Instance.ResetBackboard();
    }

    public void SetKeycodeLevel()
    {
        int activeInput = Array.FindIndex(InputManager.Instance.numberInputs, input => input);

        if (activeInput >= 0)
        {
            currentLevelIndex = activeInput + 1;
            PointsUI.Instance.gameObject.SetActive(false);

            PointsUI.Instance.pointsCount = 0;
            SetActiveLevelObjects();
            GameManager.Instance.ResetBackboard();
            BallRange.Instance.ResetBallRange();
            BallSpawner.Instance.ResetBallPos(activeBall.transform.position);
            GameManager.Instance.attempts = 3;
            ArcMaterial.tutorialMode = false;

            activeBall.playerHitBird = false;
        }
    }

    private void SetActiveLevelObjects()
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
}
