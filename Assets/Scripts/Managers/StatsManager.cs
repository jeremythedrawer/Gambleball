using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;

    public int targetScore = 50;
    public int currentScore {  get; private set; }
    public int attemptsLeft { get; private set; } = 9;

    private int scoresInRow;
    public bool onFire { get; private set; }
    public bool fromDowntown { get; private set; }



    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BallSpawner.onPlayerNotScored += UpdateAttempts;
        BallSpawner.onInBasket += PlayerScored;
        SceneManager.activeSceneChanged += Run;
    }
    private void OnDisable()
    {
        BallSpawner.onPlayerNotScored -= UpdateAttempts;
        BallSpawner.onInBasket -= PlayerScored;
        SceneManager.activeSceneChanged -= Run;
    }
    private void Update()
    {
        CheckScoredFromDowntown();
        CheckOnFire();
    }

    private void Run(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Menu")
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }

    private void UpdateAttempts()
    {
        if (attemptsLeft > 0)
        {
            attemptsLeft--;
            scoresInRow = 0;
        }
        else
        {
            attemptsLeft = 9;
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

    private void CheckScoredFromDowntown()
    {
        fromDowntown = Vector2.Distance(BallSpawner.instance.currentChosenPos, Basket.instance.transform.position) > 3.5;
    }

    private void CheckOnFire()
    {
        onFire = scoresInRow >= 3;
    }
}
