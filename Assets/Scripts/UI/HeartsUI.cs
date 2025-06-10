using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance { get; private set; }

    private HeartsMaterial[] hearts = new HeartsMaterial[3];

    private int currentHeartIndex = 2;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        hearts = GetComponentsInChildren<HeartsMaterial>();
        BallSpawner.onPlayerNotScored += PlayerDidntScore;
        BallSpawner.onPlayerScored += ReplenishCurrentHeart;
    }
    private void UpdateHearts()
    {
        TrackHeartsLeft();

        if (StatsManager.instance.currentHeartAttempts > 0 && !BallSpawner.instance.activeBall.playerScored)
        {
            hearts[currentHeartIndex - 1].attemptsLeft--; // Loose attempt on current heart
        }
        else if (StatsManager.instance.currentHeartAttempts == 0 || !BallSpawner.instance.activeBall.playerScored) //Replenish Hearts
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].attemptsLeft > 0)
                {
                    hearts[i].attemptsLeft = 3;
                }
            }
        }
        else if (StatsManager.instance.currentHeartAttempts == 0 && currentHeartIndex > 1) // loose heart
        {
            currentHeartIndex--;
            hearts[currentHeartIndex].attemptsLeft = 0;
        }
        else // reset hearts
        {
            currentHeartIndex = 3;
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].alpha != 1)
                {
                    hearts[i].attemptsLeft = 3;
                    hearts[i].ShowHeart();
                }
            }
        }
    }

    private void PlayerDidntScore()
    {
        if (currentHeartIndex == 0 && hearts[currentHeartIndex].attemptsLeft == 1)
        {
            ResetHearts();
        }
        else
        {
            LooseAttempt();

            if (hearts[currentHeartIndex].attemptsLeft == 0)
            {
                hearts[currentHeartIndex].alpha = 0;
                LooseHeart();
            }
        }
    }

    private void LooseHeart()
    {
        currentHeartIndex--;
    }
    private void LooseAttempt()
    {
        hearts[currentHeartIndex].attemptsLeft--;;
    }

    private void ReplenishCurrentHeart()
    {
        hearts[currentHeartIndex].attemptsLeft = 3;
    }

    private void ResetHearts()
    {
        currentHeartIndex = 2;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].alpha = 0;
            hearts[i].attemptsLeft = 3;
            hearts[i].ShowHeart();
        }
    }
    private void TrackHeartsLeft()
    {
        if (StatsManager.instance.currentHeartAttempts % 3 == 0)
        {
            currentHeartIndex--;
        }
    }
}
