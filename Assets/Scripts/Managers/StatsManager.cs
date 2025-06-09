using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int daysUnlocked = 1;
    public int currentScore {  get; private set; }
    public float currentTime {  get; private set; }
    public int attemptsLeft { get; private set; } = 9;

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
        BallSpawner.onPlayerNotScored += UpdateAttempts;
        BallSpawner.onInBasket += PlayerScored;
        GameModeManager.onPlayerBeatDay += PrepareForNextDay;
        GameModeManager.onPlayerLost += ResetDay;
    }
    private void OnDisable()
    {
        BallSpawner.onPlayerNotScored -= UpdateAttempts;
        BallSpawner.onInBasket -= PlayerScored;
        GameModeManager.onPlayerBeatDay -= PrepareForNextDay;
        GameModeManager.onPlayerLost -= ResetDay;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Menu") return;
        CheckScoredFromDowntown();
        CheckOnFire();
    }

    private void UpdateAttempts()
    {
        if (attemptsLeft > 0)
        {
            attemptsLeft--;
            scoresInRow = 0;
        }
    }

    private void PlayerScored()
    {
        TallyPoints();
        scoresInRow++;
    }
    private void TallyPoints()
    {
        //TODO: if active ball is special tally three instead
        currentScore += 2;
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

    public void SetCountDownTime()
    {
        Debug.Log(GameModeManager.instance.currentGameMode.modeData.ToString());
        if (GameModeManager.instance.currentGameMode.modeData is TimerData timeData)
        {
            currentTime = timeData.time;
        }
    }
    public void PrepareForNextDay()
    {
        currentScore = 0;
        daysUnlocked++;
    }

    private void ResetDay()
    {
        currentScore = 0;
        attemptsLeft = 9;
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
