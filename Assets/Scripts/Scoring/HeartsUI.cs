using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public static HeartsUI Instance { get; private set; }

    private SpriteRenderer[] hearts = new SpriteRenderer[3];

    public int heartsLeft { get; private set; } = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i] = GetComponentInChildren<SpriteRenderer>();
        }

        gameObject.SetActive(false);
    }

    public void LooseHeart()
    {
        heartsLeft--;
        hearts[heartsLeft].gameObject.SetActive(false);
    }

    public void ResetHearts()
    {
        heartsLeft = 3;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
