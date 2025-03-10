using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private BasketMaterial basketMaterial;
    private void Awake()
    {
        basketMaterial = GetComponentInParent<BasketMaterial>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            if (collision.attachedRigidbody.linearVelocityY < 0)
            {
                collision.attachedRigidbody.linearVelocity *= 0.25f;
                basketMaterial.scoreColor = Color.green;
            }
        }
    }
}
