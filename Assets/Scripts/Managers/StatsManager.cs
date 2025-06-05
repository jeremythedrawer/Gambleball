using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager instance;
    public int totalScore;
    public int attemptsLeft = 9;

    public int successfulAttempts;

    public int scoresInRow;
    public bool scoredFromThree;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BallSpawner.onPlayerNotScored += UpdateAttempts;
        BallSpawner.onInBasket += PlayerScored;
    }

    private void UpdateAttempts()
    {
        if (attemptsLeft > 0)
        {
            attemptsLeft--;
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
    }
    private void TallyPoints()
    {
        //TODO: if active ball is special tally three instead
        totalScore += 2;
    }
}
