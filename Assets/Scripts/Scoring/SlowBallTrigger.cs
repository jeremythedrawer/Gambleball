using UnityEngine;

public class SlowBallTrigger : MonoBehaviour
{
    [SerializeField] private float maxVelocity = 2;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            if (Mathf.Abs(collision.attachedRigidbody.linearVelocityX) > maxVelocity)
            {
                collision.attachedRigidbody.linearVelocity = collision.attachedRigidbody.linearVelocity.normalized * maxVelocity;
            }
        }
    }
}
