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

    public void LooseHeart()
    {
        heartsLeft--;
        hearts[heartsLeft].lifeLost = true;
    }

    public void ResetHearts()
    {
        heartsLeft = 3;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].lifeLost = false;
            hearts[i].ShowHeart();
        }
    }

    public void HideHearts()
    {
        heartsLeft = 3;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].alpha = 0;
        }
    }
}
