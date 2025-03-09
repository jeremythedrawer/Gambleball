using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private BasketScoreController basketScoreController;

    private void Awake()
    {
        basketScoreController = transform.parent.GetComponentInChildren<BasketScoreController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            if (collision.attachedRigidbody.linearVelocityY < 0)
            {
                collision.attachedRigidbody.linearVelocity *= new Vector2(1.0f, 0.25f);
                TotalScoreController.Instance.totalScore += basketScoreController.basketScore;
            }
        }
    }
}
