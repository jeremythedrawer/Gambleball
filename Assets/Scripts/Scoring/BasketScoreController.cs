using TMPro;
using UnityEngine;

public class BasketScoreController : MonoBehaviour
{
    private TextMeshPro scoreText;
    private Vector2 basketPos;
    private BallMovement ball;

    public int basketScore {  get; private set; }

    private float distanceFromBasket;
    private float maxDistance;
    private void Awake()
    {
        basketPos = transform.parent.transform.position;
        ball = FindFirstObjectByType<BallMovement>();
        scoreText = GetComponent<TextMeshPro>();
    }
    private void Start()
    {
        maxDistance = Vector2.Distance(basketPos, CamData.Instance.bottomLeftWorldPos);
        
    }

    private void Update()
    {
        Vector2 ballPos = ball.newBallPos;
        distanceFromBasket = Vector2.Distance(ballPos, basketPos);
        float normScore = distanceFromBasket / maxDistance;
        basketScore = Mathf.RoundToInt(normScore * 100);

        scoreText.text = basketScore.ToString();
    }
}
