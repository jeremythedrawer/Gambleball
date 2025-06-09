using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    private TextMeshPro pointsUIText;

    private void OnEnable()
    {
        pointsUIText = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        pointsUIText.text = "Score: " + StatsManager.instance.currentScore.ToString() + " / " + GameModeManager.instance.currentGameMode.targetScore.ToString();
    }
}
