using UnityEngine;

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
    }

    private void Update()
    {
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
