using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance { get; private set; }

    private HeartsMaterial[] hearts = new HeartsMaterial[3];

    private int heartsLeft = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        hearts = GetComponentsInChildren<HeartsMaterial>();
    }

    private void OnEnable()
    {
        BallSpawner.onOutOfBounds += UpdateHearts;
    }
    private void UpdateHearts()
    {
        TrackHeartsLeft();

        if (StatsManager.instance.attemptsLeft > 0 && !BallSpawner.instance.activeBall.playerScored)
        {
            hearts[heartsLeft - 1].attemptsLeft--; // Loose attempt on current heart
        }
        else if (StatsManager.instance.attemptsLeft == 0 || !BallSpawner.instance.activeBall.playerScored) //Replenish Hearts
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].attemptsLeft > 0)
                {
                    hearts[i].attemptsLeft = 3;
                }
            }
        }
        else if (StatsManager.instance.attemptsLeft == 0 && heartsLeft > 1) // loose heart
        {
            heartsLeft--;
            hearts[heartsLeft].attemptsLeft = 0;
        }
        else // reset hearts
        {
            heartsLeft = 3;
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

    private void TrackHeartsLeft()
    {
        if (StatsManager.instance.attemptsLeft % 3 == 0)
        {
            heartsLeft--;
        }
    }
}
