using UnityEngine;

public class BowlingBall : Ball
{
    [Header("Bowling Ball Parameters")]
    public float backboardDetectionRaduis = 0.1f;
    private Rigidbody2D backboard => GameManager.Instance.activeBasket.backboardRB;
    public float smashBackboardThreshold = 6f;
    public LayerMask backboardLayer;
    public override void Update()
    {
        base.Update();
        DetectBackboardHit();
    }

    private void DetectBackboardHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, backboardDetectionRaduis, backboardLayer);

        if (hit != null)
        {
            if (rigidBodyBall.linearVelocityX > smashBackboardThreshold)
            {
                backboard.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, backboardDetectionRaduis);
    }
}
