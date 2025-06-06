using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    public static PointsUI Instance { get; private set; }

    private TextMeshPro pointsUIText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        pointsUIText = GetComponent<TextMeshPro>();
    }
    private void Update()
    {
        pointsUIText.text = "Score: " + StatsManager.instance.totalScore.ToString();
    }
}
