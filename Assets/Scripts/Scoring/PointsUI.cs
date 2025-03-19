using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    public static PointsUI Instance { get; private set; }

    private TextMeshPro pointsUIText;
    public int pointsCount {  get; set; }
    private bool hasScoredFlag;
    private bool hasHitBirdFlag;

    private Ball activeBall => GameManager.Instance.activeBall;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        pointsUIText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        gameObject.SetActive(false);      
    }
    private void Update()
    {
        TallyPoints();
        pointsUIText.text = "Total Points: " + pointsCount.ToString();
    }
    private void TallyPoints()
    {
        if (!hasScoredFlag && activeBall.playerScored)
        {
            pointsCount += activeBall.playerHitBird ? 5 : 1;
            hasScoredFlag = true;
        }

        if (!hasHitBirdFlag && activeBall.playerHitBird)
        {
            pointsCount++;
            hasHitBirdFlag = true;
        }

        if (hasScoredFlag && !activeBall.playerScored && !activeBall.playerHitBird)
        {
            hasScoredFlag = false;
            hasHitBirdFlag = false;
        }
    }

    public void ResetGame()
    {
        pointsCount = 0;
    }
}
