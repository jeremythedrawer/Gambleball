using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private BasketMaterial basketMaterial;
    private BoxCollider2D triggerCollider;
    public bool playerScored {  get; set; }
    private bool enteredFromTop;
    private void Start()
    {
        basketMaterial = GetComponentInParent<BasketMaterial>();
        triggerCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            Vector2 contactPoint = collision.ClosestPoint(triggerCollider.bounds.center);

            enteredFromTop = contactPoint.y > triggerCollider.bounds.center.y && 
                             collision.attachedRigidbody.linearVelocityY < 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball") && enteredFromTop)
        {
            Vector2 contactPoint = collision.ClosestPoint(triggerCollider.bounds.center);

            bool exitedBottom = contactPoint.y < triggerCollider.bounds.min.y;
            bool exitedLeft = contactPoint.x < triggerCollider.bounds.min.x;
            bool exitedRight = contactPoint.x > triggerCollider.bounds.max.x;

            if ((exitedBottom || exitedRight || exitedRight) && 
            collision.attachedRigidbody.linearVelocityY < 0)
            {
                collision.attachedRigidbody.linearVelocity *= 0.25f;
                BasketMaterial.scoreColor = Color.green;
                playerScored = true;
            }

        }
    }
}
