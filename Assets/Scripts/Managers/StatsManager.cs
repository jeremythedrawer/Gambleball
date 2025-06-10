using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int daysUnlocked = 1;
    public int currentScore { get; private set; }
    public float currentTime { get; private set; }
    public int currentMoneyBallAttempts { get; private set; }
    public int currentHeartAttempts { get; private set; } = 9;

    public bool onFire { get; private set; }
    public bool fromDowntown { get; private set; }
    private int scoresInRow;
    private float oneSecondTimer = 1f;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        BallSpawner.onPlayerNotScored += UpdateHeartAttempts;
        BallSpawner.onOutOfBounds += UpdateMoneyBallStats;
        BallSpawner.onInBasket += PlayerScored;
        GameModeManager.onPlayerBeatDay += PrepareForNextDay;
        GameModeManager.onPlayerLost += ResetDay;
    }
    private void OnDisable()
    {

        BallSpawner.onPlayerNotScored -= UpdateHeartAttempts;
        BallSpawner.onOutOfBounds -= UpdateMoneyBallStats;
        BallSpawner.onInBasket -= PlayerScored;
        GameModeManager.onPlayerBeatDay -= PrepareForNextDay;
        GameModeManager.onPlayerLost -= ResetDay;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "End") return;
        CheckScoredFromDowntown();
        CheckOnFire();
    }

    private void UpdateHeartAttempts()
    {
        if (GameModeManager.instance.currentGameMode.modeData is LivesData)
        {
            if (currentHeartAttempts > 0)
            {
                currentHeartAttempts--;
            }
        }
        scoresInRow = 0;
    }

    private void UpdateMoneyBallStats()
    {
        if (GameModeManager.instance.currentGameMode.modeData is MoneyBallData)
        {
            if (currentMoneyBallAttempts > 0)
            {
                currentMoneyBallAttempts--;
            }
        }
    }
    private void PlayerScored()
    {
        if (GameModeManager.instance.currentGameMode.modeData is MoneyBallData)
        {
            if (BallSpawner.instance.type == BallType.Moneyball)
            {
                TallyPoints(scoreAmount: 3);
            }
            else
            {
                TallyPoints(scoreAmount: 2);
            }

            if(BallSpawner.instance.type == BallType.AttemptBoost)
            {
                currentMoneyBallAttempts += 2;
            }
        }
        else
        {
            TallyPoints(scoreAmount: 2);
        }
        scoresInRow++;
    }
    private void TallyPoints(int scoreAmount)
    {
        currentScore += scoreAmount;
    }

    public void CountDownTime()
    {
        oneSecondTimer -= Time.deltaTime;

        if (oneSecondTimer <= 0f)
        {
            currentTime -= 1;
            oneSecondTimer = 1f;
        }
    }

    public void SetHeartAttempts(LivesData livesData)
    {
        currentHeartAttempts = livesData.lives * 3;
    }
    public void SetCountDownTime(TimerData timerData)
    {
        currentTime = timerData.time;
    }

    public void SetMoneyBallAttempts(MoneyBallData moneyBallData)
    {
        currentMoneyBallAttempts = moneyBallData.startingAttempts;
    }

    public void PrepareForNextDay()
    {
        currentScore = 0;
        daysUnlocked++;
    }

    private void ResetDay()
    {
        currentScore = 0;
    }
    private void CheckScoredFromDowntown()
    {
        fromDowntown = Vector2.Distance(BallSpawner.instance.currentChosenPos, Basket.instance.transform.position) > 3.5;
    }

    private void CheckOnFire()
    {
        onFire = scoresInRow >= 3;
    }
}
