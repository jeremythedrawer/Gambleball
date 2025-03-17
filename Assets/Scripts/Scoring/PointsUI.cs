using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    public static PointsUI Instance { get; private set; }

    private TextMeshPro pointsUIText;
    public int pointsCount {  get; set; }
    private bool hasScoredFlag;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        pointsUIText = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (LevelManager.Instance.activeBasket.scoreTrigger.playerScored)
        {
            if (!hasScoredFlag)
            {
                pointsCount++;
                hasScoredFlag = true;
            }
        }
        else
        {
            hasScoredFlag = false;
        }

        pointsUIText.text = "Total Points " + pointsCount.ToString();
    }
}
