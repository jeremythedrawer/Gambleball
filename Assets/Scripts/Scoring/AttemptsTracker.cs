using TMPro;
using UnityEngine;

public class AttemptsTracker : MonoBehaviour
{
    private TextMeshPro attemptsText;

    private void Start()
    {
        attemptsText = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        int attempts = GameManager.Instance.attempts;
        if (attempts == 3)
        {
            attempts = 0;
        }
        attemptsText.text = "Attempts: " + attempts.ToString();
    }
}
