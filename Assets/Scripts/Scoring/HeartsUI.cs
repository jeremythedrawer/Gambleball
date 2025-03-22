using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance { get; private set; }

    private HeartsMaterial[] hearts = new HeartsMaterial[3];

    public int heartsLeft { get; private set; } = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        hearts = GetComponentsInChildren<HeartsMaterial>();
    }


    public void LooseAttempt()
    {
        hearts[heartsLeft - 1].attemptsLeft--;
    }
    public void LooseHeart()
    {
        heartsLeft--;
        hearts[heartsLeft].attemptsLeft = 0;
    }

    public void ResetHearts()
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

    public void ShowOneHeart()
    {
        heartsLeft = 1;
        hearts[0].ShowHeart();
        hearts[0].attemptsLeft = 3;
    }

    public void ReplenishHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if ( hearts[i].attemptsLeft > 0)
            {
                hearts[i].attemptsLeft = 3;
            }
        }
    }
    public void HideHearts()
    {
        heartsLeft = 3;
        for (int i = 1; i < hearts.Length; i++)
        {
            hearts[i].alpha = 0;
        }
    }
}
