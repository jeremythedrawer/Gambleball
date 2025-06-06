using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;
    public int totalScore;
    public int attemptsLeft = 9;

    public int successfulAttempts;

    public int scoresInRow;
    public bool onFire;
    public bool fromDowntown;

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
            successfulAttempts = 0;
        }
    }

    private void PlayerScored()
    {
        TallyPoints();
        successfulAttempts++;
        scoresInRow++;
    }
    private void TallyPoints()
    {
        //TODO: if active ball is special tally three instead
        totalScore += 2;
    }

    private void CheckScoredFromDowntown()
    {
        fromDowntown = Vector2.Distance(BallSpawner.instance.currentChosenPos, Basket.instance.transform.position) > 3.2 && !onFire;
    }

    private void CheckOnFire()
    {
        onFire = scoresInRow >= 3;
    }
}
