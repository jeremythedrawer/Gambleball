using TMPro;
using UnityEngine;

public class TotalScoreController : MonoBehaviour
{
    public static TotalScoreController Instance { get; private set; }

    private TextMeshPro totalScoreText;

    public int totalScore {  get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        totalScoreText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        totalScoreText.text = "Score: " + totalScore.ToString();
    }
}
