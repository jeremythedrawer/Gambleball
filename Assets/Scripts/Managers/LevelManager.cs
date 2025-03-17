using System;
using System.Linq;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelData levelData;
    public static LevelManager Instance { get; private set; }

    public int currentLevelIndex {  get; set; }
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

    private void Update()
    {
        int activeInput = Array.FindIndex(InputManager.Instance.numberInputs, input => input);

        if (activeInput >= 0)
        {
            SetKeycodeLevel(activeInput);
        }
    }
    private void SetUp()
    {
        if (Instance == null) Instance = this;
    }
    public void SetNextLevel()
    {
        if (currentLevelIndex == levelData.levels.Count) // to loop to start
        {
            currentLevelIndex = 1;
        }
        else
        {
            currentLevelIndex++;

        }

        SetActiveLevelObjects();
        GameManager.Instance.ResetBackboard();
    }

    public void SetKeycodeLevel(int numberInput)
    {
        currentLevelIndex = numberInput + 1;

        SetActiveLevelObjects();
        BallRange.Instance.ResetBallRange();
        BallSpawner.Instance.ResetBallPos(activeBall.transform.position);
        GameManager.Instance.attempts = 3;
    }

    private void SetActiveLevelObjects()
    {
        if (activeBall != null) activeBall.gameObject.SetActive(false);
        if (activeBasket != null) activeBasket.gameObject.SetActive(false);

        foreach (Ball ball in BallSpawner.Instance.allBalls)
        {
            if (levelData.levels[currentLevelIndex - 1].ball.GetType() == ball.GetType())
            {
                activeBall = ball;
                break;
            }
        }

        foreach (Basket basket in BasketSpawner.Instance.allBaskets)
        {
            if (levelData.levels[currentLevelIndex - 1].basket.GetType() == basket.GetType())
            {
                activeBasket = basket;
                break;
            }
        }

        activeBall.gameObject.SetActive(true);
        activeBasket.gameObject.SetActive(true);

        BasketSpawner.Instance.SetNewBasketPos();
    }
}
