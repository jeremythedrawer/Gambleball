using TMPro;
using UnityEngine;

public class AttemptsUI : MonoBehaviour
{
    private TextMeshPro AttemptUIText;
    public PlusScoreMaterial plusAttemptsUI;
    private void OnEnable()
    {
        AttemptUIText = GetComponent<TextMeshPro>();
        BallSpawner.onInBasket += EnactPlusAttemptsUI;
    }

    private void OnDisable()
    {
        BallSpawner.onInBasket -= EnactPlusAttemptsUI;
        
    }
    private void Update()
    {
        AttemptUIText.text = "ATTEMPTS: " + StatsManager.instance.currentMoneyBallAttempts.ToString();
    }

    public void EnactPlusAttemptsUI()
    {
        if (BallSpawner.instance.type == BallType.AttemptBoost)
        {
            plusAttemptsUI.alpha = 1;
        }
    }
}
