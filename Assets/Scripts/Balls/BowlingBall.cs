using UnityEngine;

public class BowlingBall : Ball
{
    private Rigidbody2D backboard => LevelManager.Instance.activeBasket.backboardRB;
    public float detectionRadius = 0.1f;
    public float smashBackboardThreshold = 6f;
    public LayerMask backboardLayer;
    private void Update()
    {
        DetectBackboardHit();
    }

    private void DetectBackboardHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, backboardLayer);

        if (hit != null && hit.attachedRigidbody == backboard)
        {
            if (rigidBodyBall.linearVelocityX > smashBackboardThreshold)
            {
                backboard.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
