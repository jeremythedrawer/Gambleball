using TMPro;
using UnityEngine;

public class AttemptsUI : MonoBehaviour
{
    private TextMeshPro timeUIText;

    private void OnEnable()
    {
        timeUIText = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        timeUIText.text = "ATTEMPTS: " + StatsManager.instance.currentMoneyBallAttempts.ToString();
    }
}
