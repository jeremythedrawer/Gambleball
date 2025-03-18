using UnityEngine;

public class SlowBallTrigger : MonoBehaviour
{
    [SerializeField] private float maxYVelocity = 4;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Mathf.Abs(collision.attachedRigidbody.linearVelocityY) > maxYVelocity)
            {
                Debug.Log(Mathf.Abs(collision.attachedRigidbody.linearVelocityY));
                collision.attachedRigidbody.linearVelocityY = -maxYVelocity;
            }
        }
    }
}
