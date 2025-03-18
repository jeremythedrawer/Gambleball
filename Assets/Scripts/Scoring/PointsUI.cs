using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    public static PointsUI Instance { get; private set; }

    private TextMeshPro pointsUIText;
    public int pointsCount {  get; set; }
    public bool hasScoredFlag { get; set; }

    private Ball activeBall => LevelManager.Instance.activeBall;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
        if (!hasScoredFlag && (activeBall.playerScored || activeBall.playerHitBird))
        {
            if (activeBall.playerScored)
            {
                if (!activeBall.playerHitBird)
                {
                    pointsCount++;
                }
                else
                {
                    pointsCount += 5;
                }
                hasScoredFlag = true;
            }
        }
        else if (hasScoredFlag && !activeBall.playerScored)
        {
            hasScoredFlag = false;
        }
    }
}
